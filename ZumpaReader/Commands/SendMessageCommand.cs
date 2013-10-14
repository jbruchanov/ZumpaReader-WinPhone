using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ZumpaReader.WebService;

namespace ZumpaReader.Commands
{
    public class SendMessageCommand : BaseCommand
    {
        private WebService.IWebService _webService;                       

        public SendMessageCommand(IWebService webservice)
        {
            _webService = webservice;
        }

        public override async void Execute(object parameter)
        {
            Execute(parameter, null);
        }
        
        public async void Execute(object parameter, Action<bool> callback)
        {
            CanExecuteIt = false;
            HasPostInformation info = (HasPostInformation)parameter;
            var result = await _webService.SendMessage(info.Subject, info.Message, info.ThreadID);
            CanExecuteIt = true;
            if (callback != null) { 
                Deployment.Current.Dispatcher.BeginInvoke( () => callback.Invoke(result.Context));
            }
        }
    }

    public interface HasPostInformation
    {
        string ThreadID {get;}
        string Subject {get;}
        string Message {get;}
    }
}
