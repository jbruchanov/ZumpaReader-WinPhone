using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ZumpaReader.Commands;
using ZumpaReader.Utils;

namespace ZumpaReader.Controls
{
    public class ImageButton : Button
    {
        private static ImageLoader _loader;

        public bool IgnoreImages
        {
            get { return (bool)GetValue(IgnoreImagesProperty); }
            set { SetValue(IgnoreImagesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IgnoreImages.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IgnoreImagesProperty =
            DependencyProperty.Register("IgnoreImages", typeof(bool), typeof(ImageButton), new PropertyMetadata(false));

        static ImageButton()
        {
            _loader = new ImageLoader();
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            string link = newContent as string;
            if (!IgnoreImages && AppSettings.AutoLoadImages && _loader.IsImageLink(link))
            {
                Content = new ProgressBar { IsIndeterminate = true, MinHeight = 32, MinWidth = 300 };
                LoadImageAsync(link);
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

                Content = link;//image link, but link is invalid (i.g. not real image)
                _loader.NotifyInvalidLink(link);
            }
        }
    }
}
