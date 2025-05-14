using Zenox.Wpf.Core.Common.MVVM.ViewModelSupport;

namespace Zenox.Wpf.Core.Common.ExecutionLog
{
    internal class ExecutionLogViewModel : NotificationBase
    {
        public List<string> ExecutionLogList
        {
            get { return field; }
            set
            {
                field = value;
                OnPropertyChanged();
            }
        }
    }
}
