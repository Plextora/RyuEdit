using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using OsuParsers.Decoders;
using OsuParsers.Replays;
using RyuEdit.Util;

namespace RyuEdit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private Replay? _osuReplay;

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Toolbar

        private void CloseButton_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
        private void MinimizeButton_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        private void WindowToolbar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();

        #endregion

        private async void OpenReplayButton_Click(object sender, RoutedEventArgs e)
        {
            SetStatusLabel.Pending("Decoding replay file...");

            OpenFileDialog openFileDialog = new()
            {
                Filter = "osu! Replay files (*.osr)|*.osr|All files (*.*)|*.*",
                InitialDirectory =
                    $@"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\osu!\Replays"
            };

            if (openFileDialog.ShowDialog() != true) return;
            _osuReplay = ReplayDecoder.Decode(openFileDialog.FileName);
            SetStatusLabel.Completed("Finished decoding replay!");
            await Task.Delay(2000);
            SetStatusLabel.Pending("Loading replay info...");
            LoadReplayInfo();
        }

        private async void LoadReplayInfo()
        {
            ReplayUsernameTextbox.Text = _osuReplay?.PlayerName;
            ComboTextbox.Text = (_osuReplay?.Combo).ToString();
            SetStatusLabel.Completed("Loaded replay info!");
            await Task.Delay(2000);
            SetStatusLabel.Default();
        }

        private async void SaveReplayButton_Click(object sender, RoutedEventArgs e)
        {
            if (ComboTextbox.Text.Contains(','))
            {
                SetStatusLabel.Error("The combo textbox cannot have commas!");
                return;
            }

            SetStatusLabel.Pending("Saving editing replay...");
            SaveFileDialog saveFileDialog = new()
            {
                Filter = "osu! Replay files (*.osr)|*.osr|All files (*.*)|*.*",
                Title = "Save replay file",
                InitialDirectory =
                    $@"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\osu!\Replays"
            };

            if (saveFileDialog.ShowDialog() != true) return;
            if (_osuReplay == null) return;

            _osuReplay.PlayerName = ReplayUsernameTextbox.Text;
            try
            {
                _osuReplay.Combo = Convert.ToUInt16(ComboTextbox.Text);
            }
            catch (OverflowException)
            {
                SetStatusLabel.Error("Combo number must be higher than 0 but lower than 65,535!");
                return;
            }
            _osuReplay.Save(saveFileDialog.FileName);
            SetStatusLabel.Completed("Saved edited replay!");
            await Task.Delay(2000);
            SetStatusLabel.Default();
        }
    }
}