using Microsoft.TED.CompositeLOBDemo.Repository.Interfaces;
using Microsoft.TED.CompositeLOBDemo.Repository.Interfaces.Models;
using SQLitePCL;
using SQLitePCL.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.TED.CompositeLOBDemo.RepositorySQLite
{
    public sealed class SQLiteFarmerRepository : IFarmerRepository
    {
        public SQLiteFarmerRepository()
        {
            var sqlLiteConn = new SQLiteConnection("testing");
            var cmd = sqlLiteConn.Prepare("CREATE TABLE TEST ( id INT NOT NULL PRIMARY KEY )");
            var result = cmd.Step();
            
        }

        public IList<Repository.Interfaces.Models.FarmerModel> GetFarmers()
        {
            return new List<FarmerModel>();
        }

        public void UpdateFarmer(Repository.Interfaces.Models.FarmerModel farmerToUpdate)
        {
            return;
        }
    }
}
