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

namespace ZumpaReader.ViewModel
{
    public class PostPageViewModel : BaseViewModel, HasPostInformation
    {
        public const string SUBJECT = "title";
        public const string THREAD_ID = "threadId";

        private const int PHOTO_INDEX = 0;
        private const int SEND_INDEX = 1;

        private string _title;

        public string Subject
        {
            get { return _title; }
            set { _title = value; NotifyPropertyChange(); }
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

        internal BitmapSource OriginalPhoto { get; set; }

        private BitmapSource _photo;
        public BitmapSource Photo
        {
            get { return _photo; }
            set { _photo = value; NotifyPropertyChange(); }
        }

        private string _originalResolution;
        public string OriginalPhotoResolution
        {
            get { return _originalResolution; }
            set { _originalResolution = value; NotifyPropertyChange(); }
        }

        private int _originalSize;

        public int OriginalSize
        {
            get { return _originalSize; }
            set { _originalSize = value; NotifyPropertyChange(); }
        }

        private string _newResolution;
        public string NewPhotoResolution
        {
            get { return _newResolution; }
            set { _newResolution = value; NotifyPropertyChange(); }
        }

        private int _newSize;
        public int NewSize
        {
            get { return _newSize; }
            set { _newSize = value; NotifyPropertyChange(); }
        }

        public ImageOperationCommand _imageOperationCommand;
        public ImageOperationCommand ImageOperationCommand
        {
            get { return _imageOperationCommand; }
            set { _imageOperationCommand = value; NotifyPropertyChange(); }
        }

        public UploadCommand _uploadCommand;
        public UploadCommand UploadCommand
        {
            get { return _uploadCommand; }
            set { _uploadCommand = value; NotifyPropertyChange(); }
        }
        

        public SendMessageCommand SendMessageCommand { get; set; }

        public PostPageViewModel()
        {
            SendMessageCommand = new SendMessageCommand(HttpService.CreateInstance());
            SendMessageCommand.CanExecuteChanged += (o, e) => { IsProgressVisible = SendMessageCommand.CanExecute(null); };
        }

        public override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string title = null;
            if (Page.NavigationContext.QueryString.TryGetValue(SUBJECT, out title))
            {
                Subject = title;
                string threadId = null;
                if (Page.NavigationContext.QueryString.TryGetValue(THREAD_ID, out threadId))
                {
                    ThreadID = threadId;
                }
            }

            (Page.ApplicationBar.Buttons[PHOTO_INDEX] as ApplicationBarIconButton).Click += (o, ea) => { OnTakePhoto(); };
            (Page.ApplicationBar.Buttons[SEND_INDEX] as ApplicationBarIconButton).Click += (o, ea) => { OnSend(); };
            TryInitPhoto();
        }

        private void TryInitPhoto()
        {
            // Get a dictionary of query string keys and values.
            IDictionary<string, string> queryStrings = Page.NavigationContext.QueryString;

            // Ensure that there is at least one key in the query string, and check whether the "FileId" key is present.
            if (queryStrings.ContainsKey("FileId"))
            {
                // Retrieve the photo from the media library using the FileID passed to the app.
                MediaLibrary library = new MediaLibrary();
                Picture photoFromLibrary = library.GetPictureFromToken(queryStrings["FileId"]);

                // Create a BitmapImage object and add set it as the image control source.
                // To retrieve a full-resolution image, use the GetImage() method instead.
                OriginalPhotoResolution = string.Format("{0}x{1}", photoFromLibrary.Width, photoFromLibrary.Height);
                Photo = new BitmapImage();
                Stream s = photoFromLibrary.GetImage();
                OriginalSize = (int)s.Length;
                Photo.SetSource(s);
                (Page.ApplicationBar.Buttons[PHOTO_INDEX] as ApplicationBarIconButton).IsEnabled = false;
                OriginalPhoto = Photo;
                ImageOperationCommand = new ImageOperationCommand(this);
                UploadCommand = new UploadCommand(this);
            }
            else
            {
                (Page as PostPage).PhotoPanoramaItem.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        public virtual void OnTakePhoto()
        {
            CameraCaptureTask task = new CameraCaptureTask();
            task.Completed += (o, e) => { OnTakenPhoto(e); };
        }

        public void OnTakenPhoto(PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {

            }
        }

        public virtual void OnSend()
        {
            object focusObj = FocusManager.GetFocusedElement();
            if (focusObj != null && focusObj is PhoneTextBox)
            {
                var binding = (focusObj as PhoneTextBox).GetBindingExpression(PhoneTextBox.TextProperty);
                binding.UpdateSource();
            }

            if (string.IsNullOrEmpty(Subject) || string.IsNullOrEmpty(Message))
            {
                new ToastPrompt { Title = ":(", Message = Resources.Labels.MissingSubjectOrMessage }.Show();
                return;
            }

            SendMessageCommand.Execute(this, (e) => OnSendResponse(e));
        }

        public void OnSendResponse(bool success)
        {
            new ToastPrompt { Title = success ? ":)" : ":(" }.Show();
            if (success && Page.NavigationService.CanGoBack)
            {
                Page.NavigationService.GoBack();
            }
        }
    }

    public class ImageOperationCommand : ICommand
    {
        private bool _get;
        private bool Can
        {
            get { return _get; }
            set { _get = value; _viewModel.IsProgressVisible = !value; Notify(); }
        }
        public bool CanExecute(object parameter)
        {
            return Can && !("90".Equals(parameter) && _viewModel.NewSize == 0);//don't let rotate not resized images
        }

        public event EventHandler CanExecuteChanged;

        private PostPageViewModel _viewModel;
        public ImageOperationCommand(PostPageViewModel model)
        {
            _viewModel = model;
            Can = true;
        }

        public void Execute(object parameter)
        {
            Can = false;
            string op = (string)parameter;
            BitmapImage newImage = null;
            int size = 0;
            if ("1".Equals(op))
            {
                _viewModel.Photo = _viewModel.OriginalPhoto;
                _viewModel.NewSize = 0;
                _viewModel.NewPhotoResolution = "";
            }
            else if ("90".Equals(op))
            {
                newImage = Convert(GenerateConstrainedBitmap((_viewModel.Photo as BitmapImage), 1, true), out size);                
            }
            else
            {
                int v = Int32.Parse(op);
                float ratio = 1f/v;
                newImage = Convert(GenerateConstrainedBitmap((_viewModel.OriginalPhoto as BitmapImage), ratio, false), out size);                
            }
            if (newImage != null) 
            { 
                _viewModel.Photo = newImage;
                _viewModel.NewSize = size;
                _viewModel.NewPhotoResolution = string.Format("{0}x{1}", newImage.PixelWidth, newImage.PixelHeight); ;
            }

            Can = true;
        }

        private void Notify()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged.Invoke(this, EventArgs.Empty);
            }
        }

