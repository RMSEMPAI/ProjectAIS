using LogicLib;
using LogicLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ITEmployeeContext : DbContext
    {
        public DbSet<ITEmployee> iTEmployees { get; set; }
        public ITEmployeeContext(DbContextOptions<ITEmployeeContext> options) : base(options)
        {

        }

    }
}
