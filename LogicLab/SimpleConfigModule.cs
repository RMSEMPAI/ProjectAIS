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
            Bind<IRepository<ITEmployee>>().To<EntityRepository<ITEmployee>>().InSingletonScope();
            Bind<IRepository<Language>>().To<EntityRepository<Language>>().InSingletonScope();
        }
    }
    public class SimpleConfigModuleDapper : NinjectModule
    {
        public override void Load()
        {
            Bind<IRepository<ITEmployee>>().To<DapperRepository<ITEmployee>>().InSingletonScope();
            Bind<IRepository<Language>>().To<DapperRepository<Language>>().InSingletonScope();
        }
    }
}
