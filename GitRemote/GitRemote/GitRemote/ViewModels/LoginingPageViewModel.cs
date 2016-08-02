using GitRemote.Models;
using GitRemote.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using Prism.Navigation;
using Xamarin.Forms;

namespace GitRemote.ViewModels
{
    public class LoginingPageViewModel : BindableBase
    {
        private readonly ShowPasswordCheckBoxModel _checkBox;
        private readonly LogInPageEntriesModel _entries;
        private readonly INavigationService _navigationService;

        public DelegateCommand CheckedCommand { get; }
        public DelegateCommand LogInCommand { get; }

        public bool IsUnChecked => _checkBox.IsUnChecked;
        public string CheckBoxImagePath => _checkBox.ImageSource;

        public string LoginEntryText
        {
            get { return _entries.LoginText; }
            set { _entries.LoginText = value;
                LogInCommand.RaiseCanExecuteChanged(); }
        }

        public string PasswordEntryText
        {
            get { return _entries.PasswordText; }
            set { _entries.PasswordText = value;
                LogInCommand.RaiseCanExecuteChanged(); }
        }

        public LoginingPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
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
            OnPropertyChanged(nameof(IsUnChecked));
            OnPropertyChanged(nameof(CheckBoxImagePath));
        }

        public void OnLogInTapped()
        {

        }
    }
}
