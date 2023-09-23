namespace SecurityCameraApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute("MainPage", typeof(MainPage));
        Routing.RegisterRoute("CameraFeed", typeof(CameraFeed));
        Routing.RegisterRoute("SettingsPage", typeof(SettingsPage));

    }
}
