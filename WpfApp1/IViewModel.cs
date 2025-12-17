using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public interface IViewModel : INotifyPropertyChanged
    {
        event EventHandler? Initialized;
        void Initialize();
    }
}
