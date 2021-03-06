﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZumpaReader.Model;
using ZumpaReader.WebService;

namespace ZumpaReader.Commands
{
    public class LoginCommand : BaseCommand
    {
        private IWebService _service;

        public event EventHandler<LoginEventArgs> CommandFinished;

        public LoginCommand(IWebService service)
        {
            _service = service;
        }

        public override async void Execute(object parameter)
        {
            Credentials creds = (Credentials)parameter;
            CanExecuteIt = false;
            try
            {
                EnsureInternet();
                LoginEventArgs args = await Execute(creds);
                OnCommandFinished(args);
            }
            catch (Exception e)
            {
                ShowError(e);
            }
            CanExecuteIt = true;
            
        }

        private async Task<LoginEventArgs> Execute(Credentials creds)
        {
            LoginEventArgs args = new LoginEventArgs { Type = creds.IsLoggedIn ? LoginEventArgs.TaskType.Logout : LoginEventArgs.TaskType.Login };
            if (creds.IsLoggedIn)
            {
                var result = await _service.Logout();
                args.LogoutResult = result.Context;
                if (result.Context)
                {
                    AppSettings.ZumpaUID = null;
                    ClearLogin(creds);
                }
            }
            else
            {
                var result = await _service.Login(creds.Login, creds.Password);
                LoginResult context = result.Context;

                creds.IsLoggedIn = context.Result;
                args.LoginResult = context;
                AppSettings.CookieString = context.Cookies;
                AppSettings.ZumpaUID = context.UID;
                if (context.Result)
                {
                    if (!String.IsNullOrEmpty(AppSettings.PushURI))
                    {
                        await _service.RegisterPushURI(creds.Login, context.UID, AppSettings.PushURI);
                    }
                }
                else
                {
                    ClearLogin(creds);
                }
            }
            return args;
        }

        /// <summary>
        /// Clears login in AppSettings (removes cookie, password, set filter to default value)
        /// </summary>
        /// <param name="creds"></param>
        private void ClearLogin(Credentials creds)
        {
            creds.IsLoggedIn = false;
            creds.Password = string.Empty;
            AppSettings.CookieString = string.Empty;
            AppSettings.Filter = 0;
        }

        public virtual void OnCommandFinished(LoginEventArgs args)
        {
            if (CommandFinished != null)
            {
                CommandFinished.Invoke(this, args);
            }
        }
    }

    /// <summary>
    /// Help container for result
    /// </summary>
    public class LoginEventArgs : EventArgs
    {
        public enum TaskType
        {
            Login, Logout
        }

        public TaskType Type { get; set; }
        public LoginResult LoginResult { get; set; }
        public bool LogoutResult { get; set; }
    }
}
