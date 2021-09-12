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

        }
    }
}