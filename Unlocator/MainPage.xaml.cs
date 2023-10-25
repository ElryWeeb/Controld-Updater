using System.Net.Http;

namespace Unlocator
{
    public partial class MainPage : ContentPage
    {
        IServiceTest Services;

        bool service = false;

        IDispatcherTimer timer = Application.Current.Dispatcher.CreateTimer();

        private static readonly HttpClient client = new HttpClient();

        public MainPage(IServiceTest Services_)
        {
            InitializeComponent();
            Services = Services_;
            timer.Interval = TimeSpan.FromSeconds(900);
            timer.Tick += (s, e) => UpdateIP();
        }

        private void Connect_Clicked(object sender, EventArgs e)
        {
            if (!service)
            {
                Services.Start();
                timer.Start();
                service = !service;
                UpdateIP();
            }
            else
            {
                Services.Stop();
                Connection.TextColor = Colors.Red;
                Connection.Text = "Not Connected";
                Connect.Text = "Connect";
                service = !service;
                timer.Stop();
            }
        }

        private async void UpdateIP()
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            IEnumerable<ConnectionProfile> profiles = Connectivity.Current.ConnectionProfiles;

            string apikey = Preferences.Default.Get("api_key", "none");

            if (apikey != "none" && apikey != "")
            {
                var values = new Dictionary<string, string>
                { 
                    { "api_key",  apikey }
                };

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("https://unlocator.com/tool/api.php", content);

                var responseString = await response.Content.ReadAsStringAsync();

                if (responseString != "Missing or Invalid API Key")
                {

                    if (accessType == NetworkAccess.Internet)
                    {
                        if (profiles.Contains(ConnectionProfile.WiFi))
                        {

                            Connection.TextColor = Colors.Green;
                            Connection.Text = responseString;
                            Connect.Text = "Disconnect";
                        }
                        else
                        {
                            Services.Stop();
                            Connection.TextColor = Colors.Red;
                            Connection.Text = "No WLAN";
                            Connect.Text = "Connect";
                            service = !service;
                            timer.Stop();
                        }
                    }
                    else
                    {
                        Services.Stop();
                        Connection.TextColor = Colors.Red;
                        Connection.Text = "No Internet";
                        Connect.Text = "Connect";
                        service = !service;
                        timer.Stop();
                    }
                }
                else
                {
                    Services.Stop();
                    Connection.TextColor = Colors.Red;
                    Connection.Text = "Apikey Missing";
                    Connect.Text = "Connect";
                    Connect.IsVisible = false;
                    Setup.Text = "Settings";
                    Setup.IsVisible = true;
                    service = !service;
                    timer.Stop();
                }
            }
        }

        private async void Setup_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Settings");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Services.Start();
            Services.Stop();

            string hassettings = Preferences.Default.Get("api_key", "none");

            if (hassettings != "none" && hassettings != "") 
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