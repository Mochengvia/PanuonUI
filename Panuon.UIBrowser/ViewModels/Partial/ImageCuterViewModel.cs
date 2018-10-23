using Caliburn.Micro;
using Microsoft.Win32;
using Panuon.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Panuon.UIBrowser.ViewModels.Partial
{
    public class ImageCuterViewModel : Screen, IShell
    {
        private PUImageCuter _cuter;
        private Image _image;
        public BitmapImage ImageSource
        {
            get { return _imageSource; }
            set { _imageSource = value;
                NotifyOfPropertyChange(() => ImageSource); }
        }
        private BitmapImage _imageSource;

        public BitmapImage CutImageSource
        {
            get { return _cutImageSource; }
            set {
                _cutImageSource = value;
                NotifyOfPropertyChange(() => CutImageSource); }
        }
        private BitmapImage _cutImageSource;

        #region Event
        public void BtnSelectFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != true)
                return;
            var file = ofd.FileName;
            ImageSource = new BitmapImage(new Uri(file));
        }
        #endregion

        public void ImageCuterLoaded(object sender)
        {
            _cuter = sender as PUImageCuter;
        }
        public void ImageLoaded(object sender)
        {
            _image = sender as Image;
        }
        public void BtnCut()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                _cuter.CutImageSource.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                ms.Seek(0, SeekOrigin.Begin);
                image.StreamSource = ms;
                image.EndInit();
                _image.Source = image;
                Clipboard.SetImage(image);
            }
        }

        public void SelectionChanged(object sender)
        {
            var comboBox = sender as PUComboBox;
            switch (comboBox.SelectedValue as string)
            {
                case "Rectangle":
                    _cuter.AreaStyle = PUImageCuter.AreaStyles.Rectangle;
                    return;
                case "Square":
                    _cuter.AreaStyle = PUImageCuter.AreaStyles.Square;
                    return;
            }
        }
    }
}
