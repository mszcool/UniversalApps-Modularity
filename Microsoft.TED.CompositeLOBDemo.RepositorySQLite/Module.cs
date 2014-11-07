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
using Caliburn.Micro;
using Microsoft.TED.CompositeLOBDemo.Repository.Interfaces;
using Microsoft.TED.WinRT.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.TED.CompositeLOBDemo.RepositorySQLite
{
    public sealed class Module : IModule
    {
        private readonly SimpleContainer _container;

        public Module(SimpleContainer container)
        {
            _container = container;    
        }

        public async System.Threading.Tasks.Task InitializeAsync()
        {
            var repo = await SQLiteFarmerRepository.CreateRepository();
            _container.RegisterInstance
                (
                    typeof(IFarmerRepository),
                    FarmerRepositoryConstants.FarmerRepository.ToString(),
                    repo
                );
        }
    }
}
