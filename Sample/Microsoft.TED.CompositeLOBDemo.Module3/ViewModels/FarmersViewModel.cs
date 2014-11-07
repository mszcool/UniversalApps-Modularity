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
using Microsoft.TED.CompositeLOBDemo.Repository.Interfaces.Models;
using Microsoft.TED.CompositeLOBDemo.SharedModule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.TED.CompositeLOBDemo.Module3.ViewModels
{
    public sealed class FarmersViewModel : Screen
    {
        private readonly IFarmerRepository _farmerRepo;
        private readonly IAppInsightsService _appInsightsService;

        public int NumberOfFarmers { get; set; }
        public ObservableCollection<FarmerModel> Farmers { get; set; }

        private FarmerModel _currentFarmer = null;
        public FarmerModel CurrentFarmer {
            get
            {
                return _currentFarmer;
            }

            set
            {
                _currentFarmer = value;
                if (value != null)
                    CanUpdateFarmer = true;
                else
                    CanUpdateFarmer = false;
            }
        }

        public FarmersViewModel(IFarmerRepository farmerRepository, IAppInsightsService appInsights)
        {
            _farmerRepo = farmerRepository;
            _appInsightsService = appInsights;
            CurrentFarmer = null;
        }

        protected override void OnActivate()
        {
            RefreshAsync();
        }

        private async Task RefreshAsync()
        {
            _appInsightsService.LogEvent("farmers/display", "Displaying farmers from local repository!");

            // Loading the farmers from the repository
            var farmers = await _farmerRepo.GetFarmers();
            var farmersObservable = new ObservableCollection<FarmerModel>(farmers);

            // Next add them to the observable collection of this view model
            Execute.OnUIThread(() =>
            {
                this.Farmers = farmersObservable;
            });
        }

        public bool CanSynchronize()
        {
            return true;
        }

        public async Task Synchronize()
        {
            var syncEventInsights = _appInsightsService.LogStartEvent("farmers/sync", "Start syncing farmers to/from backend...");

            try
            {
                // First synchronize with the backend service
                await _farmerRepo.SyncWithBackend();

                // Next refresh the UI
                await RefreshAsync();
            }
            catch (Exception ex)
            {
                _appInsightsService.LogError("farmers/sync/error", "Unable to synchronize farmers from backend!", ex);
            }
            finally
            {
                _appInsightsService.LogEndEvent(syncEventInsights);
            }
        }

        public bool CanUpdateFarmer { get; set; }

        public async Task UpdateFarmer()
        {
            _appInsightsService.LogEvent("farmers/update", "Updating selected farmer in local repository!");

            // Update the selected farmer
            if (CurrentFarmer != null)
                await _farmerRepo.UpdateFarmer((FarmerModel)CurrentFarmer);
        }
    }
}
