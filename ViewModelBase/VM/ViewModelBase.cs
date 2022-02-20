using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;

namespace ViewModelBaseLib.VM
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Fires when property is changed
        /// </summary>
        /// <param name="Prop"></param>
        public void OnPropertyChanged(string Prop)
        {
            var temp = Volatile.Read(ref PropertyChanged);

            temp?.Invoke(this, new PropertyChangedEventArgs(Prop));
        }

        /// <summary>
        /// Sets the field if it is not equal to value and calls PropertChanged
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public bool SetValue<T>(ref T field, T value, string propName)
        {
            if (Equals(field, value))
            {
                return false;
            }
            else
            {
                field = value;
                OnPropertyChanged(propName);
                return true;
            }
        }
    }
}
