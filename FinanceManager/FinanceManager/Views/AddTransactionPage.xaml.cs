using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FinanceManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddTransactionPage : ContentPage
    {
        public List<string> Accounts { get; set; } = new List<string> 
        { 
            "Cash",
            "Revolut",
            "BCR",
            "BRD"
        };

        public List<string> Categorys { get; set; } = new List<string>
        {
            "Shopping",
            "Utilities",
            "Salary",
            "Services",
            "Food",
            "Gadgets"
        };

        public List<string> Types { get; set; } = new List<string>
        {
            "Income",
            "Expense",
        };

        public string Name { get; set; }
        public string Description { get; set; }
        public float? Price { get; set; } = null;
        public DateTime Date { get; set; } = DateTime.Now;

        public string SelectedAccount { get; set; }
        public string SelectedCategory { get; set; }
        public string SelectedTypes { get; set; }

        public AddTransactionPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private async void SaveBtn_Clicked(object sender, EventArgs e)
        {
          await Services.DatabaseConnection.AddTransaction(new Models.Transaction
            {
                Name = Name,
                Description = Description,
                Price = (float)Price,
                Category = SelectedCategory,
                Date = Date,
                Type = SelectedTypes,
                Account=SelectedAccount
            });

          await Navigation.PopAsync();
        }

        private async void CancelBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}