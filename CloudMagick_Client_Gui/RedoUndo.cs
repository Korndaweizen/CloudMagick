using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GraphicsMagick;

namespace CloudMagick_Client_Gui
{
    public class RedoUndo
    {
        private static List<MagickImage> _imagesList;
        private static int _pointer;
        private static PictureBox _box;
        private const int Maxsize = 10;

        public RedoUndo(PictureBox box)
        {
            _box = box;
            _imagesList = new List<MagickImage>();
            _pointer = -1;
        }

        public RedoUndo(PictureBox box, MagickImage initImage)
        {
            _box = box;
            _imagesList = new List<MagickImage> { initImage };
            _pointer = 0;
        }

        public static MagickImage GetCurrentImage()
        {
            if (_pointer<0)
            {
                return null;
            }
            return _imagesList.ElementAt(_pointer);
        }
        public static void AddImage(MagickImage image)
        {
            //image.Write("images/test.png");
            //image=new MagickImage("images/test.png");
            if (_pointer < _imagesList.Count-1)
            {
                _imagesList.RemoveRange(_pointer+1,_imagesList.Count-1-_pointer);
            }
            _imagesList.Add(image);
            _pointer++;

            if (_imagesList.Count> Maxsize)
            {
                _imagesList.RemoveAt(0);
                _pointer--;
            }
            if (image == null)
                _box.Image = null;
            else
            _box.Image = image.ToBitmap();
        }

        public void Redo()
        {
            if (_pointer<_imagesList.Count-1)
            {
                _pointer++;
                _box.Image = _imagesList.ElementAt(_pointer)?.ToBitmap();
            }
        }

        public void Undo()
        {
            if(_pointer > 0)
            {
                _pointer--;
                _box.Image = _imagesList.ElementAt(_pointer)?.ToBitmap();
            }
        }

        public static bool IsPointerAtNewest()
        {
            return (_pointer == _imagesList.Count - 1);
        }


    }
}
