using WPFBasics.Common.ViewModelSupport;

namespace WPFBasics.Common.ExecutionLog
{
    internal class ExecutionLogViewModel : ViewModelNotifyPropertyBase
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
