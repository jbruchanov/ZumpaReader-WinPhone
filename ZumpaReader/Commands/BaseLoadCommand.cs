using Coding4Fun.Toolkit.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using ZumpaReader.WebService;

namespace ZumpaReader.Commands
{
    public abstract class BaseLoadCommand : BaseCommand
    {
        public string LoadURL { get; set; }

        public IWebService WebService { get; private set; }

        public BaseLoadCommand(IWebService service)
        {
            WebService = service;
        }

        
    }
}
