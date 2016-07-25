using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace GitRemote.Views
{
    public partial class LoginingPage
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
        public ICommand TapShowPassword { get; set; }

        public ICommand TapLogIn { get; set; }

        public ShowPasswordCheckBox()
        {
            TapShowPassword = new Command(OnShowPasswordTapped);
            TapLogIn = new Command(OnLogInTapped, CheckLogInEnableState);
        }

        public bool CheckLogInEnableState()
        {
            return !( string.IsNullOrEmpty(LoginEntryText) || string.IsNullOrEmpty(PasswordEntryText) );
        }

        #region
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region
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

        private string _loginEntryText = string.Empty;

        public string LoginEntryText
        {
            get { return _loginEntryText; }
            set
            {
                _loginEntryText = value;
                OnPropertyChanged(nameof(LoginEntryText));
                ( ( Command )TapLogIn ).ChangeCanExecute();
            }
        }

        private string _passwordEntryText = string.Empty;

        public string PasswordEntryText
        {
            get { return _passwordEntryText; }
            set
            {
                _passwordEntryText = value;
                OnPropertyChanged(nameof(PasswordEntryText));
                ( ( Command )TapLogIn ).ChangeCanExecute();
            }
        }
        #endregion



        /// <summary>
        /// Doing what is needing after CheckBox tap
        /// </summary>
        private void OnShowPasswordTapped()
        {
            ImagePath = IsPasswordUnVisible ? "btn_Green_check_mark.png" : "btn_stat_notify_checkbox_square_unchecked.png";
            IsPasswordUnVisible = !IsPasswordUnVisible;

        }

        /// <summary>
        /// Doing what is needing after Log In tap
        /// </summary>
        public void OnLogInTapped()
        {
            //throw new NotImplementedException();
        }
    }


}
