using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    public class ViewManager
    {
        public ViewModelManager _viewModelManager;
        private IKernel _service;
        private Dictionary<IViewModel, IView> active_View = new();
        public ViewManager(ViewModelManager viewModelManager, IKernel service)
        {
            _service = service;
            _viewModelManager = viewModelManager;
            _viewModelManager.ViewModelReady += OnViewModelReady;
        }
        private void OnViewModelReady(object? sender, ViewModelEventArgs e)
        {
            if (_viewModelManager != null)
                ShowView(e.ViewModel);
        }
        public Window ShowView(IViewModel vm)
        {
            if (active_View.TryGetValue(vm, out var outview))
            {
                outview.Show();
                return (Window)outview;
            }
            var view = _service.Get(_viewModelManager.GetViewTypeForVM(vm.GetType())) as IView;
            if (view == null)
                throw new Exception();
            view.DataContext = vm;
            active_View[vm] = view;
            view.Show();
            return (Window)view;
        }
        public void CloseView(IViewModel vm)
        {
            if (active_View.TryGetValue(vm, out var view))
            {
                view.Close();
                active_View.Remove(vm);
            }
        }
    }
}
