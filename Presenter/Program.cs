using LogicLib;
using LogicLibrary;
using Microsoft.VisualBasic.ApplicationServices;
using static System.Runtime.InteropServices.JavaScript.JSType;
using DataAccessLayer;
using Laba1;
using Ninject;

namespace Presenter
{
    internal static class Program
    {
        /// <summary>
        ///  Основная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //var cs = "Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename = C:\\Users\\genda\\source\\repos\\ProjectAIS\\DataAccessLayer\\Database1.mdf; Integrated Security = True";
            //var presenter = new PresenterEmployee(new Logic(new EntityRepository<ITEmployee>(cs), new EntityRepository<Language>(cs)), new Form1());
            IKernel kernel = new StandardKernel(new SimpleConfigPresenter());
            var presenter = kernel.Get<PresenterEmployee>();
            if (presenter != null)
            {
                presenter.Run();
            }
        }
    }
}