using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace Unlocator_DNS;
public partial class Settings : ContentPage
{
    bool issaved = false;
	public Settings()
	{
		InitializeComponent();
	}

    protected override bool OnBackButtonPressed()
    {
        string savedapi = Preferences.Default.Get("api_key", "none");
        bool mobileenabled = Preferences.Default.Get("mobile_risk", false);
        bool savedmobile = Preferences.Default.Get("mobile", false);

        if (savedapi == ApiKey.Text || savedapi == "none")
        {
            if (mobileenabled)
            {
                if (savedmobile == Mobile.IsChecked)
                {
                    Navigation.PushAsync(new MainPage());
                }
                else
                {
                    string text = "Settings not saved, please save or revert changes.";
                    ToastDuration duration = ToastDuration.Short;
                    double fontSize = 14;

                    var toast = Toast.Make(text, duration, fontSize);
                    toast.Show();
                }
            }
            else
            {
                Navigation.PushAsync(new MainPage());
            }
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

    private void Mobile_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (Preferences.Default.Get("mobile_risk", false) == false)
        {
            string text = "This is not adviced, as your mobile IP is shared.";
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(text, duration, fontSize);
            toast.Show();

            text = "If youre sure, tap the checkbox again.";
            toast = Toast.Make(text, duration, fontSize);
            toast.Show();

            Warning.TextColor = Colors.Black;

            Mobile.IsChecked = false;

            Preferences.Default.Set("mobile_risk", true);
        }

        issaved = false;


    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (Preferences.Default.Get("mobile_risk", false) == false)
        {
            Warning.TextColor = Colors.Red;
        }
        else
        {
            Warning.TextColor = Colors.Black;
            Mobile.IsChecked = Preferences.Default.Get("mobile", false);
        }

        ApiKey.Text = Preferences.Default.Get("api_key", "");

    }

    private void Save_Button_Clicked(object sender, EventArgs e)
    {
        Preferences.Default.Set("mobile", Mobile.IsChecked);
        Preferences.Default.Set("api_key", ApiKey.Text);
    }

    private void Revert_Button_Clicked(object sender, EventArgs e)
    {
        Mobile.IsChecked = Preferences.Default.Get("mobile", false);
        ApiKey.Text = Preferences.Default.Get("api_key", "");
    }
}

