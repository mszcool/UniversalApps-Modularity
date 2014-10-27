using Microsoft.TED.CompositeLOBDemo.Module3.Views;
using Microsoft.TED.CompositeLOBDemo.Repository.Interfaces;
using Microsoft.TED.WinRT.ModularHub;
using Microsoft.TED.WinRT.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.TED.CompositeLOBDemo.Module3
{
    public sealed class Module : IModule
    {
        private readonly IHubMetadataService _hubMetadataService;
        private readonly IFarmerRepository _farmerRepository;

        public Module(IHubMetadataService hubMetadataService, IFarmerRepository farmerRepository) 
        {
            _hubMetadataService = hubMetadataService;
            _farmerRepository = farmerRepository;
        }

        public async Task InitializeAsync()
        {
            _hubMetadataService.AddHubSection
                (
                    "M3", 
                    "Farmers and Animals",
                    typeof(FarmersView).AssemblyQualifiedName
                );

            await Task.FromResult<object>(null);
        }
    }
}
