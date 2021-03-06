﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Navigation;

namespace ZumpaReader.Utils
{    
    /// <summary>
    /// Basic UriMapper for support multiple project targeting and problems with assembly names in urls
    /// </summary>
    public class ZumpaUriMapper : UriMapperBase
    {         
        public override Uri MapUri(Uri uri)
        {
            string tempUri = uri.ToString();
            string mappedUri;

            // Launch from the photo share picker.
            // Incoming URI example: /MainPage.xaml?Action=ShareContent&FileId=%7BA3D54E2D-7977-4E2B-B92D-3EB126E5D168%7D
            if ((tempUri.Contains("ShareContent")) && (tempUri.Contains("FileId")))
            {
                // Redirect to PhotoShare.xaml.
                mappedUri = tempUri.Replace("/MainPage.xaml", "/ZumpaReader;component/Pages/PostPage.xaml");
                return new Uri(mappedUri, UriKind.Relative);
            }
            else if ("/MainPage.xaml".Equals(tempUri))
            {
                return new Uri("/ZumpaReader;component/MainPage.xaml", UriKind.RelativeOrAbsolute);
            }
            else if (tempUri != null && tempUri.StartsWith("/Pages/ThreadPage.xaml"))
            {
                return new Uri(tempUri.Replace("/Pages/ThreadPage.xaml", "/ZumpaReader;component/Pages/ThreadPage.xaml"), UriKind.RelativeOrAbsolute);
            }

            // Otherwise perform normal launch.
            return uri;
        }
    }
}
