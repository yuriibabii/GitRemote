using GitRemote.DI;
using GitRemote.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;

namespace GitRemote.ViewModels
{
    public class TwoFactorAuthPageViewModel : BindableBase
    {
        public DelegateCommand HyperLinkTappedCommand { get; }
        private readonly IDevice _device;

        public TwoFactorAuthPageViewModel(IDevice device)
        {
            _device = device;
            HyperLinkTappedCommand = new DelegateCommand(OnHyperLinkTapped);
        }

        private async void OnHyperLinkTapped()
        {
            await _device.LaunchUriAsync(new Uri(ConstantsService.TwoFactorAuthUrl));
        }
    }
}
