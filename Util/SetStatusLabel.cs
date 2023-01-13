using System.Windows.Controls;
using System.Windows.Media;

namespace RyuEdit.Util;

public abstract class SetStatusLabel
{
    public static void Default(string statusText, Label statusLabel)
    {
        statusLabel.Foreground = Brushes.PeachPuff;
        statusLabel.Content = $"Status: {statusText}";
    }
    
    public static void Pending(string statusText, Label statusLabel)
    {
        statusLabel.Foreground = Brushes.Khaki;
        statusLabel.Content = $"Status: {statusText}";
    }

    public static void Completed(string statusText, Label statusLabel)
    {
        statusLabel.Foreground = Brushes.LightGreen;
        statusLabel.Content = $"Status: {statusText}";
    }
    
    public static void Error(string statusText, Label statusLabel)
    {
        statusLabel.Foreground = Brushes.Crimson;
        statusLabel.Content = $"Status: {statusText}";
    }
}