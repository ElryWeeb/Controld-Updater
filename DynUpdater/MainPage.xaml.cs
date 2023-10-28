using Android.Telecom;
using System.Net.Http;

namespace DynUpdater
{
    public partial class MainPage : ContentPage
    {

        private static readonly HttpClient client = new HttpClient();

        string ip;

        public MainPage()
        {
            InitializeComponent();
            Startup();
        }

        public void Startup()
        {
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

        private void Connect_Clicked(object sender, EventArgs e)
        {
            UpdateIP();
        }

        private async void UpdateIP()
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            IEnumerable<ConnectionProfile> profiles = Connectivity.Current.ConnectionProfiles;

            string apikey = Preferences.Default.Get("api_key", "none");


            if (apikey != "none" && apikey != "")
            {
                System.Net.WebRequest req = System.Net.WebRequest.Create(apikey);
                System.Net.WebResponse resp = req.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                string responseString = sr.ReadToEnd().Trim();

                Connection.Text = responseString;

                if (responseString == "OK")
                {

                    if (accessType == NetworkAccess.Internet)
                    {
                        if (profiles.Contains(ConnectionProfile.WiFi))
                        {
                            Connection.TextColor = Colors.Green;
                            Connection.Text = "IP applied";
                        }
                        else
                        {
                            Connection.TextColor = Colors.Orange;
                            Connection.Text = "No WLAN - No Update";
                        }
                    }
                    else
                    {
                        Connection.TextColor = Colors.Red;
                        Connection.Text = "No Internet - No Update";
                    }
                }
                else
                {
                    Connection.TextColor = Colors.Red;
                    Connection.Text = responseString;
                    Connect.IsVisible = false;
                    Setup.Text = "Settings";
                    Setup.IsVisible = true;
                }
            }
        }


        private async void Setup_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Settings());
        }
    }
}