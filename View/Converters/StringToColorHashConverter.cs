using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Data;
using ColorWindows = System.Windows.Media.Color;
using ColorDrawing = System.Drawing.Color;

namespace SnailPass.View.Converters
{
    [ValueConversion(typeof(string), typeof(SolidColorBrush))]
    public class StringToColorHashConverter : IValueConverter
    {
        private const int L = 190;
        private const int S = 240;
        int H = 0;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = value as string;

            long sum = 0;
            text.AsParallel().ForAll(c => sum += (byte)c);
            H = (int)(sum % 240);

            int win32color = ColorHLSToRGB(H, L, S);
            ColorDrawing colorD = ColorTranslator.FromWin32(win32color);
            ColorWindows colorW = ColorWindows.FromArgb(colorD.A, colorD.R, colorD.G, colorD.B);

            return new SolidColorBrush(colorW);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }

        [DllImport("shlwapi.dll")]
        public static extern int ColorHLSToRGB(int H, int L, int S);
    }
}
