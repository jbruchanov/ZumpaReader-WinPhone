using Coding4Fun.Toolkit.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ZumpaReader.Commands;
using ZumpaReader.Utils;
using ZumpaReader.WebService;
using Microsoft.Xna.Framework.Media;
using System.Windows.Media.Imaging;
using ZumpaReader.Pages;
using System.IO;
using System.Windows.Media;
using System.Windows.Controls;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using ZumpaReader.Model;

namespace ZumpaReader.ViewModel
{
    public class PostPageViewModel : BaseViewModel, HasPostInformation
    {
        public const string SUBJECT = "title";
        public const string REPLY_TO = "replyto";
        public const string THREAD_ID = "threadId";
        private const string FileId = "FileId";

        private const int PHOTO_INDEX = 0;
        private const int LIBRARY_INDEX = 1;
        private const int SEND_INDEX = 2;

        #region BindingProperties
        private string _subject;
        public string Subject
        {
            get { return _subject; }
            set { _subject = value; NotifyPropertyChange(); }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; NotifyPropertyChange(); }
        }

        private string _threadId;
        public string ThreadID
        {
            get { return _threadId; }
            set { _threadId = value; NotifyPropertyChange(); }
        }

        private bool _isProgressVisible;
        public bool IsProgressVisible
        {
            get { return _isProgressVisible; }
            set { _isProgressVisible = value; NotifyPropertyChange(); }
        }

        private BitmapSource _photo;
        public BitmapSource Photo
        {
            get { return _photo; }
            set { _photo = value; NotifyPropertyChange(); }
        }

        private string _newResolution;
        public string PhotoResolution
        {
            get { return _newResolution; }
            set { _newResolution = value; NotifyPropertyChange(); }
        }

        private string _newSize;
        public string PhotoSize
        {
            get { return _newSize; }
            set { _newSize = value; NotifyPropertyChange(); }
        }

        private ImageOperationCommand _imageOperationCommand;
        public ImageOperationCommand ImageOperationCommand
        {
            get { return _imageOperationCommand; }
            set { _imageOperationCommand = value; NotifyPropertyChange(); }
        }

        private UploadImageCommand _uploadCommand;
        public UploadImageCommand UploadCommand
        {
            get { return _uploadCommand; }
            set { _uploadCommand = value; NotifyPropertyChange(); }
        }

        private SendMessageCommand _sendMessageCommand;
        public SendMessageCommand SendMessageCommand
        {
            get { return _sendMessageCommand; }
            set { _sendMessageCommand = value; NotifyPropertyChange(); }
        }

        private Survey _survey;
        public Survey Survey
        {
            get { return _survey; }
            set { _survey = value; NotifyPropertyChange(); }
        }

        #endregion

        private IWebService _webService;

        public PostPageViewModel()
        {
            _survey = new Survey() { Answers = new string[6] };
            _webService = HttpService.CreateInstance();
            SendMessageCommand = new SendMessageCommand(_webService);
            SendMessageCommand.CanExecuteChanged += (o, e) =>
            {
                IsProgressVisible = !SendMessageCommand.CanExecute(null);
                (Page.ApplicationBar.Buttons[SEND_INDEX] as ApplicationBarIconButton).IsEnabled = !IsProgressVisible;
            };
        }

