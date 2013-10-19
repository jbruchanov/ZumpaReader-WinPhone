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
        WebService.IWebService _service;

        public SwitchFavoriteThreadCommand(IWebService service)
        {
            CanExecuteIt = true;
            _service = service;
        }

        public async override void Execute(object parameter)
        {
            CanExecuteIt = false;
            try
            {
                var item = parameter as ZumpaItem;
                await _service.SwitchThreadFavourite(item.ID);
            }
            catch (Exception e)
            {
                ShowError(e);
            }

            CanExecuteIt = true;
        }
    }
}
