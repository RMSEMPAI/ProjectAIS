using System.Numerics;
using System.Text.Json;
using DataAccessLayer;
using LogicLib;
using LogicLibrary;
using Microsoft.EntityFrameworkCore;


namespace DBCreate
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var data = JsonSerializer.Deserialize<List<ITEmployee>>(File.ReadAllText("C:\\Users\\stepa\\AppData\\Roaming\\data.json"));
            var optionsBuilder = new DbContextOptionsBuilder<ITEmployeeContext>();

            optionsBuilder.UseSqlServer($"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\stepa\\source\repos\\Khomkolova\\ProjectAIS\\LogicLab\\Database1.mdf;Integrated Security=True");
            var _context = new EntityRepository<ITEmployee>(new ITEmployeeContext(optionsBuilder.Options));

            foreach (var i in data)
                _context.Add(i);

            Console.WriteLine(_context.ReadAll().ElementAt(0));
        }
    }
}
