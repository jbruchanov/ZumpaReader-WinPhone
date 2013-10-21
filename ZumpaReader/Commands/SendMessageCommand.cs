using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ZumpaReader.Model;
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
            WebService.WebService.ContextResult<bool> result = null;
            try
            {
                EnsureInternet();
                HasPostInformation info = (HasPostInformation)parameter;
                Survey s = info.Survey;
                //if (!CheckSurvey(s)) { s = null;}
                result = await _webService.SendMessage(info.Subject, info.Message, null, info.ThreadID);
            }
            catch (Exception e)
            {
                ShowError(e);
            }
            CanExecuteIt = true;
            
            if (callback != null && result != null) { 
                Deployment.Current.Dispatcher.BeginInvoke( () => callback.Invoke(result.Context));
            }
        }

        /// <summary>
        /// Check survey, throws exception if it's invalid
        /// </summary>
        /// <param name="s"></param>
        /// <returns>true if is fine and use it, false to ignore it</returns>
        public bool CheckSurvey(Survey s)
        {
            if (!string.IsNullOrEmpty(s.Question))
            {
                if (string.IsNullOrEmpty(s.Answers[0]) || string.IsNullOrEmpty(s.Answers[1]))
                {
                    throw new Exception(Resources.Labels.InvalidSurvey);
                }
            }
            else
            {
                bool anything = !string.IsNullOrEmpty(s.Question);
                foreach(string item in s.Answers){
                    anything |= !string.IsNullOrEmpty(item);
                }
                if (anything)
                {
                    throw new Exception(Resources.Labels.InvalidSurvey);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
    }

    public interface HasPostInformation
    {
        string ThreadID {get;}
        string Subject {get;}
        string Message {get;}
        Survey Survey {get;}
    }
}
