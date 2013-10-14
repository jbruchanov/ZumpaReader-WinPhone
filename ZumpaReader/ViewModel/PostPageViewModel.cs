using Coding4Fun.Toolkit.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ZumpaReader.Commands;
using ZumpaReader.Utils;
using ZumpaReader.WebService;

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
            get{ return _isProgressVisible;}
            set{ _isProgressVisible = value; NotifyPropertyChange();}
        }

        public SendMessageCommand SendMessageCommand {get;set;}
                
        public PostPageViewModel()
        {
            SendMessageCommand = new SendMessageCommand(HttpService.CreateInstance());
            SendMessageCommand.CanExecuteChanged += (o,e) => {IsProgressVisible = SendMessageCommand.CanExecute(null); };
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
        }

        public virtual void OnTakePhoto()
        {

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
                new ToastPrompt {Title =":(", Message = Resources.Labels.MissingSubjectOrMessage}.Show();
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
}
