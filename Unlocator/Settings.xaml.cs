using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace Unlocator;
public partial class Settings : ContentPage
{ 
	public Settings()
	{
		InitializeComponent();
	}

    protected override bool OnBackButtonPressed()
    {
        string savedapi = Preferences.Default.Get("api_key", "none");

        if (savedapi == ApiKey.Text || savedapi == "none")
        {
            Shell.Current.Navigation.PopToRootAsync();
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

    }

    private void Save_Button_Clicked(object sender, EventArgs e)
    {
        Preferences.Default.Set("api_key", ApiKey.Text);
    }

    private void Revert_Button_Clicked(object sender, EventArgs e)
    {
        ApiKey.Text = Preferences.Default.Get("api_key", "");
    }
}

