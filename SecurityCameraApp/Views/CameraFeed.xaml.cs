using SecurityCameraApp.Sevices;
using SecurityCameraApp.ViewModels;
using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows.Input;

namespace SecurityCameraApp;

public partial class CameraFeed : ContentPage
{

	public CameraFeed()
	{
		InitializeComponent();

	}

    protected override void OnAppearing()
    {
        if (BindingContext is CameraFeedViewModel vm)
        {
            vm.OnConnect();
        }
    }

}