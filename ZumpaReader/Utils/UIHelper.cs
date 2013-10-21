using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace ZumpaReader.Utils
{
    public class UIHelper
    {
        /// <summary>
        /// Find any children in a template
        /// </summary>
        /// <param name="depObj">Template</param>
        /// <param name="name">Name of object</param>
        /// <returns></returns>
        public static DependencyObject FindVisualChildren(DependencyObject depObj, string name)
        {
            if (depObj != null)
            {
                for (int i = 0, n = VisualTreeHelper.GetChildrenCount(depObj); i < n; i++)
                {
                    var obj = VisualTreeHelper.GetChild(depObj, i);
                    object objName = obj.GetValue(FrameworkElement.NameProperty);
                    if (name.Equals(objName))
                    {
                        return obj;
                    }
                    else
                    {
                        if (VisualTreeHelper.GetChildrenCount(obj) > 0)
                        {
                            return FindVisualChildren(obj, name);
                        }
                    }
                }
            }
            return null;
        }
    }
}
