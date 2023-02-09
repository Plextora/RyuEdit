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
    private static readonly TextBox? ScoreTextBox = Form?.ScoreTextBox;
    private static readonly TextBox? ReplayTimestampTextBox = Form?.ReplayTimestampTextBox;

    /*
     * "Why do you have so many 'CheckX' functions that you don't use?"
     * I'll probably have a use for these functions later, so they'll be staying in the codebase.
     * (I will definitely regret this later)
     */
    
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
    
    public static bool CheckScore()
    {
        if (ScoreTextBox != null && ScoreTextBox.Text.Any(char.IsLetter))
        {
            SetStatusLabel.Error("The score textbox cannot contain letters");
            return false;
        }

        if (ScoreTextBox != null && ScoreTextBox.Text.Contains(','))
        {
            SetStatusLabel.Error("The score textbox cannot have commas!");
            return false;
        }

        try
        {
            Convert.ToInt32(ScoreTextBox?.Text);
        }
        catch (OverflowException)
        {
            SetStatusLabel.Error("Score number must be higher than -2,147,483,648 but lower than 2,147,483,647!");
            return false;
        }

        return true;
    }

    public static bool CheckJudgements()
    {
        foreach (var i in ElementManager.JudgementTextBoxes)
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