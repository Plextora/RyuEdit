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
        private static Replay? _osuReplay;

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

            if (openFileDialog.ShowDialog() != true)
            {
                SetStatusLabel.Default();
                return;
            }

            _osuReplay = ReplayDecoder.Decode(openFileDialog.FileName);
            SetStatusLabel.Completed("Finished decoding replay! Now loading replay info...");
            await Task.Delay(2000);
            SetStatusLabel.Pending("Loading replay info...");
            OnReplayLoaded();
        }

        private static async void OnReplayLoaded()
        {
            ElementManager.LoadReplayInfo(_osuReplay);

            SetStatusLabel.Completed("Loaded replay info!");
            await Task.Delay(2000);
            SetStatusLabel.Default();
        }

        private async void SaveReplayButton_Click(object sender, RoutedEventArgs e)
        {
            if (_osuReplay == null)
            {
                SetStatusLabel.Error("You must open a replay before saving it!");
                return;
            }

            if (!CheckFields.CheckCombo()) return;
            if (!CheckFields.CheckJudgements()) return;
            if (!CheckFields.CheckTimestamp()) return;
            if (!CheckFields.CheckScore()) return;

            SetStatusLabel.Pending("Saving replay file...");
            SaveFileDialog saveFileDialog = new()
            {
                Filter = "osu! Replay files (*.osr)|*.osr|All files (*.*)|*.*",
                Title = "Save replay file",
                InitialDirectory =
                    $@"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\osu!\Replays"
            };

            if (saveFileDialog.ShowDialog() != true)
            {
                SetStatusLabel.Default();
                return;
            }

            _osuReplay.PlayerName = ReplayUsernameTextbox.Text;
            _osuReplay.Combo = Convert.ToUInt16(ComboTextbox.Text);
            _osuReplay.ReplayScore = Convert.ToInt32(ScoreTextBox.Text);
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