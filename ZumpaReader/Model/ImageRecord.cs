using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Microsoft.Phone.Data.Linq;
using Microsoft.Phone.Data.Linq.Mapping;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ZumpaReader.Model
{
    [Table(Name = "Images")]
    public class ImageRecord : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        [Column(CanBeNull = false, IsPrimaryKey = true)]
        public string Link { get; set; }

        [Column(CanBeNull = false)]
        public String File { get; set; }

        [Column(CanBeNull = false)]
        public long Size { get; set; }

        private bool _isValid;
        [Column(CanBeNull = false)]
        public bool IsValid //only this one is meant like changeable
        { 
            get { return _isValid; }
            set { NotifyPropertyChanging(); _isValid = value; NotifyPropertyChanged(); }
        }

        public void NotifyPropertyChanging([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanging != null)
            {
                PropertyChanging.Invoke(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
