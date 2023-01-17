using System;
using System.Globalization;
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
            ReplayTimestampTextBox.Text = _osuReplay?.ReplayTimestamp.ToLocalTime().ToString(CultureInfo.CurrentCulture);
            IsPerfectComboCheckbox.IsChecked = _osuReplay is { PerfectCombo: true };
            _300CountTextBox.Text = _osuReplay?.Count300.ToString();
            _100CountTextBox.Text = _osuReplay?.Count100.ToString();
            _50CountTextBox.Text = _osuReplay?.Count50.ToString();
            MissCountTextBox.Text = _osuReplay?.CountMiss.ToString();
            GekiCountTextBox.Text = _osuReplay?.CountGeki.ToString();
            KatuCountTextBox.Text = _osuReplay?.CountKatu.ToString();

            SetStatusLabel.Completed("Loaded replay info!");
            await Task.Delay(2000);
            SetStatusLabel.Default();
        }

        private async void SaveReplayButton_Click(object sender, RoutedEventArgs e)
        {
            if (_osuReplay == null) return;
            if (!CheckFields.CheckCombo()) return;
            if (!CheckFields.CheckJudgements()) return;
            if (!CheckFields.CheckTimestamp()) return;

            SetStatusLabel.Pending("Saving editing replay...");
            SaveFileDialog saveFileDialog = new()
            {
                Filter = "osu! Replay files (*.osr)|*.osr|All files (*.*)|*.*",
                Title = "Save replay file",
                InitialDirectory =
                    $@"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\osu!\Replays"
            };

            if (saveFileDialog.ShowDialog() != true) return;

            _osuReplay.PlayerName = ReplayUsernameTextbox.Text;
            _osuReplay.Combo = Convert.ToUInt16(ComboTextbox.Text);
            if (IsPerfectComboCheckbox.IsChecked != null)
                _osuReplay.PerfectCombo = (bool)IsPerfectComboCheckbox.IsChecked;
            _osuReplay.Count300 = Convert.ToUInt16(_300CountTextBox.Text);
            _osuReplay.Count100 = Convert.ToUInt16(_100CountTextBox.Text);
            _osuReplay.Count50 = Convert.ToUInt16(_50CountTextBox.Text);
            _osuReplay.CountMiss = Convert.ToUInt16(MissCountTextBox.Text);
            _osuReplay.CountGeki = Convert.ToUInt16(GekiCountTextBox.Text);
            _osuReplay.CountKatu = Convert.ToUInt16(KatuCountTextBox.Text);
            _osuReplay.ReplayTimestamp = Convert.ToDateTime(ReplayTimestampTextBox.Text);

            _osuReplay.Save(saveFileDialog.FileName);
            SetStatusLabel.Completed("Saved edited replay!");
            await Task.Delay(2000);
            SetStatusLabel.Default();
        }
    }
}