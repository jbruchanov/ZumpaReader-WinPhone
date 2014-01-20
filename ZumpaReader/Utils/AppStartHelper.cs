using RemoteLogCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZumpaReader.WebService;

namespace ZumpaReader.Utils
{
    public class AppStartHelper
    {
        public static async void RefreshServerConfigValue(IWebService service)
        {
            bool imageAsButton = AppSettings.ShowImageAsButton;
            bool showImages = AppSettings.ShowSettingsAutoLoadImages;
            
            try
            {
                AppSettings.ShowImageAsButton = AppSettings.ShowSettingsAutoLoadImages = false;
                ZumpaReader.WebService.WebService.ContextResult<Dictionary<string, object>> result = await service.GetConfig();

                if (!result.HasError)
                {
                    Dictionary<string, object> data = result.Context;
                    if(data.ContainsKey(AppSettings.PropertyKeys.ShowImageAsButton.ToString()))
                    {
                        AppSettings.ShowImageAsButton = Convert.ToBoolean(data[AppSettings.PropertyKeys.ShowImageAsButton.ToString()]);
                    }
                    if (data.ContainsKey(AppSettings.PropertyKeys.ShowSettingsAutoLoadImages.ToString()))
                    {
                        AppSettings.ShowSettingsAutoLoadImages = Convert.ToBoolean(data[AppSettings.PropertyKeys.ShowSettingsAutoLoadImages.ToString()]);
                    }
                }
            }
            catch (Exception e)
            {
                //do nothing
                RLog.E(typeof(AppStartHelper), e, "Loading config from server");
            }
        }
    }
}
