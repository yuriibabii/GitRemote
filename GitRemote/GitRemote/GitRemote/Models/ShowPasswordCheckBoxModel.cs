namespace GitRemote.Models
{
    public class ShowPasswordCheckBoxModel
    {
        private const string CheckedImageSource = "btn_Green_check_mark.png";
        private const string UncheckedImageSource = "btn_stat_notify_checkbox_square_unchecked.png";

        public bool IsChecked = true;
        public string ImageSource = UncheckedImageSource;

        public void ChangeCheckedProperty()
        {
            IsChecked = !IsChecked;
        }

        public void ChangeImageState()
        {
            ImageSource = ImageSource == UncheckedImageSource ? CheckedImageSource : UncheckedImageSource;
        }
    }
}
