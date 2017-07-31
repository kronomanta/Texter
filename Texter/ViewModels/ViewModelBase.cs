using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace Texter.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        private readonly ReaderWriterLockSlim _locker = new ReaderWriterLockSlim();
        private readonly Dictionary<string, PropertyChangedEventArgs> _propertyChangeArgs = new Dictionary<string, PropertyChangedEventArgs>();

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string member = "")
        {
            PropertyChangedEventArgs e;
            _locker.EnterUpgradeableReadLock();
            try
            {
                if (!_propertyChangeArgs.TryGetValue(member, out e))
                {
                    _locker.EnterWriteLock();
                    try
                    {
                        e = new PropertyChangedEventArgs(member);
                        _propertyChangeArgs.Add(member, e);
                    }
                    finally
                    {
                        _locker.ExitWriteLock();
                    }
                }
            }
            finally
            {
                _locker.ExitUpgradeableReadLock();
            }

            PropertyChanged?.Invoke(this, e);
        }

    }
}
