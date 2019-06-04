using Caliburn.Micro;
using Microsoft.Win32;
using Panuon.UI;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Panuon.UIBrowser.ViewModels.Special
{
    public class ImageCuterViewModel : Screen, IShell
    {
        #region Identity
        private PUImageCuter _cuter;

        private Image _image;

        private IWindowManager _windowManager;
        #endregion

        #region Constructor
        public ImageCuterViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
        }
        #endregion

        #region Binding
        public BitmapImage ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                NotifyOfPropertyChange(() => ImageSource);
            }
        }
        private BitmapImage _imageSource;

        public BitmapImage CutImageSource
        {
            get { return _cutImageSource; }
            set
            {
                _cutImageSource = value;
                NotifyOfPropertyChange(() => CutImageSource);
            }
        }
        private BitmapImage _cutImageSource;
        #endregion

        #region Event
        public void BtnSelectFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "图像文件|*.jpg;*.png;*.jpeg;*.bmp";
            if (ofd.ShowDialog() != true)
                return;
            var file = ofd.FileName;
            ImageSource = new BitmapImage(new Uri(file));
        }

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
            if (_cuter.GetCutedImage() == null)
            {
                PUMessageBox.ShowDialog("没有图片源。");
                return;
            }
            _image.Source = _cuter.GetCutedImage();
        }

        public void SelectionChanged(object sender)
        {
            var comboBox = sender as PUComboBox;
            switch (comboBox.SelectedValue as string)
            {
                case "Rectangle":
                    _cuter.ImageType = ImageType.Rectangle;
                    return;
                case "Square":
                    _cuter.ImageType = ImageType.Square;
                    return;
            }
        }
        #endregion

    }
}
