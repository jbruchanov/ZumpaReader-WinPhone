using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Input;
using ZumpaReader.Commands;
using System.Diagnostics;

namespace ZumpaReader
{
    public partial class ZumpaSubItemView : UserControl
    {
        public object ViewModel
        {
            get { return (object)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }        

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(object), typeof(ZumpaSubItemView), new PropertyMetadata(null));

        public ICommand ReplyCommand
        {
            get { return (ICommand)GetValue(ReplyCommandProperty); }
            set { SetValue(ReplyCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ReplyCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReplyCommandProperty =
            DependencyProperty.Register("ReplyCommand", typeof(ICommand), typeof(ZumpaSubItemView), new PropertyMetadata(null, ReplyCommandPropertyChange));

        private static void ReplyCommandPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var myself = d as ZumpaSubItemView;
            myself.ReplyCommand = e.NewValue as ICommand;
            myself.MenuItemReply.Command = myself.ReplyCommand;//in this case, binding doesn't work :(
        }

        public bool IgnoreImages
        {
            get { return (bool)GetValue(IgnoreImagesProperty); }
            set { SetValue(IgnoreImagesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IgnoreImages.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IgnoreImagesProperty =
            DependencyProperty.Register("IgnoreImages", typeof(bool), typeof(ZumpaSubItemView), new PropertyMetadata(false));


        public ICommand OpenLinkCommand
        {
            get { return (ICommand)GetValue(OpenLinkCommandProperty); }
            set { SetValue(OpenLinkCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OpenLinkCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OpenLinkCommandProperty =
            DependencyProperty.Register("OpenLinkCommand", typeof(ICommand), typeof(ZumpaSubItemView), new PropertyMetadata(null));

        public ZumpaSubItemView()
        {
            //ReplyCommand = new TestCommand();
            InitializeComponent();            
            //MenuItemReply.Command = ReplyCommand;
        }

        private class TestCommand : ICommand
        {

            public bool CanExecute(object parameter)
            {
                Debug.WriteLine(parameter);
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                Debug.WriteLine(parameter);
            }
        }
    }
}
