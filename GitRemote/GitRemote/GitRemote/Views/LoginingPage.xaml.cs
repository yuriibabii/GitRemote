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
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this,  new PropertyChangedEventArgs(propertyName));
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
                if (value == null) throw new ArgumentNullException(nameof(value));
                _imagePath = value;
                OnPropertyChanged("ImagePath");
            } 
        }

        private bool IsPasswordVisible { get; set; }

        private void OnShowPasswordTapped()
        {
            ImagePath = IsPasswordVisible ? "btn_stat_notify_checkbox_square_unchecked.png" : "btn_Green_check_mark.png";
            IsPasswordVisible = !IsPasswordVisible;          
        }
    }
}
