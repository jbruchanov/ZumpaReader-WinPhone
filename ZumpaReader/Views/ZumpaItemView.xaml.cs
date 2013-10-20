using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Animation;
using System.Diagnostics;

namespace ZumpaReader.Views
{
    public partial class ZumpaItemView : UserControl
    {

        private bool _animated;
        static int i = 0;
        private static long _lastAnimation = 0;
        private static int SECOND = 10000000;

        public ZumpaItemView()
        {
            InitializeComponent();
            Opacity = 0;
            Loaded += ZumpaItemView_Loaded;
        }

        void ZumpaItemView_Loaded(object sender, RoutedEventArgs e)
        {
            AnimateShowUp();
        }

        public void AnimateShowUp()
        {
            if (DateTime.Now.Ticks - _lastAnimation > SECOND)
            {
                i = 0;
                _lastAnimation = DateTime.Now.Ticks;
            }

            if (_animated) { return; }

            _animated = true;
            // Create a duration of 2 seconds.
            Duration duration = new Duration(TimeSpan.FromMilliseconds(500));

            DoubleAnimation alphaAnimation = new DoubleAnimation();
            alphaAnimation.Duration = duration;
            Storyboard sb = new Storyboard();
            sb.Duration = duration;
            sb.Children.Add(alphaAnimation);
            Storyboard.SetTarget(alphaAnimation, this);
            Storyboard.SetTargetProperty(alphaAnimation, new PropertyPath(FrameworkElement.OpacityProperty));
            alphaAnimation.From = 0;
            alphaAnimation.To = 1;
            sb.BeginTime = TimeSpan.FromMilliseconds(i++ * 30);
            sb.Begin();
        }
    }
}
