using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZumpaReader.ViewModel
{
    public class BaseViewModel
    {
        public BasePage _basePage {get; private set;}

        public virtual void OnAttachPage(BasePage page)
        {
            if(page == null)
            {
                throw new ArgumentNullException("Page is null!");
            }
            _basePage = page;        
        }
    }
}
