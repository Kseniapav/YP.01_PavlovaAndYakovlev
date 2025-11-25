using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientsYchet
{
    public class Master
    {
        public int IdMaster { get; set; }
        public string Name { get; set; }
        public string Speciality { get; set; }
        public string Number { get; set; }

        public Master() { }

        public Master(int idMaster, string name, string speciality, string number)
        {
            IdMaster = idMaster;
            Name = name;
            Speciality = speciality;
            Number = number;
        }
    }
}
