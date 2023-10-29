using Android.Accounts;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using GoogleGson;
using Newtonsoft.Json;
using Org.Json;
using RestSharp;
using System.Linq;

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
                var options = new RestClientOptions("https://ipv4.icanhazip.com/");
                var client = new RestClient(options);
                var request = new RestRequest("");
                request.Timeout = 1000;
                string ipv4 = "";
                string ipv6 = "";

                var response = await client.ExecutePostAsync(request);

                if (response.Content != null)
                {
                    ipv4 = response.Content.Trim();

                }

                options = new RestClientOptions("https://ipv6.icanhazip.com/");
                client = new RestClient(options);
                request = new RestRequest("");
                request.Timeout = 1000;

                response = await client.ExecutePostAsync(request);

                if (response.Content != null)
                {
                    ipv6 = response.Content.Trim();

                }

                if (ipv4 == "")
                { 
                    Connection.TextColor = Colors.Red;
                    Connection.Text = "Fatal Error";
                    MoreInfo.Text = "Couldnt get a IPv4 adress.";
                    Connect.IsEnabled = true;
                    return;
                }
                else if (ipv6 == "")
                {
                    string text = "No IPv6 found, only Updating IPv4.";
                    ToastDuration duration = ToastDuration.Short;
                    double fontSize = 14;
                    var toast = Toast.Make(text, duration, fontSize);
                    await toast.Show();
                }

                options = new RestClientOptions("https://api.controld.com/access");
                client = new RestClient(options);
                request = new RestRequest("");
                request.Timeout = 1000;
                request.AddHeader("Content-Type", "application/json; charset=utf-8");
                request.AddHeader("accept", "application/json");
                request.AddHeader("authorization", "Bearer " + apikey);
                request.AddParameter("device_id", devicekey);
                request.AddParameter("ips[]", ipv4);

                response = await client.ExecutePostAsync(request);

                Rootobject stuff = JsonConvert.DeserializeObject<Rootobject>(response.Content);

                if (stuff.success)
                {
                    if (ipv6 != "")
                    {
                        options = new RestClientOptions("https://api.controld.com/access");
                        client = new RestClient(options);
                        request = new RestRequest("");
                        request.Timeout = 1000;
                        request.AddHeader("Content-Type", "application/json; charset=utf-8");
                        request.AddHeader("accept", "application/json");
                        request.AddHeader("authorization", "Bearer " + apikey);
                        request.AddParameter("device_id", devicekey);
                        request.AddParameter("ips[]", ipv6);

                        response = await client.ExecutePostAsync(request);

                        Rootobject stuffv6 = JsonConvert.DeserializeObject<Rootobject>(response.Content);

                        if (stuffv6.success)
                        {
                            Connection.TextColor = Colors.Green;
                            Connection.Text = "IPv4 & v6 applied";
                            MoreInfo.Text = ipv4 + "\n" + ipv6;
                        }
                        else
                        {
                            Connection.TextColor = Colors.Orange;
                            Connection.Text = "IPv4 applied, IPv6 failed";
                            MoreInfo.Text = ipv4 + "\n --------- \n" + stuffv6.error.message;
                        }
                    }
                    else
                    {
                        Connection.TextColor = Colors.Green;
                        Connection.Text = "IPv4 applied";
                        MoreInfo.Text = ipv4;
                    }
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