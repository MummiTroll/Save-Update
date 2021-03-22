using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Save.Controls
{
    public class MyTextBox : TextBox
    {
        public MyTextBox()
        {
            Width = 275;
            Height = 26;
            FontSize = 14;
            FontFamily = new FontFamily("SegoeUI");
            Margin = new Thickness(0, 0, 0, 5);
            VerticalContentAlignment = VerticalAlignment.Center;
            HorizontalContentAlignment = HorizontalAlignment.Left;
            HorizontalAlignment = HorizontalAlignment.Center;
        }
    }
}
