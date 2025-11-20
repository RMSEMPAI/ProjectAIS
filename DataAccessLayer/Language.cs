using LogicLab;
using LogicLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class Language : IDomainObject
    {
        public int Id { get; set; }
        public string Name { get; set; }



        public Language()
        {
        }

        public Language(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
