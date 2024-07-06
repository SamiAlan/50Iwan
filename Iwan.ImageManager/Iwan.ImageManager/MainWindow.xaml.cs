using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Iwan.ImageManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _currentImagePath = string.Empty;
        private string _previousSaveFolder = string.Empty;
        private string _previousOpenFolder = string.Empty;
        private bool _resolutionInvalid = true;

        private bool IsStartButtonEnabled => !string.IsNullOrEmpty(_currentImagePath) 
            && File.Exists(_currentImagePath) && !_resolutionInvalid;

        public MainWindow()
        {
            InitializeComponent();
            gridImage.MouseDown += HandleImageClicked;
            btnStart.Click += StartClicked;
            btnClear.Click += ClearClicked;
            btnByFolder.Click += ByFolderClicked;
            btnByRoot.Click += ByRootClicked;
            txtResolutionDecrease.TextChanged += ValidateResolution;
            txtResolutionDecrease.Text = "2";
        }

        private void HandleImageClicked(object sender, MouseButtonEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "Images | *.png;*.jpeg;*.jpg;*.JPG",
                Title = "Choose image file",
                Multiselect = false,
                CheckFileExists = true
            };

            if (!string.IsNullOrEmpty(_previousOpenFolder))
                ofd.InitialDirectory = _previousOpenFolder;

            var result = ofd.ShowDialog();

            if (!result.HasValue) result = false;

            if (!result.Value) return;

            _previousOpenFolder = ofd.FileName.Replace(Path.GetFileName(ofd.FileName), "");

            SetCurrentImage(ofd.FileName);

            btnStart.IsEnabled = IsStartButtonEnabled;

            SetInfoStatus("Image loaded");
        }

        private void StartClicked(object sender, EventArgs e)
        {
            // Get a place to save the image
            var ofd = new SaveFileDialog
            {
                Title = "Save image",
                FileName = Path.GetFileName(_currentImagePath),
            };

            if (!string.IsNullOrEmpty(_previousSaveFolder))
                ofd.InitialDirectory = _previousSaveFolder;

            var result = ofd.ShowDialog();

            if (!result.HasValue) result = false;

            if (!result.Value) return;

            _previousSaveFolder = ofd.FileName.Replace(Path.GetFileName(ofd.FileName), "");
            var resolutionDecrease = Convert.ToInt32(txtResolutionDecrease.Text);

            try
            {
                byte[] bytes;

                if (Path.GetExtension(_currentImagePath) == ".png")
                    bytes = ImageManipulator.CompressPngImage(_currentImagePath, resolutionDecrease);
                else
                    bytes = ImageManipulator.CompressJpegImage(_currentImagePath, 50, resolutionDecrease);

                File.WriteAllBytes(ofd.FileName, bytes);
            }
            catch (Exception ex)
            {
                SetErrorStatus(ex.Message);
                return;
            }
            
            // Compress and resize the image then save it
            SetSuccessStatus("Image done");
        }

        private void SetCurrentImage(string imagePath)
        {
            _currentImagePath = imagePath;
            image.Source = new BitmapImage(new Uri(imagePath));
        }

        private async void ByFolderClicked(object sender, RoutedEventArgs e)
        {
            var sourceFolderDialog = new FolderPicker()
            {
                Title = "Select source folder"
            };

            if (!string.IsNullOrEmpty(_previousOpenFolder))
                sourceFolderDialog.InputPath = _previousOpenFolder;

            var sourceResult = sourceFolderDialog.ShowDialog();

            if (!sourceResult.HasValue) sourceResult = false;

            if (!sourceResult.Value) return;

            _previousOpenFolder = sourceFolderDialog.ResultPath;

            var destinationFolderDialog = new FolderPicker()
            {
                Title = "Select destination folder"
            };

            if (!string.IsNullOrEmpty(_previousSaveFolder))
                destinationFolderDialog.InputPath = _previousSaveFolder;

            var destinationResult = destinationFolderDialog.ShowDialog();

            if (!destinationResult.HasValue) destinationResult = false;

            if (!destinationResult.Value) return;

            _previousSaveFolder = destinationFolderDialog.ResultPath;

            var supportedExtensions = new string[] { ".png", ".jpg", ".jpeg", ".JPG", ".JPEG" };

            if (!int.TryParse(txtResolutionDecrease.Text, out var resolutionDecrease))
            {
                resolutionDecrease = 1;
                txtResolutionDecrease.Text = resolutionDecrease.ToString();
            }

            SetInfoStatus("Working on images");

            await Task.Delay(300);
            foreach (var file in new DirectoryInfo(_previousOpenFolder)
                .GetFiles().Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden) && supportedExtensions.Contains(f.Extension)))
            {
                try
                {
                    byte[] bytes;

                    if (Path.GetExtension(file.FullName) == ".png")
                        bytes = ImageManipulator.CompressPngImage(file.FullName, resolutionDecrease);
                    else
                        bytes = ImageManipulator.CompressJpegImage(file.FullName, 50, resolutionDecrease);

                    File.WriteAllBytes(Path.Combine(_previousSaveFolder, Path.GetFileName(file.FullName)), bytes);
                }
                catch (Exception ex)
                {
                    SetErrorStatus(ex.Message);
                    return;
                }

                // Compress and resize the image then save it
                SetSuccessStatus("Images done");
            }
        }

        private async void ByRootClicked(object sender, RoutedEventArgs e)
        {
            var sourceFolderDialog = new FolderPicker()
            {
                Title = "Select root folder"
            };

            if (!string.IsNullOrEmpty(_previousOpenFolder))
                sourceFolderDialog.InputPath = _previousOpenFolder;

            var sourceResult = sourceFolderDialog.ShowDialog();

            if (!sourceResult.HasValue) sourceResult = false;

            if (!sourceResult.Value) return;

            _previousOpenFolder = sourceFolderDialog.ResultPath;

            var supportedExtensions = new string[] { ".png", ".jpg", ".jpeg", ".JPG", ".JPEG" };

            if (!int.TryParse(txtResolutionDecrease.Text, out var resolutionDecrease))
            {
                resolutionDecrease = 1;
                txtResolutionDecrease.Text = resolutionDecrease.ToString();
            }

            await Task.Delay(300);

            foreach (var directoryPath in Directory.GetDirectories(_previousOpenFolder))
            {
                var directoryName = Path.GetFileName(directoryPath);

                SetInfoStatus($"Working on item {directoryName}");

                await Task.Delay(300);

                var directoryWithFiles = Path.Combine(_previousOpenFolder, directoryName, directoryName);

                foreach (var file in new DirectoryInfo(directoryWithFiles)
                    .GetFiles().Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden) && supportedExtensions.Contains(f.Extension)))
                {
                    try
                    {
                        byte[] bytes;

                        if (Path.GetExtension(file.FullName) == ".png")
                            bytes = ImageManipulator.CompressPngImage(file.FullName, resolutionDecrease);
                        else
                            bytes = ImageManipulator.CompressJpegImage(file.FullName, 80, resolutionDecrease);

                        var filePath = Path.Combine(directoryWithFiles, file.Name);

                        File.WriteAllBytes(filePath, bytes);
                    }
                    catch (Exception ex)
                    {
                        SetErrorStatus(ex.Message);
                        return;
                    }
                }

                var files = new DirectoryInfo(directoryWithFiles).GetFiles().Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden))
                    .Select(f => "\"" + f.FullName + "\"");
                var rarFilePath = "\"" + Path.Combine(directoryWithFiles, $"{directoryName}.rar") + "\"";
                var filesArgs = string.Join(' ', files);
                // Process.Start("\"C:\\Program Files\\WinRAR\\rar.exe\"", $"a -ep {rarFilePath} {filesArgs}").WaitForExit();
                var p = new Process()
                {
                    StartInfo = new ProcessStartInfo("\"C:\\Program Files\\WinRAR\\rar.exe\"", $"a -ep {rarFilePath} {filesArgs}")
                    {
                        CreateNoWindow = true,

                    }
                };

                p.Start();
                p.WaitForExit();

                SetInfoStatus($"Item {directoryName} done");
                await Task.Delay(300);
            }


            SetSuccessStatus($"Items done");
            await Task.Delay(300);
        }

        private void ClearClicked(object sender, RoutedEventArgs e)
        {
            _currentImagePath = string.Empty;
            image.Source = null;
            _resolutionInvalid = true;
            txtResolutionDecrease.Text = "2";
            txtResolutionDecreaseError.Text = string.Empty;
            txtResolutionDecreaseError.Visibility = Visibility.Collapsed;
            btnStart.IsEnabled = IsStartButtonEnabled;
            ClearStatus();
        }

        #region Validations

        private void ValidateResolution(object sender, TextChangedEventArgs args)
        {
            if (!int.TryParse(txtResolutionDecrease.Text, out var resolutionDecrease))
            {
                _resolutionInvalid = true;
                txtResolutionDecreaseError.Text = "Invalid resolution decrease value";
                txtResolutionDecreaseError.Visibility = Visibility.Visible;
            }
            else if (resolutionDecrease <= 0)
            {
                _resolutionInvalid = true;
                txtResolutionDecreaseError.Text = "Invalid resolution decrease value";
                txtResolutionDecreaseError.Visibility = Visibility.Visible;
            }
            else
            {
                txtResolutionDecreaseError.Visibility = Visibility.Collapsed;
                txtResolutionDecreaseError.Text = string.Empty;
                _resolutionInvalid = false;
            }

            btnStart.IsEnabled = IsStartButtonEnabled;
        }

        #endregion

        #region Status

        private void ClearStatus()
        {
            SetStatus(string.Empty, Brushes.Black);
        }

        private void SetInfoStatus(string message)
        {
            SetStatus(message, Brushes.Blue);
        }

        private void SetSuccessStatus(string message)
        {
            SetStatus(message, Brushes.Green);
        }

        private void SetErrorStatus(string message)
        {
            SetStatus(message, Brushes.Red);
        }

        private void SetWarningStatus(string message)
        {
            SetStatus(message, Brushes.Orange);
        }

        private void SetStatus(string message, SolidColorBrush brush)
        {
            lblStatus.Content = message;
            lblStatus.Foreground = brush;
        }
        
        #endregion
    }
}
