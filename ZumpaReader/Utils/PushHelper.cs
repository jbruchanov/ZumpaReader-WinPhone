using Microsoft.Phone.Notification;
using RemoteLogCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                                AppSettings.PushURI = pushUri;
                                //UpdatePushUri(e.ChannelUri);
                                //if (RemoteLog.PushUriChanged != null)
                                //{
                                //    //RemoteLog.PushUriChanged.Invoke(o, e);
                                //}
                            });
                    channel.Open();
                }
                else
                {

                    // the channel already exists.  httpChannel.ChannelUri contains the device’s
                    // unique locator	

                    pushUri = channel.ChannelUri.ToString();
                    channel.HttpNotificationReceived += new EventHandler<HttpNotificationEventArgs>((o, e) =>
                    {
                        //_pushMessageHandler.OnNotificiationReceived(o, e);
                    });


                    if (!channel.IsShellToastBound) { channel.BindToShellToast(); }

                    channel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>((o, e) =>
                    {
                        //_pushMessageHandler.OnToastNotificiationReceived(o, e);
                    });

                    if (!channel.IsShellTileBound) { channel.BindToShellTile(); }
                    // handle error events
                    channel.ErrorOccurred +=
                        new EventHandler<NotificationChannelErrorEventArgs>((o, e) =>
                        {
                            RLog.E(typeof(PushHelper), e.Message);
                        });
                }
            });
            t.Start();
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
