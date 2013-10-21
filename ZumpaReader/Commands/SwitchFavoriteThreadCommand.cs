using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZumpaReader.Model;
using ZumpaReader.WebService;

namespace ZumpaReader.Commands
{
    public class SwitchFavoriteThreadCommand : BaseCommand
    {
        private WebService.IWebService _service;

        private Action<bool> _callback;

        public SwitchFavoriteThreadCommand(IWebService service) : this(service, null) { }

        public SwitchFavoriteThreadCommand(IWebService service, Action<bool> callback)
        {
            CanExecuteIt = true;
            _service = service;
            _callback = callback;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter">Expected ZumpaItem or stringed int of id</param>
        public async override void Execute(object parameter)
        {
            CanExecuteIt = false;
            try
            {
                EnsureInternet();
                int id = 0;
                var item = parameter as ZumpaItem;
                if (item != null)
                {
                    id = item.ID;
                }
                else
                {
                    id = Int32.Parse((string)parameter);
                }
                
                ZumpaReader.WebService.WebService.ContextResult<bool> result = await _service.SwitchThreadFavourite(id);
                if (_callback != null)
                {
                    _callback.Invoke(result.Context);
                }
            }
            catch (Exception e)
            {
                ShowError(e);
            }

            CanExecuteIt = true;
        }
    }
}
