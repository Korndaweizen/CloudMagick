using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CloudMagick_Client_Gui
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

        public RedoUndo(PictureBox box, Image initImage)
        {
            _box = box;
            _imagesList = new List<Image> { initImage };
            _pointer = 0;
        }

        public static void AddImage(Image image)
        {
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
            _box.Image = image;
        }

        public void Redo()
        {
            if (_pointer<_imagesList.Count-1)
            {
                _pointer++;
                _box.Image = _imagesList.ElementAt(_pointer);
            }
        }

        public void Undo()
        {
            if(_pointer > 0)
            {
                _pointer--;
                _box.Image = _imagesList.ElementAt(_pointer);
            }
        }

        public bool isPointerAtNewest()
        {
            return (_pointer == _imagesList.Count - 1);
        }


    }
}
