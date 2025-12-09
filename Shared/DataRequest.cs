using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLib;


namespace Shared
{
    public class DataRequest
    {
        public bool IsPosition {  get; set; }
        public bool IsDepartment { get; set; }
        public bool Promote { get; set; }
        public Position position { get; set; }
        public Department department { get; set; }
        
    }
}
