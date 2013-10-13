using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ZumpaReader.Commands;
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

        private bool _isProgressVisible;
        public bool IsProgressVisible
        {
            get { return _isProgressVisible; }
            set { _isProgressVisible = value; NotifyPropertyChange(); }
        }

        public ICommand LoginCommand { get; private set; }

        public ICommand LogoutCommand { get; private set; }

        #endregion

        public SettingsViewModel()
        {
            var c = new WebService.WebService.WebServiceConfig();
            c.BaseURL = ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.WebServiceURL];
            c.LastAnswerAuthor = true;
            var service = new HttpService(c);
            //LoginCommand = new LoginCommand(service);
            //LoginCommand.CanExecuteChanged += Command_CanExecuteChanged;
            //LogoutCommand = new LogoutCommand(service);
            //LoginCommand.CanExecuteChanged += Command_CanExecuteChanged;
            //NotifyPropertyChange("LoginCommand");
            //NotifyPropertyChange("LogoutCommand");
        }

        private void Command_CanExecuteChanged(object sender, EventArgs e)
        {
            IsProgressVisible = !(LoginCommand.CanExecute(null) && LogoutCommand.CanExecute(null));
        }
    }
}
