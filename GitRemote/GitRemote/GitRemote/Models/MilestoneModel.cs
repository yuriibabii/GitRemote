using Prism.Mvvm;
using Xamarin.Forms;

namespace GitRemote.Models
{
    public class MilestoneModel : BindableBase
    {
        private string _title = string.Empty;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        private bool _isDescription;
        public bool IsDescription
        {
            get { return _isDescription; }
            set { SetProperty(ref _isDescription, value); }
        }

        private bool _isActivated;

        public bool IsActivated
        {
            get { return _isActivated; }
            set { SetProperty(ref _isActivated, value); }
        }

        private GridLength _descriptionHeight;
        public GridLength DescriptionHeight
        {
            get
            {
                DescriptionHeight = IsDescription ? 16 : 0;
                return _descriptionHeight;
            }
            set { SetProperty(ref _descriptionHeight, value); }
        }
    }
}
