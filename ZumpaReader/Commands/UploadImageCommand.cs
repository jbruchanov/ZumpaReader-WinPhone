using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ZumpaReader.WebService;

namespace ZumpaReader.Commands
{
    public class UploadImageCommand : BaseCommand
    {
        private const int QUALITY = 80;

        private IWebService _service;
        private Action<string> _callback;

        public UploadImageCommand(IWebService service, Action<string> callback)
        {
            _service = service;
            _callback = callback;
            CanExecuteIt = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter">Expects BitmapSource</param>
        public override async void Execute(object parameter)
        {
            CanExecuteIt = false;
            try
            {
                EnsureInternet();
                BitmapSource source = (parameter as BitmapSource);
                using (MemoryStream ms = await SaveToJpegAsync(source))
                {
                    var res = await _service.UploadImage(ms.ToArray());
                    if (_callback != null)
                    {
                        _callback.Invoke(res.Context);
                    }
                }
            }
            catch (Exception e)
            {
                ShowError(e);
            }
            CanExecuteIt = true;
        }

        public static Task<MemoryStream> SaveToJpegAsync(BitmapSource source)
        {
            WriteableBitmap wb = new WriteableBitmap(source);     
            Task<MemoryStream> task = new Task<MemoryStream>( () =>
            {
                MemoryStream ms = new MemoryStream();
                wb.SaveJpeg(ms, (int)wb.PixelWidth, (int)wb.PixelHeight, 0, QUALITY);
                return ms;
            });
            task.Start();
            return task;
        }
    }
}