        public override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (!AppSettings.IsLoggedIn)
            {
                ThreadPool.QueueUserWorkItem((stateInfo) =>
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show(Resources.Labels.SmileSad, Resources.Labels.NotLoggedIn, MessageBoxButton.OK);
                        (Page as PostPage).Panorama.IsEnabled = false;
                        Page.ApplicationBar.IsVisible = false;
                    });
                });
                return;
            }

            string title = null;
            if (Page.NavigationContext.QueryString.TryGetValue(SUBJECT, out title))
            {
                Subject = title;
                string threadId = null;
                if (Page.NavigationContext.QueryString.TryGetValue(THREAD_ID, out threadId))
                {
                    ThreadID = threadId;
                    (Page as PostPage).SurveyPanoramaItem.Visibility = Visibility.Collapsed;
                }
            }

            string replyto = null;
            if (Page.NavigationContext.QueryString.TryGetValue(REPLY_TO, out replyto))
            {
                Message += String.Format("@{0}: \n", replyto);
            }

            (Page.ApplicationBar.Buttons[PHOTO_INDEX] as ApplicationBarIconButton).Click += (o, ea) => { OnPhotoTaking(); };
            (Page.ApplicationBar.Buttons[LIBRARY_INDEX] as ApplicationBarIconButton).Click += (o, ea) => { OnImageChoosing(); };
            (Page.ApplicationBar.Buttons[SEND_INDEX] as ApplicationBarIconButton).Click += (o, ea) => { OnSending(); };
            TryInitPhoto();
        }

        private void TryInitPhoto()
        {
            // Get a dictionary of query string keys and values.
            IDictionary<string, string> queryStrings = Page.NavigationContext.QueryString;

            // Ensure that there is at least one key in the query string, and check whether the "FileId" key is present.
            if (queryStrings.ContainsKey(FileId))
            {
                string fileId = queryStrings[FileId];
                MediaLibrary library = new MediaLibrary();
                Picture photoFromLibrary = library.GetPictureFromToken(fileId);
                InitImaging(null, fileId);
            }
        }

        private void InitImaging(Stream stream, string fileId)
        {
            ImageOperationCommand = stream != null
                                        ? new ImageOperationCommand(stream, (e) => { OnPhotoChanged(e); })
                                        : new ImageOperationCommand(fileId, (e) => { OnPhotoChanged(e); });

            ImageOperationCommand.CanExecuteChanged += (o, e) => { IsProgressVisible = !ImageOperationCommand.CanExecute(null); };
            ImageOperationCommand.Execute("1");//Load default image

            UploadCommand = new UploadImageCommand(_webService, (link) => { Message += String.Format("\n<{0}>", link); });
            UploadCommand.CanExecuteChanged += (o, e) => { IsProgressVisible = !UploadCommand.CanExecute(null); };
        }

        public void OnImageChoosing()
        {
            PhotoChooserTask task = new PhotoChooserTask();
            task.Completed += (o, e) => { OnImageChosen(e); };
            task.Show();
        }

        private void OnImageChosen(PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                InitImaging(e.ChosenPhoto, null);
            }
        }

        public async void OnPhotoChanged(BitmapSource source)
        {
            Photo = source;
            PhotoResolution = string.Format("{0}x{1}", source.PixelWidth, source.PixelHeight);
            using (MemoryStream ms = await UploadImageCommand.SaveToJpegAsync(source))
            {
                PhotoSize = String.Format("{0:#,###0} B", ms.Length);
            }
        }

        public void OnPhotoTaking()
        {
            CameraCaptureTask task = new CameraCaptureTask();
            task.Completed += (o, e) => { OnPhotoTaken(e); };
            task.Show();
        }

        public void OnPhotoTaken(PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                InitImaging(e.ChosenPhoto, null);
            }
        }

        public void OnSending()
        {
            object focusObj = FocusManager.GetFocusedElement();
            if (focusObj != null && focusObj is PhoneTextBox)
            {
                var binding = (focusObj as PhoneTextBox).GetBindingExpression(PhoneTextBox.TextProperty);
                binding.UpdateSource();
            }

            if (string.IsNullOrEmpty(Subject) || string.IsNullOrEmpty(Message))
            {
                new ToastPrompt { Title = Resources.Labels.SmileSad, Message = Resources.Labels.MissingSubjectOrMessage }.Show();
                return;
            }

            SendMessageCommand.Execute(this, (e) => OnSentResponse(e));
        }

        public void OnSentResponse(bool success)
        {
            new ToastPrompt { Title = success ? Resources.Labels.SmileHappy : Resources.Labels.SmileSad }.Show();
            if (success && Page.NavigationService.CanGoBack)
            {
                Page.NavigationService.GoBack();
            }
        }
    }


}
