using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Entry = Microcharts.ChartEntry;

namespace FinanceManager.Services
{
   public static class ChartGenerator
    {

        public static List<string> Categorys { get; set; } = new List<string>
        {
            "Shopping",
            "Utilities",
            "Services",
            "Food",
            "Gadgets"
        };

        public static async Task<Chart> GetOverView(DateTime fromPeriod)
        {
            // "SELECT SUM(Price) FROM \"Transaction\" WHERE Type = \"Income\"" get sum from Income
            // "SELECT SUM(Price) FROM \"Transaction\" WHERE Type = \"Expense\"" get sum from Expense

            var incomeSum = await Services.DatabaseConnection.GetFunctionResult($"SELECT SUM(Price) FROM \"Transaction\" WHERE Type = \"Income\" ");
            var expenseSum = await Services.DatabaseConnection.GetFunctionResult($"SELECT SUM(Price) FROM \"Transaction\" WHERE Type = \"Expense\" ");
            Color color1 = (Color)Application.Current.Resources["AccentLitePurple"];
            Color color2 = (Color)Application.Current.Resources["AccentDarkPurple"];

            List<Entry> entrys = new List<Entry>
            {
                new Entry(incomeSum)
                {
                    Color = SKColor.Parse(color1.ToHex()),
                    ValueLabelColor = SKColor.Parse(color1.ToHex()),
                    Label = "Income",
                    ValueLabel = $"{string.Format("{0:N2}", incomeSum)}"
                },
                new Entry(expenseSum)
                {
                    Color = SKColor.Parse(color2.ToHex()),
                    ValueLabelColor = SKColor.Parse(color2.ToHex()),
                    Label = "Expenses",
                    ValueLabel = $"{string.Format("{0:N2}", expenseSum)}"
                }
            };

            return new BarChart { Entries = entrys, LabelTextSize = 40f, BackgroundColor = SKColor.Parse(Color.Transparent.ToHex()) };
        }


        public static async Task<Chart> GetIncomesGraf(DateTime fromPeriod)
        {
            var listOfIncom = await Services.DatabaseConnection.GetIncomeTransactions();
            Entry entry;
            List<Entry> entrys = new List<Entry>();
            Color color;
            Random rand = new Random();

            foreach (var incom in listOfIncom)
            {
                color = Color.FromRgb(rand.Next(150), rand.Next(150), rand.Next(150));
                entry = new Entry(incom.Price)
                {
                    Color = SKColor.Parse(color.ToHex()),
                    ValueLabelColor = SKColor.Parse(color.ToHex()),
                    Label = $"{incom.Name}",
                    ValueLabel = $"{string.Format("{0:N2}", incom.Price)}"
                };

                entrys.Add(entry);
            }

            return new DonutChart { Entries = entrys, LabelTextSize = 40f, BackgroundColor = SKColor.Parse(Color.Transparent.ToHex()) };
        }

        public static async Task<Chart> GetExpencesCategory(DateTime fromPeriod)
        {
            Entry entry;
            List<Entry> entrys = new List<Entry>();
            Color color;
            Random rand = new Random();

            foreach (string cateory in Categorys)
            {
                var categorySum = await Services.DatabaseConnection.GetFunctionResult($"SELECT SUM(Price) FROM \"Transaction\" WHERE Category = \"{cateory}\" ");
                if (categorySum > 0)
                {
                    color = Color.FromRgb(rand.Next(150), rand.Next(150), rand.Next(150));
                    entry = new Entry(categorySum)
                    {
                        Color = SKColor.Parse(color.ToHex()),
                        ValueLabelColor = SKColor.Parse(color.ToHex()),
                        Label = $"{cateory}",
                        ValueLabel = $"{string.Format("{0:N2}", categorySum)}"
                    };

                    entrys.Add(entry);
                }
            }

            return new DonutChart { Entries = entrys, LabelTextSize = 40f, BackgroundColor = SKColor.Parse(Color.Transparent.ToHex()) };
        }
    }
}
