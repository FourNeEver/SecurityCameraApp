using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SecurityCameraApp.Sevices;

namespace SecurityCameraApp.ViewModels
{
    internal class CameraFeedViewModel : BindableObject
    {
        public CameraFeedViewModel() 
        {
            Connect = new Command(OnConnect);
            NavigateSettings = new Command(OnNavigateSettings);
        }

        public ICommand Connect { get; set; }
        public ICommand NavigateSettings { get; set; }

        bool connectEnabled = false;
        string connectStatus = "Connect";
        string frameRate = "No wideo";

        public string ConnectStatus
        {
            get => connectStatus;
            set
            {
                if (value == connectStatus) { return; }
                connectStatus = value;
                OnPropertyChanged();
            }
        }

        public bool ConnectEnabled
        {
            get => connectEnabled;
            set
            {
                if (value == connectEnabled) { return; }
                connectEnabled = value;
                OnPropertyChanged();
            }
        }

        public string FrameRate
        {
            get => frameRate;
            set
            {
                if (value == frameRate) { return; }
                frameRate = value;
                OnPropertyChanged();
            }
        }

        private async void Refresh()
        {

            ConnectEnabled = false;

            var buffer = new byte[1024 * 1000];
            string con = "Connecting...";
            ConnectStatus = con;
            con = await WebHandlingService.StartListeningAsync(App.Current.Resources["Adress"].ToString(), int.Parse(App.Current.Resources["VPort"].ToString()));
            ConnectStatus = con;
            int frames = 0;
            DateTime lastTime = DateTime.Now;

            while (WebHandlingService.Connected)
            {
                WebHandlingService.checkConnectrion();
                if (WebHandlingService.Connected)
                {
                    (App.Current.Resources["Frame"], ConnectStatus) = await WebHandlingService.FetchImage(buffer);
                    frames++;
                    if ((DateTime.Now - lastTime).TotalSeconds >= 1)
                    {
                        FrameRate = frames.ToString() + " FPS";
                        frames = 0;
                        lastTime = DateTime.Now;

                    }
                }

            }
            if (!WebHandlingService.Connected)
            {
                App.Current.Resources["Frame"] = "nofeed.png";
                ConnectEnabled = true;
            }

        }


        public void OnConnect()
        {
            Refresh();
        }

        async void OnNavigateSettings()
        {
            await Shell.Current.GoToAsync("SettingsPage");
        }

    }
}
