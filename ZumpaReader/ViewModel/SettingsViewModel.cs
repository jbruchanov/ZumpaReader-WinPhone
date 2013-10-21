using Coding4Fun.Toolkit.Controls;
using RemoteLogCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZumpaReader.Commands;
using ZumpaReader.Converters;
using ZumpaReader.Model;
using ZumpaReader.Utils;
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
            get { return AppSettings.IsLoggedIn; }
            set { AppSettings.IsLoggedIn = value; NotifyPropertyChange(); }
        }

        public bool LastAuthor
        {
            get { return AppSettings.LastAuthor; }
            set { AppSettings.LastAuthor = value; NotifyPropertyChange(); }
        }

        public int Filter
        {
            get { return AppSettings.Filter; }
            set { AppSettings.Filter = value; NotifyPropertyChange(); }
        }

        public bool AutoLoadImages
        {
            get { return AppSettings.AutoLoadImages; }
            set { AppSettings.AutoLoadImages = value; NotifyPropertyChange(); }
        }

        private string _storageValues;
        public string StorageValues
        {
            get { return _storageValues; }
            set { _storageValues = value; NotifyPropertyChange(); }
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
            LoginCommand.CanExecuteChanged += (o,e) => {IsProgressVisible = !LoginCommand.CanExecuteIt;};
            LoginCommand.CommandFinished += (o, e) =>
            {
                string title, msg;
                if (e.Type == LoginEventArgs.TaskType.Login)
                {
                    title = String.Format("{0} {1} {2}",
                    e.LoginResult.Result ? Resources.Labels.SmileHappy : Resources.Labels.SmileSad,
                    Resources.Labels.Login,
                    e.LoginResult.Result ? Resources.Labels.Successful : Resources.Labels.Unsuccessful
                    );
                    msg = e.LoginResult.ZumpaResult;
                }
                else
                {
                    title = e.LogoutResult ? Resources.Labels.SmileHappy : Resources.Labels.SmileSad;
                    msg = String.Format("{0} {1}", Resources.Labels.Logout,
                                    e.LogoutResult ? Resources.Labels.Successful : Resources.Labels.Unsuccessful);
                }

                ShowToast(title, msg);
            };

            if (IsEmulator())
            {
                Login = ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.Login];
                Password = ZumpaReaderResources.Instance[ZumpaReaderResources.Keys.Password];
            }
            StorageValues = Resources.Labels.InProgress;
            LoadStorageValues();
        }

        private async void LoadStorageValues()
        {
            var vals = await LoadStorageValuesAsync();
            StorageValues = ConvertStorageValues(vals.Downloaded, vals.FreeSpace);
        }

        private Task<StorageValues> LoadStorageValuesAsync()
        {
            var task = new Task<StorageValues>(() =>
            {
                return ImageLoader.GetStorageValues();
            });
            task.Start();
            return task;
        }

        private void ShowToast(string title, string message)
        {
            ToastPrompt tp = new ToastPrompt { Message = message, Title = title, TextOrientation = System.Windows.Controls.Orientation.Vertical, TextWrapping = TextWrapping.Wrap };
            tp.Show();
        }

        public string Convert(bool value)
        {
            return value ? Resources.Labels.Login : Resources.Labels.Logout;
        }

        public static bool IsEmulator()
        {
            return "XDeviceEmulator".Equals(new DeviceDataProvider().GetDevice().Model);
        }

        private static string ConvertStorageValues(long images, long freeSpace)
        {
            return String.Format("{0}:\t{1}\n{2}:\t{3}", Resources.Labels.Downloaded, StringUtils.ConvertToReadableSize(images), Resources.Labels.Free, StringUtils.ConvertToReadableSize(freeSpace));
        }
    }
}
