using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ZumpaReader.Commands
{
    public class ImageOperationCommand : BaseCommand
    {
        private readonly string _fileId;

        private Action<WriteableBitmap> _callback;

        private int _rotation;

        private float _lastScale = 1;

        public ImageOperationCommand(string fileId, Action<WriteableBitmap> callback)
        {
            _fileId = fileId;
            _callback = callback;
            CanExecuteIt = true;
        }

        /// <summary>
        /// Load Image from library
        /// </summary>
        /// <returns></returns>
        private WriteableBitmap LoadImage()
        {
            MediaLibrary library = new MediaLibrary();
            Picture photoFromLibrary = library.GetPictureFromToken(_fileId);
            BitmapImage image = new BitmapImage();
            image.SetSource(photoFromLibrary.GetImage());
            return new WriteableBitmap(image);
        }

        public override bool CanExecute(object parameter)
        {
            return CanExecuteIt && !("90".Equals(parameter) && _lastScale == 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter">Expectes string numeric string 90 to rotate or </param>
        public override void Execute(object parameter)
        {
            CanExecuteIt = false;
            string param = (parameter as string);
            WriteableBitmap result = null;
            if (param != null)
            {
                result = Convert(param);
            }
            if (_callback != null)
            {
                _callback.Invoke(result);
            }
            CanExecuteIt = true;
        }

        public WriteableBitmap Convert(string parameter)
        {
            WriteableBitmap image = LoadImage();
            if ("90".Equals(parameter))
            {
                _rotation += 90;
                if (_lastScale != 1)
                {
                    image = Scale(image, _lastScale);
                }
                image = WriteableBitmapExtensions.Rotate(image, _rotation);
            }
            else
            {
                int v = 1;
                if (Int32.TryParse(parameter, out v))
                {
                    _lastScale = 1f / v;
                    image = Scale(image, _lastScale);
                }                
            }
            //preventively try release everything
            GC.Collect();
            return image;
        }

        private WriteableBitmap Scale(WriteableBitmap srouce, float scale)
        {
            WriteableBitmap wb = new WriteableBitmap(srouce);
            int w = (int)(wb.PixelWidth * _lastScale);
            int h = (int)(wb.PixelHeight * _lastScale);
            return wb.Resize(w, h, WriteableBitmapExtensions.Interpolation.Bilinear);
        }
    }
}
