using Coding4Fun.Toolkit.Controls;
using RemoteLogCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ZumpaReader.Commands;
using ZumpaReader.Utils;

namespace ZumpaReader.Controls
{
    /// <summary>
    /// Simpla UI Control for lazy loading of images, bind Link to URL
    /// </summary>
    public class ImageButton : Button
    {
        private static ImageLoader _loader;

        private bool _longClick = false; //help var for ignoring click event when long click was handled

        #region properties
        
        public string Link
        {
            get { return (string)GetValue(LinkProperty); }
            set { SetValue(LinkProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Link.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LinkProperty =
            DependencyProperty.Register("Link", typeof(string), typeof(ImageButton), new PropertyMetadata(null, OnLinkPropertyChanged));

        private static void OnLinkPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ImageButton ib = d as ImageButton;
            if (ib != null) { ib.OnLinkChanged(e.NewValue as string); }
        }

        /// <summary>
        /// Simple switch for temporaryli disable image loading
        /// </summary>
        public bool IgnoreImages
        {
            get { return (bool)GetValue(IgnoreImagesProperty); }
            set { SetValue(IgnoreImagesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IgnoreImages.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IgnoreImagesProperty =
            DependencyProperty.Register("IgnoreImages", typeof(bool), typeof(ImageButton), new PropertyMetadata(false));

        #endregion
        static ImageButton()
        {
            _loader = new ImageLoader();
        }

        protected virtual void OnLinkChanged(string link)
        {
            if (!IgnoreImages && AppSettings.AutoLoadImages && _loader.IsImageLink(link))
            {
                Content = new ProgressBar { IsIndeterminate = true, MinHeight = 23, MinWidth = 300 };
                LoadImageAsync(link);
            }
            else
            {
                Content = new TextBlock{ Text = link, TextWrapping = TextWrapping.Wrap};
            }
        }

        private async void LoadImageAsync(string link)
        {
            try
            {
                Stream s = await _loader.LoadAsync(link);
                if (s != null)
                {
                    BitmapImage bi = new BitmapImage();
                    bi.SetSource(s);
                    Content = new Image { Source = bi };
                }
                else
                {
                    Content = new TextBlock { Text = link, TextWrapping = TextWrapping.Wrap };
                }
            }
            catch (Exception e)//image link, but link is invalid (i.g. not real image)
            {
                RLog.E(this, e, "Unable to load:" + link);
                Content = link;
                _loader.NotifyInvalidLink(link);
            }
        }

        protected override void OnMouseLeftButtonUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!_longClick) 
            { 
                base.OnMouseLeftButtonUp(e);
            }
            e.Handled = _longClick;//ignore this event if it's called from OnHold...
            _longClick = false;
        }

        protected override void OnHold(System.Windows.Input.GestureEventArgs e)
        {
            base.OnHold(e);
            Clipboard.SetText(Link);
            new ToastPrompt{Message = ZumpaReader.Resources.Labels.LinkSaved, TextWrapping = TextWrapping.Wrap}.Show();
            e.Handled = true;
            _longClick = true;
        }
    }
}
