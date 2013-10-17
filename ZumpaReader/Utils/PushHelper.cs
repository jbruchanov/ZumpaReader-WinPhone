using Coding4Fun.Toolkit.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Notification;
using RemoteLogCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZumpaReader.WebService;

namespace ZumpaReader.Utils
{
    public class PushHelper
    {
        private static string APP_NAME = "ZumpaReader";
        private static string CHANNEL_NAME = "www.scurab.com";

        public static void Register()
        {
            Task t = new Task(() =>
            {
                // Try to find an existing channel
                var channel = HttpNotificationChannel.Find(APP_NAME);

                string pushUri;
                if (null == channel)
                {
                    channel = new HttpNotificationChannel(APP_NAME);
                    channel.ConnectionStatusChanged += channel_ConnectionStatusChanged;
                    channel.ErrorOccurred += channel_ErrorOccurred;
                    // handle Uri notification events                
                    channel.ChannelUriUpdated +=
                            new EventHandler<NotificationChannelUriEventArgs>((o, e) =>
                            {
                                pushUri = e.ChannelUri.ToString();
                                if (!String.Equals(AppSettings.PushURI, pushUri))
                                {
                                    AppSettings.PushURI = pushUri;
                                    if (AppSettings.IsLoggedIn)
                                    {
                                        HttpService.CreateInstance().RegisterPushURI(AppSettings.Login, AppSettings.ZumpaUID, pushUri);
                                    }
                                }
                                Finish(channel);
                            });
                    channel.Open();
                }
                else
                {
                    Finish(channel);
                }
            });
            t.Start();
        }

        static void Finish(HttpNotificationChannel channel)
        {
            string pushUri = channel.ChannelUri.ToString();
            channel.HttpNotificationReceived += new EventHandler<HttpNotificationEventArgs>((o, e) =>
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    //not used..
                    //new ToastPrompt { Title = e.Notification.Channel.ChannelName }.Show();
                });
            });


            if (!channel.IsShellToastBound) { channel.BindToShellToast(); }

            channel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>((o, e) =>
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    string url = e.Collection["wp:Param"];
                    ToastPrompt tp = new ToastPrompt { Title = e.Collection["wp:Text1"], Message = e.Collection["wp:Text2"], TextOrientation = System.Windows.Controls.Orientation.Vertical};
                    tp.Tap += (o1, e1) =>
                    {
                        try
                        {
                            ((PhoneApplicationFrame)Application.Current.RootVisual).Navigate(new Uri(url, UriKind.RelativeOrAbsolute));
                        }
                        catch { }//ignore any error
                    };
                    tp.Show();
                });

            });

            if (!channel.IsShellTileBound) { channel.BindToShellTile(); }
            // handle error events
            channel.ErrorOccurred +=
                new EventHandler<NotificationChannelErrorEventArgs>((o, e) =>
                {
                    RLog.E(typeof(PushHelper), e.Message);
                });
        }

        static void channel_ErrorOccurred(object sender, NotificationChannelErrorEventArgs e)
        {
            Debug.WriteLine(e.Message);
        }

        static void channel_ConnectionStatusChanged(object sender, NotificationChannelConnectionEventArgs e)
        {
            Debug.WriteLine(e.ConnectionStatus);
        }
    }
}
