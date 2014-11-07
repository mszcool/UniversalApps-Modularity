// 
// Copyright (c) Microsoft.  All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//           http://www.apache.org/licenses/LICENSE-2.0 
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
using Microsoft.TED.CompositeLOBDemo.Repository.Interfaces;
using Microsoft.TED.CompositeLOBDemo.Repository.Interfaces.Models;
using SQLitePCL;
using SQLitePCL.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Microsoft.TED.CompositeLOBDemo.RepositorySQLite
{
    public sealed class SQLiteFarmerRepository : IFarmerRepository
    {
        private const string DBNAME = "farmerDatabase.sqlite";
        private const string WEBROOT = "http://localhost:11355/";

        private SQLiteConnection Connection { get; set; }

        private SQLiteFarmerRepository()
        {
            Connection = new SQLiteConnection(DBNAME);
        }

        #region IFarmerRepository Implementation

        public Task<IList<Repository.Interfaces.Models.FarmerModel>> GetFarmers()
        {
            return Task.Run<IList<FarmerModel>>(() =>
            {
                return ExecuteQuery();
            });
        }

        public Task UpdateFarmer(Repository.Interfaces.Models.FarmerModel farmerToUpdate)
        {
            return Task.Run(() =>
            {
                PerformInsertUpdate(farmerToUpdate, true);
            });
        }

        public async Task SyncWithBackend()
        {
            await UploadData();
            await DownloadData();
        }

        #endregion

        #region Private/Internal Helper Methods

        internal async static Task<SQLiteFarmerRepository> CreateRepository()
        {
            try
            {
                // Try to find the existing database
                var existingDb = await ApplicationData.Current.LocalFolder.GetFileAsync(DBNAME);
            }
            catch (System.IO.FileNotFoundException)
            {
                // Database does not exist, therefore provision the database
                ProvisionDatabase();
            }
            return new SQLiteFarmerRepository();
        }

        private static void ProvisionDatabase(SQLiteConnection sqConn = null)
        {
            if (sqConn == null)
                sqConn = new SQLitePCL.SQLiteConnection(DBNAME);

            using (var cmdCreateTable = sqConn.Prepare
                                    (
                                        "CREATE TABLE Farmer(" +
                                        "  Id INT, " +
                                        "  Firstname TEXT, " +
                                        "  Lastname TEXT, " +
                                        "  Speciality TEXT, " +
                                        "  HasAnimals BOOLEAN, " +
                                        "  HasWineyards BOOLEAN, " +
                                        "  HasWholeGrainFields BOOLEAN, " +
                                        "  Country TEXT, " +
                                        "  NeedsSync BOOLEAN);"
                                    ))
            {
                var result = cmdCreateTable.Step();
                if (result != SQLiteResult.DONE)
                    throw new Exception(string.Format("Unable to create database, error returned: {0}!", result.ToString()));
            }
        }

        private IList<FarmerModel> ExecuteQuery(bool needsSyncOnly = false)
        {
            var farmers = new List<FarmerModel>();
            var queryText = "SELECT * FROM Farmer";
            if (needsSyncOnly) queryText = string.Concat(queryText, " WHERE NeedsSync = 1");

            using (var queryCmd = Connection.Prepare(queryText))
            {
                while (queryCmd.Step() == SQLiteResult.ROW)
                {
                    var farmer = new FarmerModel()
                    {
                        Id = (int)queryCmd.GetInteger("Id"),
                        Firstname = queryCmd.GetText("Firstname"),
                        Lastname = queryCmd.GetText("Lastname"),
                        Country = queryCmd.GetText("Country"),
                        Speciality = queryCmd.GetText("Speciality"),
                        HasAnimals = ((int)queryCmd.GetInteger("HasAnimals") == 1),
                        HasWholeGrainFields = ((int)queryCmd.GetInteger("HasWholeGrainFields") == 1),
                        HasWineyards = ((int)queryCmd.GetInteger("HasWineyards") == 1)
                    };
                    farmers.Add(farmer);
                }
            }

            return farmers;
        }

        private void PerformInsertUpdate(Repository.Interfaces.Models.FarmerModel farmerToUpdate, bool needsSync)
        {
            var updateCmdSql = string.Empty;
            using (var queryFarmerCmd = Connection.Prepare("SELECT * FROM Farmer WHERE Id = @Id"))
            {
                queryFarmerCmd.Bind("@Id", farmerToUpdate.Id);
                if (queryFarmerCmd.Step() == SQLiteResult.ROW)
                {
                    updateCmdSql = "UPDATE Farmer " +
                                   " SET Firstname = @Firstname, " +
                                   "     Lastname = @Lastname, " +
                                   "     Speciality = @Speciality, " +
                                   "     Country = @Country, " +
                                   "     HasAnimals = @HasAnimals, " +
                                   "     HasWineyards = @HasWineyards, " +
                                   "     HasWholeGrainFields = @HasWholeGrainFields, " +
                                   "     NeedsSync = @NeedsSync" +
                                   " WHERE Id = @Id";
                }
                else
                {
                    updateCmdSql = "INSERT INTO Farmer(Id, Firstname, Lastname, Speciality, Country, HasAnimals, HasWineyards, HasWholeGrainFields, NeedsSync) " +
                                   "VALUES(@Id, @Firstname, @Lastname, @Speciality, @Country, @HasAnimals, @HasWineyards, @HasWholeGrainFields, @NeedsSync)";
                }
            }

            using (var updateCmd = Connection.Prepare(updateCmdSql))
            {
                updateCmd.Bind("@Id", farmerToUpdate.Id);
                updateCmd.Bind("@Firstname", farmerToUpdate.Firstname);
                updateCmd.Bind("@Lastname", farmerToUpdate.Lastname);
                updateCmd.Bind("@Speciality", farmerToUpdate.Speciality);
                updateCmd.Bind("@Country", farmerToUpdate.Country);
                updateCmd.Bind("@HasAnimals", farmerToUpdate.HasAnimals ? 1 : 0);
                updateCmd.Bind("@HasWineyards", farmerToUpdate.HasWineyards ? 1 : 0);
                updateCmd.Bind("@HasWholeGrainFields", farmerToUpdate.HasWholeGrainFields ? 1 : 0);
                updateCmd.Bind("@NeedsSync", needsSync ? 1 : 0);

                var result = updateCmd.Step();
                if (result != SQLiteResult.DONE)
                    throw new Exception(string.Format("Unable to insert/update record with the following result: {0}", result.ToString()));
            }
        }

        private async Task DownloadData()
        {
            // Perform the http GET operations against the web API holding the farmers backend
            var httpClient = new HttpClient();
            var result = await httpClient.GetAsync(string.Concat(WEBROOT, "api/", "Farmers"));
            result.EnsureSuccessStatusCode();

            // De-serialize the response into farmer objects
            var farmers = await result.Content.ReadAsAsync<List<FarmerModel>>();

            // Next delete the existing table and re-created it
            using (var cmdDeleteTable = Connection.Prepare("DROP TABLE Farmer"))
            {
                var deleteResult = cmdDeleteTable.Step();
                if (deleteResult != SQLiteResult.DONE)
                    throw new Exception("Unable to delete existing table with existing content!");
            }
            ProvisionDatabase(Connection);

            // Now insert all farmers downloaded into the database
            foreach (var farmer in farmers)
            {
                PerformInsertUpdate(farmer, false);
            }
        }

        private async Task UploadData()
        {
            // Create the http Client
            var httpClient = new HttpClient();

            // Get all changed records from the database
            var farmers = await Task<FarmerModel>.Run(() =>
            {
                return ExecuteQuery(true);
            });

            // Now upload the resulting farmers
            // Note: this logic could be optimized by batch-uploads, but since this is not the main
            //       target of this sample, we do one-by-one
            var mediaTypeFormatter = new JsonMediaTypeFormatter();
            foreach (var farmerToUpload in farmers)
            {
                var response = await httpClient.PutAsync<FarmerModel>
                                            (
                                                string.Concat(WEBROOT, "api/", "Farmers/", farmerToUpload.Id),
                                                farmerToUpload,
                                                mediaTypeFormatter
                                            );
                response.EnsureSuccessStatusCode();
            }
        }

        #endregion
    }
}
