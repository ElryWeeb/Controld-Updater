using System.Net.Http;

namespace Unlocator
{
    public partial class MainPage : ContentPage
    {
        IServiceTest Services;

        bool service = false;

        IDispatcherTimer timer = Application.Current.Dispatcher.CreateTimer();

        private static readonly HttpClient client = new HttpClient();

        string ip;

        public MainPage() => Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

        ~MainPage() => Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;

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
                service = true;
                UpdateIP();
            }
            else
            {
                Services.Stop();
                Connection.TextColor = Colors.Red;
                Connection.Text = "Not Connected";
                Connect.Text = "Start Service";
                service = false;
                timer.Stop();
            }
        }

        private async void UpdateIP()
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            IEnumerable<ConnectionProfile> profiles = Connectivity.Current.ConnectionProfiles;

            string apikey = Preferences.Default.Get("api_key", "none");

            System.Net.WebRequest req = System.Net.WebRequest.Create("http://ifconfig.me");
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            string responseip = sr.ReadToEnd().Trim();


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
                            if (ip != responseip)
                            {
                                Connection.TextColor = Colors.Green;
                                Connection.Text = responseString;
                                Connect.Text = "Stop Service";
                            } else
                            {
                                Connection.Text = "IP Synced";
                            }

                        }
                        else
                        {
                            Connection.TextColor = Colors.Orange;
                            Connection.Text = "No WLAN - No Update";
                            Connect.Text = "Stop Service";
                        }
                    }
                    else
                    {
                        Connection.TextColor = Colors.Red;
                        Connection.Text = "No Internet - No Update";
                        Connect.Text = "Stop Service";
                    }
                }
                else
                {
                    Services.Stop();
                    Connection.TextColor = Colors.Red;
                    Connection.Text = "Wrong / Missing Apikey";
                    Connect.Text = "Start Service";
                    Connect.IsVisible = false;
                    Setup.Text = "Settings";
                    Setup.IsVisible = true;
                    service = false;
                    timer.Stop();
                }
            }
        }


        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess != NetworkAccess.Internet)
            {
                foreach (var item in e.ConnectionProfiles)
                {
                    if (item == ConnectionProfile.WiFi)
                    {
                        UpdateIP();
                    }
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