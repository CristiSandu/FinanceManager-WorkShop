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
        public Button CurrentCheck { get; set; } 
        public ExpencesPage()
        {
            InitializeComponent();

            BillListFilters = new ObservableCollection<string>
            {
                "Income",
                "Expences",
                "Global"
            };
            BindingContext = this;
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
                
                CurrentCheck = btn;
            }
            else
            {
                CurrentCheck = btn;
            }

        }
    }
}