using Microsoft.TED.CompositeLOBDemo.Repository.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.TED.CompositeLOBDemo.Repository.Interfaces
{
    public enum FarmerRepositoryConstants
    {
        FarmerRepository
    }

    public interface IFarmerRepository
    {
        IList<FarmerModel> GetFarmers();
        void UpdateFarmer(FarmerModel farmerToUpdate);
    }
}
