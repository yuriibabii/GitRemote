using GitRemote.Models;
using Prism.Commands;
using Prism.Mvvm;

namespace GitRemote.ViewModels
{
    public class LoginingPageViewModel : BindableBase
    {
        private readonly ShowPasswordCheckBox _checkBox;

        public DelegateCommand CheckedCommand { get; private set; }

        public bool IsChecked => _checkBox.IsChecked;
        public string CheckBoxImage => _checkBox.ImageSource;

        public LoginingPageViewModel()
        {
            _checkBox = new ShowPasswordCheckBox();
            CheckedCommand = new DelegateCommand(OnCheckBoxTapped);
        }

        public void OnCheckBoxTapped()
        {
            _checkBox.ChangeCheckedProperty();
            _checkBox.ChangeImageState();
            OnPropertyChanged(nameof(IsChecked));
            OnPropertyChanged(nameof(CheckBoxImage));
        }
    }
}
