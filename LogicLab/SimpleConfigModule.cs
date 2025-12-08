using DataAccessLayer;
using LogicLibrary;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLib
{
    public class SimpleConfigModule : NinjectModule
    {

        public override void Load()
        {
            string cs = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\stepa\\source\\repos\\Khomkolova\\ProjectAIS\\DataAccessLayer\\Database1.mdf;Integrated Security=True";
            Bind<IRepository<ITEmployee>>().To<EntityRepository<ITEmployee>>().InSingletonScope().WithConstructorArgument("connectionStr", cs);
            Bind<IRepository<Language>>().To<EntityRepository<Language>>().InSingletonScope().WithConstructorArgument("connectionStr", cs);
        }
    }
    public class SimpleConfigModuleDapper : NinjectModule
    {
        public override void Load()
        {
            string cs = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\stepa\\source\\repos\\Khomkolova\\ProjectAIS\\DataAccessLayer\\Database1.mdf;Integrated Security=True";
            Bind<IRepository<ITEmployee>>().To<DapperRepository<ITEmployee>>().InSingletonScope().WithConstructorArgument("connectionStr", cs);
            Bind<IRepository<Language>>().To<DapperRepository<Language>>().InSingletonScope().WithConstructorArgument("connectionStr", cs);
        }
    }
}
