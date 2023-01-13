﻿using System;
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
    public partial class MainWindow : Window
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
            SetStatusLabel.Pending("Decoding replay file...", StatusLabel);

            OpenFileDialog openFileDialog = new()
            {
                Filter = "osu! Replay files (*.osr)|*.osr|All files (*.*)|*.*",
                InitialDirectory =
                    $@"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\osu!\Replays"
            };

            if (openFileDialog.ShowDialog() != true) return;
            _osuReplay = ReplayDecoder.Decode(openFileDialog.FileName);
            SetStatusLabel.Completed("Finished decoding replay!", StatusLabel);
            await Task.Delay(2000);
            SetStatusLabel.Pending("Loading replay info...", StatusLabel);
            LoadReplayInfo();
        }

        private async void LoadReplayInfo()
        {
            ReplayUsernameTextbox.Text = _osuReplay?.PlayerName;
            SetStatusLabel.Completed("Loaded replay info!", StatusLabel);
            await Task.Delay(2000);
            SetStatusLabel.Default(StatusLabel);
        }

        private async void SaveReplayButton_Click(object sender, RoutedEventArgs e)
        {
            SetStatusLabel.Pending("Saving editing replay...", StatusLabel);
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
            _osuReplay.Save(saveFileDialog.FileName);
            SetStatusLabel.Completed("Saved edited replay!", StatusLabel);
            await Task.Delay(2000);
            SetStatusLabel.Default(StatusLabel);
        }
    }
}