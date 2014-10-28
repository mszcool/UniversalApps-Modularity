using Microsoft.TED.CompositeLOBDemo.Repository.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.TED.CompositeLOBDemo.Repository.Interfaces
{
    public enum FarmerRepositoryConstants
    {
        FarmerRepository
    }

    public interface IFarmerRepository
    {
        Task<IList<FarmerModel>> GetFarmers();
        Task UpdateFarmer(FarmerModel farmerToUpdate);
        Task SyncWithBackend();
    }
}
