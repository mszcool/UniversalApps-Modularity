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
        }

        public IList<Repository.Interfaces.Models.FarmerModel> GetFarmers()
        {
            return _mockFarmers;
        }

        public void UpdateFarmer(Repository.Interfaces.Models.FarmerModel farmerToUpdate)
        {
            var existingFarmer = (from f in _mockFarmers
                                  where f.Id == farmerToUpdate.Id
                                  select f).FirstOrDefault();
            if (existingFarmer != null)
            {
                _mockFarmers.Remove(existingFarmer);
            }
            _mockFarmers.Add(farmerToUpdate);
        }
    }
}
