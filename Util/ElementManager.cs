using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using OsuParsers.Replays;

namespace RyuEdit.Util;

public static class ElementManager
{
    private static readonly MainWindow? Form = Application.Current.Windows[0] as MainWindow;
    private static readonly TextBox?[] AllTextBoxes =
    {
        Form?.ReplayUsernameTextbox,
        Form?.ComboTextbox,
        Form?.ScoreTextBox,
        Form?.ReplayTimestampTextBox,
        Form?._300CountTextBox,
        Form?._100CountTextBox,
        Form?._50CountTextBox,
        Form?.MissCountTextBox,
        Form?.GekiCountTextBox,
        Form?.KatuCountTextBox
    };

    private static readonly Dictionary<TextBox?, string?> TextBoxToValue = new();
    
    public static readonly IDictionary<TextBox?, string> JudgementTextBoxes = new Dictionary<TextBox, string>
    {
        { Form?._300CountTextBox!, "number of 300s" },
        { Form?._100CountTextBox!, "number of 100s" },
        { Form?._50CountTextBox!, "number of 50s" },
        { Form?.MissCountTextBox!, "number of misses" },
        { Form?.GekiCountTextBox!, "number of Geki's" },
        { Form?.KatuCountTextBox!, "number of Katu's" }
        // definitely not best practice but whatever, bad practice is my jam!
    }!;

    public static void LoadReplayInfo(Replay? replay)
    {
        if (TextBoxToValue.Count != 0)
            TextBoxToValue.Clear();

        TextBoxToValue.Add(AllTextBoxes[0], replay?.PlayerName);
        TextBoxToValue.Add(AllTextBoxes[1], (replay?.Combo).ToString());
        TextBoxToValue.Add(AllTextBoxes[2], replay?.ReplayScore.ToString());
        TextBoxToValue.Add(AllTextBoxes[3], replay?.ReplayTimestamp.ToLocalTime().ToString(CultureInfo.CurrentCulture));
        TextBoxToValue.Add(AllTextBoxes[4], replay?.Count300.ToString());
        TextBoxToValue.Add(AllTextBoxes[5], replay?.Count100.ToString());
        TextBoxToValue.Add(AllTextBoxes[6], replay?.Count50.ToString());
        TextBoxToValue.Add(AllTextBoxes[7], replay?.CountMiss.ToString());
        TextBoxToValue.Add(AllTextBoxes[8], replay?.CountGeki.ToString());
        TextBoxToValue.Add(AllTextBoxes[9], replay?.CountKatu.ToString());

        foreach (var i in TextBoxToValue.Where(i => i.Key != null))
        {
            if (i.Key == null) continue;
            i.Key.Text = i.Value;
            i.Key.IsEnabled = true;
        }

        if (Form == null) return;
        Form.IsPerfectComboCheckbox.Opacity = 1;
        Form.DoubleTimeCheckBox.Opacity = 1;
        Form.SaveReplayButton.Opacity = 1;
        Form.IsPerfectComboCheckbox.IsEnabled = true;
        Form.DoubleTimeCheckBox.IsEnabled = true;
        Form.SaveReplayButton.IsEnabled = true;
        Form.OpenReplayButton.IsEnabled = true;
    }
}