using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;

namespace ZumpaReader.Utils
{
    public class AnimationHelper
    {
        public const int ALPHA_DELAY = 30;
        public const int ALPHA_DURATION = 500;

        public static void AlphaAnimation(DependencyObject alphaChangingObject, int delayOrder)
        {
            // Create a duration of 2 seconds.
            Duration duration = new Duration(TimeSpan.FromMilliseconds(ALPHA_DURATION));

            DoubleAnimation alphaAnimation = new DoubleAnimation();
            alphaAnimation.Duration = duration;
            Storyboard sb = new Storyboard();
            sb.Duration = duration;
            sb.Children.Add(alphaAnimation);
            Storyboard.SetTarget(alphaAnimation, alphaChangingObject);
            Storyboard.SetTargetProperty(alphaAnimation, new PropertyPath(FrameworkElement.OpacityProperty));
            alphaAnimation.From = 0;
            alphaAnimation.To = 1;
            sb.BeginTime = TimeSpan.FromMilliseconds(delayOrder * ALPHA_DELAY);
            sb.Begin();
        }
    }
}
