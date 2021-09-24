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
    public partial class ExpencesPage : ContentPage
    {

        public ObservableCollection<string> BillListFilters { get; set; }
        public ObservableCollection<Models.Transaction> TransactionsList { get; set; }

        public Button CurrentCheck { get; set; }

        public ExpencesPage()
        {
            InitializeComponent();

            BillListFilters = new ObservableCollection<string>
            {
                "Income",
                "Expences",
                "Global",
            };
            BindingContext = this;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            
            if (CurrentCheck == null)
            {
                TransactionsList = new ObservableCollection<Models.Transaction>(await Services.DatabaseConnection.GetGlobalTransactions());
                expemcesList.ItemsSource = TransactionsList;
            }
        }

        private async void Filter_Clicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Style == (Style)Application.Current.Resources["MainButtonUnChecked"])
                btn.Style = (Style)Application.Current.Resources["MainButtonChecked"];
            else
                btn.Style = (Style)Application.Current.Resources["MainButtonUnChecked"];

            if (btn.Text == "Income")
                TransactionsList = new ObservableCollection<Models.Transaction>(await Services.DatabaseConnection.GetIncomeTransactions());
            else if (btn.Text == "Expences")
                TransactionsList = new ObservableCollection<Models.Transaction>(await Services.DatabaseConnection.GetExpensesTransactions());
            else
                TransactionsList = new ObservableCollection<Models.Transaction>(await Services.DatabaseConnection.GetGlobalTransactions());

            expemcesList.ItemsSource = TransactionsList;
            selectedFilter.Text = btn.Text;

            if (CurrentCheck != null)
            {
                if (CurrentCheck.Style == (Style)Application.Current.Resources["MainButtonChecked"])
                    CurrentCheck.Style = (Style)Application.Current.Resources["MainButtonUnChecked"];
                else
                    CurrentCheck.Style = (Style)Application.Current.Resources["MainButtonChecked"];
            }

            CurrentCheck = btn;
        }

        private async void addTransaction_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddTransactionPage());
        }

        private void expemcesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void hideFilters_Clicked(object sender, EventArgs e)
        {
            if (filtersList.IsVisible)
            {
                filtersList.IsVisible = false;
                selectedFilter.IsVisible = true;
            }
            else
            {
                filtersList.IsVisible = true;
                selectedFilter.IsVisible = false;
            }
        }
    }
}