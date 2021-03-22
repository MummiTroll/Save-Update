using System.Windows.Controls;
using System.Windows;

namespace Save.Controls
{
    public class MyLabel : Label
    {
        public MyLabel()
        {
            Width = 80;
            Height = 26;
            FontSize = 12;
            Margin = new Thickness(0, 0, 0, 5);
            VerticalContentAlignment = VerticalAlignment.Center;
            HorizontalContentAlignment = HorizontalAlignment.Center;
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
        }
    }
}
