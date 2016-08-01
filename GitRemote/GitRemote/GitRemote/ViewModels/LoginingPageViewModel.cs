using GitRemote.Models;
using GitRemote.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;

namespace GitRemote.ViewModels
{
    public class LoginingPageViewModel : BindableBase
    {
        private readonly ShowPasswordCheckBoxModel _checkBox;
        private readonly LogInPageEntriesModel _entries;

        public DelegateCommand CheckedCommand { get; private set; }
        public DelegateCommand LogInCommand { get; private set; }

        public bool IsChecked => _checkBox.IsChecked;
        public string CheckBoxImage => _checkBox.ImageSource;

        public LoginingPageViewModel()
        {
            _checkBox = new ShowPasswordCheckBoxModel();
            _entries = new LogInPageEntriesModel();

            Func<bool> isLogInCommandEnable = () =>
            StringService.CheckForNullOrEmpty(_entries.LoginText, _entries.PasswordText);

            CheckedCommand = new DelegateCommand(OnCheckBoxTapped);
            LogInCommand = new DelegateCommand(OnLogInTapped, isLogInCommandEnable);
        }

        public void OnCheckBoxTapped()
        {
            _checkBox.ChangeCheckedProperty();
            _checkBox.ChangeImageState();
            OnPropertyChanged(nameof(IsChecked));
            OnPropertyChanged(nameof(CheckBoxImage));
        }

        public void OnLogInTapped()
        {

        }
    }
}
