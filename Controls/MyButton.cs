using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Save.Controls
{
    public class MyButton : Button
    {
        Style style = new Style
        {
            TargetType = typeof(Border),
            Setters = { new Setter { Property = Border.CornerRadiusProperty, Value = new CornerRadius(12) } }
        };
        public MyButton()
        {
            Width = 16;
            Height = 16;
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
            Margin = new Thickness(0, 5, 0, 10);
            Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Save;component/Resources/Button-violet_round_x2_64.jpg", UriKind.RelativeOrAbsolute)));
            Resources.Add(style.TargetType, style);
        }
    }
}
