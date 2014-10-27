using Caliburn.Micro;
using Microsoft.TED.CompositeLOBDemo.Repository.Interfaces;
using Microsoft.TED.WinRT.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.TED.CompositeLOBDemo.RepositoryMock
{
    public sealed class Module : IModule
    {
        private readonly SimpleContainer _container;

        public Module(SimpleContainer container)
        {
            _container = container;
        }

        public System.Threading.Tasks.Task InitializeAsync()
        {
            _container.RegisterSingleton
                (
                    typeof(IFarmerRepository), 
                    FarmerRepositoryConstants.FarmerRepository.ToString(), 
                    typeof(MockFarmerRepository)
                );

            return Task.FromResult<object>(null);
        }
    }
}
