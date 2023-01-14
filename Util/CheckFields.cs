using System;
using System.Windows;
using System.Windows.Controls;

namespace RyuEdit.Util;

public static class CheckFields
{
    private static readonly MainWindow? Form = Application.Current.Windows[0] as MainWindow;
    
    private static readonly TextBox? ComboTextbox = Form?.ComboTextbox;
    private static readonly TextBox? ReplayUsernameTextbox = Form?.ReplayUsernameTextbox;

    public static bool CheckCombo()
    {
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
}