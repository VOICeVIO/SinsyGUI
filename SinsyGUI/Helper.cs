using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace VOICeVIO.SinsyGUI
{
    static class Helper
    {
        public static string ToParamString(this Language lang)
        {
            switch (lang)
            {
                case Language.Japanese:
                    return "j";
                case Language.Chinese:
                    return "c";
                case Language.English:
                    return "e";
            }

            return "j";
        }

        public static string ToParamString(this LabelType type)
        {
            switch (type)
            {
                case LabelType.None:
                    return "d";
                case LabelType.Normal:
                    return "n";
                case LabelType.WithTime:
                    return "t";
                case LabelType.Mono:
                    return "m";
            }

            return "d";
        }
    }

    public class EnumToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.Equals(true) ? parameter : Binding.DoNothing;
        }
    }
}
