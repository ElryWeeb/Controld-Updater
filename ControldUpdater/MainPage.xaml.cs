using Android.Accounts;
using GoogleGson;
using Newtonsoft.Json;
using Org.Json;
using RestSharp;

namespace ControldUpdater
{
    public partial class MainPage : ContentPage
    {

        private static readonly HttpClient client = new HttpClient();

        public MainPage()
        {
            InitializeComponent();
            Startup();
        }

        public void Startup()
        {
            string hasapi = Preferences.Default.Get("api_key", "");
            string hasdevice = Preferences.Default.Get("device_key", "");

            if (hasapi != "" && hasapi != "")
            {
                Connect.IsVisible = true;
            }
            else
            {
                Connect.IsVisible = false;
            }
        }

        private void Connect_Clicked(object sender, EventArgs e)
        {
            UpdateIP();
        }

        private async void UpdateIP()
        {
            Connect.IsEnabled = false;
            string apikey = Preferences.Default.Get("api_key", "");
            string devicekey = Preferences.Default.Get("device_key", "");


            if (apikey != "" && devicekey != "")
            {
                string currentipv4 = "";
                string currentipv6 = "";
                string currentip = "";

                var options = new RestClientOptions("https://ipv4.icanhazip.com/");
                var client = new RestClient(options);
                var request = new RestRequest("");
                request.Timeout = 1000;


                var response = await client.ExecutePostAsync(request);

                if (response.Content != null)
                {
                    currentipv4 = response.Content.Trim();
                }

                options = new RestClientOptions("https://ipv6.icanhazip.com/");
                client = new RestClient(options);
                request = new RestRequest("");
                request.Timeout = 1000;

                response = await client.ExecutePostAsync(request);

                if (response.Content != null)
                {
                    currentipv6 = response.Content.Trim();
                }

                if (currentipv6 != "" || currentipv4 != "")
                {
                    if (currentipv6 != "")
                    {
                        currentip = currentipv4 + "," + currentipv6;
                    }
                    else
                    {
                        currentip = currentipv4;
                    }
                }
                else
                {
                    Connection.TextColor = Colors.Red;
                    Connection.Text = "Error";
                    MoreInfo.Text = "Couldnt get any IP Adress.";
                    Connect.IsEnabled = true;
                    return;
                }

                options = new RestClientOptions("https://api.controld.com/access");
                client = new RestClient(options);
                request = new RestRequest("");
                request.Timeout = 1000;
                request.AddHeader("Content-Type", "application/json; charset=utf-8");
                request.AddHeader("accept", "application/json");
                request.AddHeader("authorization", "Bearer " + apikey);
                request.AddParameter("device_id", devicekey);
                request.AddParameter("ips[]", currentip);

                response = await client.ExecutePostAsync(request);

                Rootobject stuff = JsonConvert.DeserializeObject<Rootobject>(response.Content);

                bool gotUpdated = stuff.success;

                if (gotUpdated)
                {
                    Connection.TextColor = Colors.Green;
                    Connection.Text = "IP applied";
                    MoreInfo.Text = currentip;
                }
                else
                {
                    Connection.TextColor = Colors.Red;
                    Connection.Text = "Error";
                    MoreInfo.Text = stuff.error.message;
                }
            }
            Connect.IsEnabled = true;
        }


        private async void Setup_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Settings());
        }
    }
}