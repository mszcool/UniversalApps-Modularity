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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.TED.CompositeLOBDemo.RepositoryMock
{
    public sealed class MockFarmerRepository : IFarmerRepository
    {
        private List<FarmerModel> _mockFarmers;

        public MockFarmerRepository()
        {

        }

        public Task<IList<Repository.Interfaces.Models.FarmerModel>> GetFarmers()
        {
            return Task.FromResult<IList<FarmerModel>>(_mockFarmers);
        }

        public Task UpdateFarmer(Repository.Interfaces.Models.FarmerModel farmerToUpdate)
        {
            var existingFarmer = (from f in _mockFarmers
                                  where f.Id == farmerToUpdate.Id
                                  select f).FirstOrDefault();
            if (existingFarmer != null)
            {
                _mockFarmers.Remove(existingFarmer);
            }
            _mockFarmers.Add(farmerToUpdate);

            return Task.FromResult<object>(null);
        }

        public Task SyncWithBackend()
        {
            _mockFarmers = new List<FarmerModel>();
            for (int i = 0; i < 10; i++)
            {
                _mockFarmers.Add
                    (
                        new FarmerModel()
                        {
                            Id = i,
                            Country = string.Format("Country {0}", i),
                            Firstname = string.Format("First {0}", i),
                            Lastname = string.Format("Last {0}", i),
                            Speciality = string.Format("Speciality {0}", i % 2),
                            HasAnimals = (i % 2 == 0),
                            HasWholeGrainFields = (i % 3 == 0),
                            HasWineyards = (i % 4 == 0)
                        }
                    );
            }

            return Task.FromResult<object>(null);
        }
    }
}
