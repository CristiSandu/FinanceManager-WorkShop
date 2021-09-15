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
                "Global",
                "Global",
                "Global",
                "Global",
                "Global",
                "Global"
            };
            BindingContext = this;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            TransactionsList = new ObservableCollection<Models.Transaction>(await Services.DatabaseConnection.GetGlobalTransactions());
            expemcesList.ItemsSource = TransactionsList;
        }

        private void Filter_Clicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Style == (Style)Application.Current.Resources["MainButtonUnChecked"])
                btn.Style = (Style)Application.Current.Resources["MainButtonChecked"];
            else
                btn.Style = (Style)Application.Current.Resources["MainButtonUnChecked"];


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
                filtersList.IsVisible = false;
            else
                filtersList.IsVisible = true;
        }

        private void hideButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}