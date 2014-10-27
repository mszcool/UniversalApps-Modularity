using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.TED.CompositeLOBDemo.Repository.Interfaces.Models
{
    public sealed class FarmerModel
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Speciality { get; set; }
        public bool HasAnimals { get; set; }
        public bool HasWineyards { get; set; }
        public bool HasWholeGrainFields { get; set; }
        public string Country { get; set; }
    }
}
