using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ZumpaReader.Model;
using ZumpaReader.WebService;

namespace ZumpaReader.Commands
{
    public class LoginCommand : BaseCommand
    {
        private IWebService _service;

        public LoginCommand(IWebService service)
        {
            _service = service;
        }

        public override bool CanExecute(object parameter)
        {
            return !AppSettings.IsLoggedIn && base.CanExecute(parameter);
        }

        public override async void Execute(object parameter)
        {
            Credentials creds = (Credentials)parameter;
            CanExecuteIt = false;
            var result = await _service.Login(creds.Login, creds.Password);
            CanExecuteIt = true;        
        }
    }
}
