using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace FinanceManager.Convertors
{
    class TransactionTypeConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string val = (string)value;
                if (val == "Income")
                {
                    return "arrow-up.png";
                }else
                {
                    return "arrow-down.png";

                }


            }
            return "arrow-down.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
