using Coding4Fun.Toolkit.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ZumpaReader.Commands;
using ZumpaReader.Converters;
using ZumpaReader.Model;
using ZumpaReader.WebService;

namespace ZumpaReader.ViewModel
{
    public class SettingsViewModel : BaseViewModel, Credentials
    {
        #region fields        
        public string Login
        {
            get { return AppSettings.Login; }
            set { AppSettings.Login = value; NotifyPropertyChange(); }
        }

        public string Password
        {
            get { return AppSettings.Password; }
            set { AppSettings.Password = value; NotifyPropertyChange(); }
        }

        public string Nickname
        {
            get { return AppSettings.ResponseName; }
            set { AppSettings.ResponseName = value; NotifyPropertyChange(); }
        }

        public bool IsLoggedIn
        {
            get {return AppSettings.IsLoggedIn; }
            set {AppSettings.IsLoggedIn = value; NotifyPropertyChange(); }
        }

        private bool _isProgressVisible;
        public bool IsProgressVisible
        {
            get { return _isProgressVisible; }
            set { _isProgressVisible = value; NotifyPropertyChange(); }
        }

        public LoginCommand LoginCommand { get; private set; }

        #endregion

        public SettingsViewModel()
        {            
            var service = HttpService.CreateInstance();
            LoginCommand = new LoginCommand(service);
            LoginCommand.CommandFinished += (o,e) =>
            {
                string title = e.IsSuccessful ? Resources.Labels.SmileHappy : Resources.Labels.SmileSad;                                
                string msg = String.Format("{0} {1}", 
                                    e.Type == LoginEventArgs.TaskType.Login ? Resources.Labels.Login : Resources.Labels.Logout,
                                    e.IsSuccessful ? Resources.Labels.Successful : Resources.Labels.Unsuccessful);
                ShowToast(title, msg);                
            };
        }        

        private void ShowToast(string title, string message)
        {
            ToastPrompt tp = new ToastPrompt{ Message = message, Title = title};
            tp.Show();
        }

        public string Convert(bool value)
        {
            return value ? Resources.Labels.Login : Resources.Labels.Logout;
        }
    }
}
