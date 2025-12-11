using DataAccessLayer;
using LogicLib;
using LogicLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using Shared;
using Laba1;

namespace Presenter
{
    internal class SimpleConfigPresenter: NinjectModule
    {
        public override void Load()
        {
            string cs = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\stepa\\source\\repos\\Khomkolova\\ProjectAIS\\DataAccessLayer\\Database1.mdf;Integrated Security=True";
            Bind<IRepository<ITEmployee>>().To<EntityRepository<ITEmployee>>().InSingletonScope().WithConstructorArgument("connectionStr", cs);
            Bind<IRepository<Language>>().To<EntityRepository<Language>>().InSingletonScope().WithConstructorArgument("connectionStr", cs);
            Bind<IEmployeeModel>().To<Logic>().InSingletonScope();
            Bind<IEmployeeView>().To<Form1>();


        }
    }
}
