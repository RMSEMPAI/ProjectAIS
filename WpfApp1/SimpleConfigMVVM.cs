using DataAccessLayer;
using LogicLib;
using LogicLibrary;
using Ninject.Modules;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class SimpleConfigView : NinjectModule
    {
        public override void Load()
        {
            
        }
    }
    public class SimpleConfigViewModel : NinjectModule
    {
        public override void Load()
        {
            string cs = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\stepa\\source\\repos\\Khomkolova\\ProjectAIS\\ProjectAIS\\DataAccessLayer\\Database1.mdf;Integrated Security=True";
            Bind<IRepository<ITEmployee>>().To<EntityRepository<ITEmployee>>().WithConstructorArgument("connectionStr", cs);
            Bind<IRepository<Language>>().To<EntityRepository<Language>>().WithConstructorArgument("connectionStr", cs);
            Bind<IEmployeeModel>().To<Logic>();

        }
    }
}
