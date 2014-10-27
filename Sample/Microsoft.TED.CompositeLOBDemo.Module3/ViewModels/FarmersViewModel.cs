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
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.TED.CompositeLOBDemo.Module3.ViewModels
{
    public sealed class FarmersViewModel : Screen
    {
        private readonly IFarmerRepository _farmerRepo;

        public int NumberOfFarmers { get; set; }
        public ObservableCollection<FarmerModel> Farmers { get; set; }

        public FarmersViewModel(IFarmerRepository farmerRepository)
        {
            _farmerRepo = farmerRepository;
        }

        protected override void OnActivate()
        {
            RefreshAsync();
        }

        private async void RefreshAsync()
        {
            Task.Run(() =>
            {
                // Loading the farmers from the repository
                var farmers = _farmerRepo.GetFarmers();
                var farmersObservable = new ObservableCollection<FarmerModel>(farmers);

                // Next add them to the observable collection of this view model
                Execute.OnUIThread(() =>
                {
                    this.Farmers = farmersObservable;
                });
            });
        }
    }
}
