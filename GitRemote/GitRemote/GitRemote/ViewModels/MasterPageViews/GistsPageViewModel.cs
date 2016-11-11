using Prism.Commands;
using Prism.Mvvm;

namespace GitRemote.ViewModels.MasterPageViews
{
    public class GistsPageViewModel : BindableBase
    {
        public DelegateCommand AddGistCommand { get; }
        public DelegateCommand RandomGistCommand { get; }

        public GistsPageViewModel()
        {
            AddGistCommand = new DelegateCommand(OnAddGist);
            RandomGistCommand = new DelegateCommand(OnRandomGist);
        }

        private void OnAddGist()
        {

        }

        private void OnRandomGist()
        {

        }
    }

}
