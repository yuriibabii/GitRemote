using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace GitRemote.Views
{
    public partial class LoginingPage : ContentPage
    {
        public LoginingPage()
        {
            InitializeComponent();
            BindingContext = new ShowPasswordCheckBox();
        }

    }

    public class MaterialEntry : Entry { }

    public class ShowPasswordCheckBox : INotifyPropertyChanged
    {
        public ShowPasswordCheckBox()
        {
            //ImageWidth = Binding.
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand TapShowPassword { protected set; get; }

        public ShowPasswordCheckBox()
        {
            TapShowPassword = new Command(OnShowPasswordTapped);
        }

        private string _imagePath = "btn_stat_notify_checkbox_square_unchecked.png";

        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                if ( value == null ) throw new ArgumentNullException(nameof(value));
                _imagePath = value;
                OnPropertyChanged(nameof(ImagePath));
            }
        }

        private double _imageHeight = 0;

        public double ImageHeight
        {
            get { return _imageHeight; }
            set
            {
                _imageHeight = value;
                OnPropertyChanged(nameof(ImageHeight));
            }
        }

        private double _imageWidth = 0;

        public double ImageWidth
        {
            get { return _imageWidth; }
            set
            {
                _imageWidth = value;
                OnPropertyChanged(nameof(ImageWidth));
            }
        }

        private bool _isPasswordUnVisible = true;

        public bool IsPasswordUnVisible
        {
            get { return _isPasswordUnVisible; }
            set
            {
                _isPasswordUnVisible = value;
                OnPropertyChanged(nameof(IsPasswordUnVisible));
            }
        }

        /// <summary>
        /// Doing what is needing after CheckBox tap
        /// </summary>
        private void OnShowPasswordTapped()
        {
            ImagePath = IsPasswordUnVisible ? "btn_Green_check_mark.png" : "btn_stat_notify_checkbox_square_unchecked.png";
            IsPasswordUnVisible = !IsPasswordUnVisible;
        }
    }
}
