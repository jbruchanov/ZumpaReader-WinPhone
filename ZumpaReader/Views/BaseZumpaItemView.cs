using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ZumpaReader.Utils;

namespace ZumpaReader.Views
{
    public class BaseZumpaItemView : UserControl
    {
        private bool _animated;

        /* Last animation time */
        private static long _lastAnimation = 0;

        /* 1 second in ticks unit */
        private static int SECOND = 10000000;

        /* static value for remebering index of animated view */
        private static int i = 0;

        public BaseZumpaItemView()
        {
            Opacity = 0; //hide immediately to avoid blink effect when view popups
            Loaded += (o, e) => { AnimateShowUp(); };
        }        

        private void AnimateShowUp()
        {
            if (DateTime.Now.Ticks - _lastAnimation > SECOND)
            {
                i = 0;
                _lastAnimation = DateTime.Now.Ticks;
            }

            if (_animated) { return; }

            _animated = true;
            AnimationHelper.AlphaAnimation(this, i++);
        }
    }
}
