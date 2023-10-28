using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using static Java.Interop.JniEnvironment;

namespace ControldUpdater;
public partial class Settings : ContentPage
{ 
	public Settings()
	{
		InitializeComponent();
	}

    protected override bool OnBackButtonPressed()
    {
        string savedapi = Preferences.Default.Get("api_key", "none");
        string saveddevice = Preferences.Default.Get("device_key", "none");

        if (savedapi == ApiKey.Text ||saveddevice == DeviceKey.Text)
        {
            Application.Current.Quit();
        } else
        {
            string text = "Settings not saved, please save or revert changes.";
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(text, duration, fontSize);
            toast.Show();
        }

        return true;

    }


    protected override void OnAppearing()
    {
        base.OnAppearing();

        ApiKey.Text = Preferences.Default.Get("api_key", "");
        DeviceKey.Text = Preferences.Default.Get("device_key", "");

    }

    private void Save_Button_Clicked(object sender, EventArgs e)
    {
        Preferences.Default.Set("api_key", ApiKey.Text);
        Preferences.Default.Set("device_key", DeviceKey.Text);
    }

    private void Revert_Button_Clicked(object sender, EventArgs e)
    {
        ApiKey.Text = Preferences.Default.Get("api_key", "");
        DeviceKey.Text = Preferences.Default.Get("device_key", "");
    }
}

