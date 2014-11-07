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
using Microsoft.TED.CompositeLOBDemo.Module3.Views;
using Microsoft.TED.CompositeLOBDemo.Repository.Interfaces;
using Microsoft.TED.CompositeLOBDemo.SharedModule;
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
        private readonly IAppInsightsService _appInsightsService;

        public Module(IHubMetadataService hubMetadataService, IFarmerRepository farmerRepository, IAppInsightsService appInsights) 
        {
            _hubMetadataService = hubMetadataService;
            _farmerRepository = farmerRepository;
            _appInsightsService = appInsights;
        }

        public async Task InitializeAsync()
        {
            _appInsightsService.LogEvent("modules/loadModule3", "Loading module #3 into app...");

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
