using System;

using Xamarin.Forms;

namespace GitRemote.Views
{
    public partial class LoginingPage : ContentPage
    {
        public LoginingPage()
        {
            InitializeComponent();
        }
    }

    public class MaterialEntry : Entry { }

    public class CheckBoxImage : ContentView
    {
        
        Label _lblText = new Label() { Text = "This is UnChecked" };
        private Image _image = new Image();

        public CheckBoxImage()
        {
            //Im.Source = "btn_stat_notify_checkbox-square-unchecked.png";
            //StackLayout stklayout = new StackLayout()
            //{
            //    //Orientation = StackOrientation.Horizontal,
            //    //Children =
            //    //{
            //    //    Im,
            //    //    lblText
            //    //}
            //};

            //TapGestureRecognizer t = new TapGestureRecognizer();
            //t.Tapped += OnScreenTapped;
            //stklayout.GestureRecognizers.Add(t);

            //Content = stklayout;

        }

        public void OnScreenTapped(object sender, EventArgs args)
        {
            var resourceName = Resources["ImageName"];
            if ( resourceName.ToString() == "btn_stat_notify_checkbox-square-unchecked" )
            {
                Resources["ImageName"] = "btn_stat_notify_Green_check_mark";
                _image.Source = ImageSource.FromResource("btn_stat_notify_Green_check_mark");
                _lblText.Text = "This is Checked";

            }
            else
            {
                Resources["ImageName"] = "btn_stat_notify_checkbox-square-unchecked";
                _image.Source = ImageSource.FromResource("btn_stat_notify_checkbox-square-unchecked");
                _lblText.Text = "This is UnChecked";

            }


        }
    }
}
