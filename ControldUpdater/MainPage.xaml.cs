using Newtonsoft.Json;
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

            string apikey = Preferences.Default.Get("api_key", "");
            string devicekey = Preferences.Default.Get("device_key", "");


            if (apikey != "" && devicekey != "")
            {
#pragma warning disable SYSLIB0014
                System.Net.WebRequest req = System.Net.WebRequest.Create("http://ifconfig.me");
                System.Net.WebResponse resp = req.GetResponse();
                StreamReader sr = new StreamReader(resp.GetResponseStream());
                string currentip = sr.ReadToEnd().Trim();

                var options = new RestClientOptions("https://api.controld.com/access");
                var client = new RestClient(options);
                var request = new RestRequest("");
                request.AddHeader("Content-Type", "application/json; charset=utf-8");
                request.AddHeader("accept", "application/json");
                request.AddHeader("authorization", "Bearer " + apikey);
                request.AddParameter("device_id", devicekey);
                request.AddParameter("ips[]", currentip);

                var response = await client.ExecutePostAsync(request);


                dynamic stuff = JsonConvert.DeserializeObject(response.Content);

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
        }


        private async void Setup_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Settings());
        }
    }
}