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

        public System.Threading.Tasks.Task InitializeAsync()
        {
            var repo = new SQLiteFarmerRepository();
            _container.RegisterInstance
                (
                    typeof(IFarmerRepository),
                    FarmerRepositoryConstants.FarmerRepository.ToString(),
                    repo
                );

            return Task.FromResult<object>(null);
        }
    }
}
