using Android.Telecom;
using System.Net.Http;

namespace DynUpdater
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
            Startup();
            timer.Interval = TimeSpan.FromSeconds(600);
            timer.Tick += (s, e) => UpdateIP();
        }

        public void Startup()
        {
            string hassettings = Preferences.Default.Get("api_key", "none");

            Services.Start();
            Services.Stop();

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
                req = System.Net.WebRequest.Create(apikey);
                resp = req.GetResponse();
                sr = new System.IO.StreamReader(resp.GetResponseStream());
                string responseString = sr.ReadToEnd().Trim();

                Connection.Text = responseString;

                if (responseString == "OK")
                {

                    if (accessType == NetworkAccess.Internet)
                    {
                        if (profiles.Contains(ConnectionProfile.WiFi))
                        {
                            if (ip != responseip)
                            {
                                Connection.TextColor = Colors.Green;
                                Connection.Text = "new IP applied";
                                Connect.Text = "Stop Service";
                                ip = responseip;
                            } else
                            {
                                Connection.TextColor = Colors.Green;
                                Connection.Text = "IP already Synced";
                                Connect.Text = "Stop Service";
                                ip = responseip;
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
            await Navigation.PushAsync(new Settings());
        }
    }
}