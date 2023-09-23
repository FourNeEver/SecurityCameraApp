using System.Text.RegularExpressions;

namespace SecurityCameraApp;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();
	}

    private async void Confirm_Clicked(object sender, EventArgs e)
    {
        App.Current.Resources["Adress"] = Adress.Text;
        App.Current.Resources["VPort"] = VPort.Text;
        App.Current.Resources["CPort"] = CPort.Text;
        App.Current.Resources["TR1"] = TR1.Text;
        App.Current.Resources["MS1"] = MS1.Text;
        App.Current.Resources["TR2"] = TR2.Text;
        App.Current.Resources["MS2"] = MS2.Text;
        await Shell.Current.GoToAsync("..");
    }

    private void VPort_TextChanged(object sender, TextChangedEventArgs e)
    {
        var regexItem = new Regex("^\\d{1,5}$");
        if (!regexItem.IsMatch(e.NewTextValue))
        {
            
            VPortError.IsVisible = true;
            Confirm.IsEnabled= false;
        }
        else
        {
            VPortError.IsVisible = false;
            Confirm.IsEnabled = true;
        }
    }

    private void picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        App.Current.Resources["RS1"] = picker.SelectedItem;
    }

    private void picker2_SelectedIndexChanged(object sender, EventArgs e)
    {
        App.Current.Resources["RS2"] = picker2.SelectedItem;
    }

    private void CPort_TextChanged(object sender, TextChangedEventArgs e)
    {
        var regexItem = new Regex("^\\d{1,5}$");
        if (!regexItem.IsMatch(e.NewTextValue))
        {

            CPortError.IsVisible = true;
            Confirm.IsEnabled = false;
        }
        else
        {
            CPortError.IsVisible = false;
            Confirm.IsEnabled = true;
        }
    }

    private void OnEntryIPChanged(object sender, TextChangedEventArgs e)
    {
        var regexItem = new Regex("^\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}$");
        if (!regexItem.IsMatch(e.NewTextValue))
        {
            IpError.IsVisible = true;
            Confirm.IsEnabled = false;
        }
        else
        {
            IpError.IsVisible = false;
            Confirm.IsEnabled = true;
        }
    }
}