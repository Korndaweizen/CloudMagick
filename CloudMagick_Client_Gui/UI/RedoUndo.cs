using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CloudMagick_Client_UI.UI
{
    public class RedoUndo
    {
        private static List<Image> _imagesList;
        private static int _pointer;
        private static PictureBox _box;
        private const int Maxsize = 10;

        public RedoUndo(PictureBox box)
        {
            _box = box;
            _imagesList = new List<Image>();
            _pointer = -1;
        }
        public RedoUndo()
        {
            _imagesList = new List<Image>();
            _pointer = -1;
        }

        public RedoUndo(PictureBox box, Image initImage)
        {
            _box = box;
            _imagesList = new List<Image> { initImage };
            _pointer = 0;
        }

        public static Image GetCurrentImage()
        {
            if (_pointer<0)
            {
                return null;
            }
            return _imagesList.ElementAt(_pointer);
        }
        public static void AddImage(Image image)
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
            if (_box == null)
            {
                return;
            }
            _box.Image = image;
        }

        public void Redo()
        {
            if (_pointer<_imagesList.Count-1)
            {
                _pointer++;
                if (_box==null)
                {
                    return;
                }
                _box.Image = _imagesList.ElementAt(_pointer) ?? null;
            }
        }

        public void Undo()
        {
            if(_pointer > 0)
            {
                _pointer--;
                if (_box == null)
                {
                    return;
                }
                _box.Image = _imagesList.ElementAt(_pointer) ?? null;
            }
        }

        public static bool IsPointerAtNewest()
        {
            return (_pointer == _imagesList.Count - 1);
        }


    }
}
