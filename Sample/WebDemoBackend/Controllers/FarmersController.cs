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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebDemoBackend.Controllers
{
    public class Farmer
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

    public class FarmersController : ApiController
    {
        private static List<Farmer> _myFarmers = new List<Farmer>
        {
            new Farmer() { Id = 1,  Firstname="Mario", Lastname="Szp", Speciality="Wine", HasAnimals=true, HasWholeGrainFields=false, HasWineyards=true, Country="Austria" },
            new Farmer() { Id = 2,  Firstname="Marlies", Lastname="Str", Speciality="Free Bio Animal", HasAnimals=true, HasWholeGrainFields=true, HasWineyards=false, Country="Austria" },
            new Farmer() { Id = 3,  Firstname="Max", Lastname="Kno", Speciality="Cheese and Milk", HasAnimals=true, HasWholeGrainFields=true, HasWineyards=false, Country="Austria" },
            new Farmer() { Id = 4,  Firstname="Christina", Lastname="Fel", Speciality="Bio Whole Grain", HasAnimals=true, HasWholeGrainFields=true, HasWineyards=false, Country="Austria" },
            new Farmer() { Id = 5,  Firstname="Thomas", Lastname="Cón", Speciality="Wine", HasAnimals=false, HasWholeGrainFields=false, HasWineyards=true, Country="France" },
            new Farmer() { Id = 6,  Firstname="Yuri", Lastname="Gold", Speciality="Whole Grain", HasAnimals=false, HasWholeGrainFields=true, HasWineyards=true, Country="Russia" },
            new Farmer() { Id = 7,  Firstname="Rachel", Lastname="Yehe", Speciality="Forest", HasAnimals=false, HasWholeGrainFields=false, HasWineyards=false, Country="Israel" },
            new Farmer() { Id = 8,  Firstname="Robert", Lastname="Eich", Speciality="Cow and Milk", HasAnimals=true, HasWholeGrainFields=true, HasWineyards=false, Country="Germany" },
            new Farmer() { Id = 9,  Firstname="Jürgen", Lastname="Peif", Speciality="Bio Science", HasAnimals=true, HasWholeGrainFields=true, HasWineyards=true, Country="Germany" },
            new Farmer() { Id = 10,  Firstname="Petra", Lastname="Klei", Speciality="Grain", HasAnimals=false, HasWholeGrainFields=true, HasWineyards=false, Country="Austria" },
            new Farmer() { Id = 11,  Firstname="Xavier", Lastname="Pou", Speciality="Wine", HasAnimals=false, HasWholeGrainFields=false, HasWineyards=true, Country="France" },
            new Farmer() { Id = 12,  Firstname="David", Lastname="Gris", Speciality="Beer", HasAnimals=false, HasWholeGrainFields=true, HasWineyards=true, Country="United Kingdom" },
            new Farmer() { Id = 13,  Firstname="Kristofer", Lastname="Lil", Speciality="Forest", HasAnimals=true, HasWholeGrainFields=false, HasWineyards=false, Country="Sweden" },
            new Farmer() { Id = 14,  Firstname="Nuna", Lastname="Cos", Speciality="Bio Animal", HasAnimals=false, HasWholeGrainFields=false, HasWineyards=false, Country="Israel" },
            new Farmer() { Id = 15,  Firstname="Jason", Lastname="Sho", Speciality="Forest", HasAnimals=false, HasWholeGrainFields=false, HasWineyards=true, Country="United States" }
        };

        public IEnumerable<Farmer> Get()
        {
            return _myFarmers;
        }

        public Farmer Get(int id)
        {
            var farmer = (from f in _myFarmers
                          where f.Id == id
                          select f).FirstOrDefault();
            if (farmer == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            else
                return farmer;
        }

        // Note: For simplicity of this demo, PUT is doing insert or update. 
        //       For production, please use PUT for update and POST for INSERT.
        public void Put(int id, [FromBody]Farmer value)
        {
            lock (_myFarmers)
            {
                var farmer = (from f in _myFarmers
                              where f.Id == id
                              select f).FirstOrDefault();
                if (farmer != null)
                {
                    value.Id = id;
                    _myFarmers.Remove(farmer);
                    _myFarmers.Add(value);
                }
                else
                {
                    var newId = _myFarmers.Max(f => f.Id) + 1;
                    value.Id = newId;
                    _myFarmers.Add(value);
                }
            }
        }

        public void Delete(int id)
        {
            lock (_myFarmers)
            {
                var farmer = _myFarmers.Where(f => f.Id == id).FirstOrDefault();
                if (farmer != null)
                    _myFarmers.Remove(farmer);
                else
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }
    }
}
