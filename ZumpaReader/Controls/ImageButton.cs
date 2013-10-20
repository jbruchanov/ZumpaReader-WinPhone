using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ZumpaReader.Commands;
using ZumpaReader.Utils;

namespace ZumpaReader.Controls
{
    public class ImageButton : Button
    {
        private static ImageLoader _loader;

        static ImageButton()
        {
            _loader = new ImageLoader();
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            string link = newContent as string;
            if (AppSettings.AutoLoadImages)
            {
                if (_loader.IsImageLink(link))
                {
                    Content = new ProgressBar { IsIndeterminate = true, MinHeight = 32, MinWidth = 200 };
                    LoadImageAsync(link);
                }
            }
        }

        private async void LoadImageAsync(string link)
        {
            try
            {
                Stream s = await _loader.LoadAsync(link);
                BitmapImage bi = new BitmapImage();
                bi.SetSource(s);
                Content = new Image { Source = bi };
            }
            catch (Exception e)
            {
                Content = link;//image link, but links page
                _loader.NotifyInvalidLink(link);
            }
        }
    }
}
