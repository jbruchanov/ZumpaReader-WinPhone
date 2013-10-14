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

        public event EventHandler<LoginEventArgs> CommandFinished;

        private const string LOGIN_SUCC_TOKEN = "portal_lln=";

        public LoginCommand(IWebService service)
        {
            _service = service;
        }

        public override async void Execute(object parameter)
        {
            Credentials creds = (Credentials)parameter;
            CanExecuteIt = false;
            LoginEventArgs args = new LoginEventArgs { Type = creds.IsLoggedIn ? LoginEventArgs.TaskType.Logout : LoginEventArgs.TaskType.Login};
            if (creds.IsLoggedIn)
            {
                var result = await _service.Logout();
                args.IsSuccessful = result.Context;
                if (result.Context)
                {                    
                    ClearLogin(creds);                    
                }
            }
            else
            {
                var result = await _service.Login(creds.Login, creds.Password);
                string cookie = result.Context;
                if (cookie.Contains(LOGIN_SUCC_TOKEN))
                {
                    args.IsSuccessful = true;                    
                    AppSettings.CookieString = cookie;
                    creds.IsLoggedIn = true;
                }
                else
                {
                    args.IsSuccessful = false;
                    ClearLogin(creds);
                }

            }
            CanExecuteIt = true;
            OnCommandFinished(args);
        }

        private void ClearLogin(Credentials creds)
        {
            creds.IsLoggedIn = false;
            creds.Password = string.Empty;
            AppSettings.CookieString = string.Empty;
        }

        public virtual void OnCommandFinished(LoginEventArgs args)
        {
            if (CommandFinished != null)
            {
                CommandFinished.Invoke(this, args);
            }
        }
    }

    public class LoginEventArgs : EventArgs
    {
        public enum TaskType
        {
            Login, Logout
        }

        public TaskType Type { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