        private WriteableBitmap GenerateConstrainedBitmap(BitmapImage sourceImage, float scale, bool rotation)
        {
            int scaledWidth = (int)(sourceImage.PixelWidth * scale);
            int scaledHeight = (int)(sourceImage.PixelHeight * scale);            
            // Create a transform to render the image rotated and scaled
            var transform = new TransformGroup();
            if (rotation)
            {                 
                var rt = new RotateTransform()
                {
                    Angle = 90,
                    CenterX = (sourceImage.PixelWidth / 2.0),
                    CenterY = (sourceImage.PixelHeight / 2.0)
                };
                transform.Children.Add(rt);
            }
            if (scale != 1) 
            {
                var st = new ScaleTransform()
                {
                    ScaleX = scale,
                    ScaleY = scale,
                    //CenterX = (sourceImage.PixelWidth / 2.0),
                    //CenterY = (sourceImage.PixelHeight / 2.0)
                };
                transform.Children.Add(st);
            }

            // Resize to specified target size
            var tempImage = new Image()
            {
                Stretch = Stretch.Fill,
                Width = sourceImage.PixelWidth,
                Height = sourceImage.PixelHeight,
                Source = sourceImage,
            };
            tempImage.UpdateLayout();

            // Render to a writeable bitmap
            var writeableBitmap = new WriteableBitmap(scaledWidth, scaledHeight);
            writeableBitmap.Render(tempImage, transform);
            writeableBitmap.Invalidate();            
            return writeableBitmap;
        }

        private BitmapImage Convert(WriteableBitmap writeBmp, out int size)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                writeBmp.SaveJpeg(ms, (int)writeBmp.PixelWidth, (int)writeBmp.PixelHeight, 0, 80);
                size = (int)ms.Length;
                BitmapImage bmp = new BitmapImage();
                bmp.SetSource(ms);
                return bmp;
            }
        }
    }

    public class UploadCommand : ICommand
    {
        private bool _get;
        private bool Can
        {
            get { return _get; }
            set { _get = value; _viewModel.IsProgressVisible = !value; Notify(); }
        }
        public bool CanExecute(object parameter)
        {
            return Can;
        }

        public event EventHandler CanExecuteChanged;

        private PostPageViewModel _viewModel;
        public UploadCommand(PostPageViewModel model)
        {
            _viewModel = model;
            Can = true;
        }

        public async void Execute(object parameter)
        {
            Can = false;

            HttpService service = HttpService.CreateInstance();
            using (MemoryStream ms = new MemoryStream())
            {   
                WriteableBitmap wb = new WriteableBitmap(_viewModel.Photo);
                wb.SaveJpeg(ms, (int)wb.PixelWidth, (int)wb.PixelHeight, 0, 100);                                
                var res = await service.UploadImage(ms.ToArray());
                _viewModel.Message += String.Format("\n<{0}>", res.Context);
                Debug.WriteLine(res.Context);
            }            
            Can = true;
        }
                     

        private void Notify()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged.Invoke(this, EventArgs.Empty);
            }
        }       
    }
}
