using RemoteLogCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZumpaReader.Utils
{
    public class RLogHelper
    {
        public static void Register()
        {
            string model = new DeviceDataProvider().GetDevice().Model;
            if ("XDeviceEmulator".Equals(model))
            {
                return;
            }
            RLog.Mode = RLog.ALL;
            RemoteLog.RegisterUnhandledExceptionHandler();
            RemoteLog.SetOwner(AppSettings.Login);
            RemoteLog.Resend();
            RemoteLog.Init("ZumpaReaderWP", ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.RemoteLogURL]);
            RemoteLog.RegistrationFinished += (o, e) =>
            {
                //RLog.D(this, "AppStart");
            };
        }
    }
}
