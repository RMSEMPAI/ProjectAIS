using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class ViewModelManager
    {
        private IKernel _service;
        private Dictionary<Type, Type> VM_TO_V = new Dictionary<Type, Type>();
        public event EventHandler<ViewModelEventArgs>? ViewModelReady;
        public ViewModelManager(IKernel service)
        {
            _service = service;
        }
        public void RegisterTo<TVM, TV>() where TVM : class, IViewModel where TV : class, IView
        {
            VM_TO_V[typeof(TVM)] = typeof(TV);
        }
        public TVM Create<TVM>() where TVM : class, IViewModel
        {
            var vm = _service.Get(typeof(TVM)) as TVM;
            if (vm == null)
                throw new Exception();
            vm.Initialized += OnViewModelInitialized;
            vm.Initialize();
            return vm;
        }
        private void OnViewModelInitialized(object? sender, EventArgs e)
        {
            if (sender is IViewModel vm)
                ViewModelReady?.Invoke(this, new ViewModelEventArgs(vm));
        }
        public Type GetViewTypeForVM(Type vm_type)
        {
            if (VM_TO_V.TryGetValue(vm_type, out var viewType))
                return viewType;
            throw new Exception(); ;
        }
    }
    public class ViewModelEventArgs : EventArgs
    {
        public IViewModel ViewModel;
        public ViewModelEventArgs(IViewModel viewModel) { this.ViewModel = viewModel; }
    }
}
