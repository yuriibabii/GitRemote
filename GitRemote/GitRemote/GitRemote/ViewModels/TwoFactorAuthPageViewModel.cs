using GitRemote.DI;
using GitRemote.Models;
using GitRemote.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;

namespace GitRemote.ViewModels
{
    public class TwoFactorAuthPageViewModel : BindableBase
    {
        private readonly IDevice _device;
        private readonly CodePageEntryModel _codePageEntryModel;
        public DelegateCommand HyperLinkTappedCommand { get; }
        public DelegateCommand LogInCommand { get; }

        public string AuthCodeEntryText
        {
            get { return _codePageEntryModel.CodeText; }
            set
            {
                _codePageEntryModel.CodeText = value;
                LogInCommand?.RaiseCanExecuteChanged();
            }
        }

        public TwoFactorAuthPageViewModel(IDevice device)
        {
            _device = device;
            _codePageEntryModel = new CodePageEntryModel();
            HyperLinkTappedCommand = new DelegateCommand(OnHyperLinkTapped);
            Func<bool> isLogInEnable = () => AuthCodeEntryText?.Length == 6;
            LogInCommand = new DelegateCommand(OnLogInTapped, isLogInEnable);
        }

        private async void OnHyperLinkTapped()
        {
            await _device.LaunchUriAsync(new Uri(ConstantsService.TwoFactorAuthUrl));
        }

        private async void OnLogInTapped()
        {

        }
    }
}
