using Caliburn.Micro;
using Microsoft.TED.CompositeLOBDemo.Repository.Interfaces;
using Microsoft.TED.CompositeLOBDemo.Repository.Interfaces.Models;
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

        public FarmersViewModel(IFarmerRepository farmerRepository)
        {
            _farmerRepo = farmerRepository;
            CurrentFarmer = null;
        }

        protected override void OnActivate()
        {
            RefreshAsync();
        }

        private async Task RefreshAsync()
        {
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
            // First synchronize with the backend service
            await _farmerRepo.SyncWithBackend();

            // Next refresh the UI
            await RefreshAsync();
        }

        public bool CanUpdateFarmer { get; set; }

        public async Task UpdateFarmer()
        {
            // Update the selected farmer
            if (CurrentFarmer != null)
                await _farmerRepo.UpdateFarmer((FarmerModel)CurrentFarmer);
        }
    }
}
