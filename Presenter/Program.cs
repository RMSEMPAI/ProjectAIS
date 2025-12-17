using LogicLib;
using LogicLibrary;
using Microsoft.VisualBasic.ApplicationServices;
using static System.Runtime.InteropServices.JavaScript.JSType;
using DataAccessLayer;
using Laba1;
using Ninject;
using WpfApp1;

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

            var vmManager = new ViewModelManager(new StandardKernel(new SimpleConfigViewModel()));
            var viewManager = new ViewManager(vmManager, new StandardKernel(new SimpleConfigView()));

            vmManager.RegisterTo<ViewModelMain, MainWindow>();

            var vm = vmManager.Create<ViewModelMain>();

            var window = viewManager.ShowView(vm);
            var app = new System.Windows.Application();

            if (window != null)
            {
                app.Run(window);
            }
        }
    }
}