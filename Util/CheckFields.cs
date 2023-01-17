using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RyuEdit.Util;

public static class CheckFields
{
    private static readonly MainWindow? Form = Application.Current.Windows[0] as MainWindow;

    private static readonly TextBox? ComboTextbox = Form?.ComboTextbox;
    private static readonly TextBox? ReplayTimestampTextBox = Form?.ReplayTimestampTextBox;
    private static readonly IDictionary<TextBox?, string> JudgementTextBoxes = new Dictionary<TextBox, string>
    {
        { Form?._300CountTextBox!, "number of 300s" },
        { Form?._100CountTextBox!, "number of 100s" },
        { Form?._50CountTextBox!, "number of 50s" },
        { Form?.MissCountTextBox!, "number of misses" },
        { Form?.GekiCountTextBox!, "number of Geki's" },
        { Form?.KatuCountTextBox!, "number of Katu's" }
        // definitely not best practice but whatever, bad practice is my jam!
    }!;

    public static bool CheckCombo()
    {
        if (ComboTextbox != null && ComboTextbox.Text.Any(char.IsLetter))
        {
            SetStatusLabel.Error("The combo textbox cannot contain letters");
            return false;
        }

        if (ComboTextbox != null && ComboTextbox.Text.Contains(','))
        {
            SetStatusLabel.Error("The combo textbox cannot have commas!");
            return false;
        }

        try
        {
            Convert.ToUInt16(ComboTextbox?.Text);
        }
        catch (OverflowException)
        {
            SetStatusLabel.Error("Combo number must be higher than 0 but lower than 65,535!");
            return false;
        }

        return true;
    }

    public static bool CheckJudgements()
    {
        foreach (var i in JudgementTextBoxes)
        {
            if (i.Key != null && i.Key.Text.Any(char.IsLetter))
            {
                SetStatusLabel.Error($"The {i.Value} textbox cannot contain letters!");
                return false;
            }

            if (i.Key != null && i.Key.Text.Contains(','))
            {
                SetStatusLabel.Error($"The {i.Value} textbox cannot contain commas!");
                return false;
            }

            try
            {
                Convert.ToUInt16(i.Key?.Text);
            }
            catch (OverflowException)
            {
                SetStatusLabel.Error($"The {i.Value} must be higher than 0 but lower than 65,535!");
                return false;
            }
        }

        return true;
    }

    public static bool CheckTimestamp()
    {
        try
        {
            Convert.ToDateTime(ReplayTimestampTextBox?.Text);
        }
        catch
        {
            SetStatusLabel.Error("Invalid replay timestamp format!");
            return false;
        }

        return true;
    }
}