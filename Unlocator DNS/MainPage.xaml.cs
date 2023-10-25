
namespace Unlocator_DNS
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }


        private void Connect_Clicked(object sender, EventArgs e)
        {

        }

        private async void Setup_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Settings());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            if(Preferences.Default.Get("api_key", "none")  != "none") 
            {
                Connect.IsVisible = true;
                Setup.Text = "Settings";
                Setup.IsVisible = true;
            }
            else
            {
                Setup.IsVisible = true;
            }
        }
    }
}