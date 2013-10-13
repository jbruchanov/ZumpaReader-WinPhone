using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZumpaReader.WebService;

namespace ZumpaReader.Commands
{
    public class LogoutCommand : BaseCommand
    {
        private IWebService _service;

        public LogoutCommand(IWebService service)
        {
            _service = service;
        }

        public override bool CanExecute(object parameter)
        {
            return AppSettings.IsLoggedIn;
        }

        public override async void Execute(object parameter)
        {
            CanExecuteIt = false;
            var result = await _service.Logout();
            AppSettings.IsLoggedIn = !result.Context;
            CanExecuteIt = true;        
        }
    }
}
