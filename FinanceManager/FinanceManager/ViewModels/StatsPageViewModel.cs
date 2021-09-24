using Microcharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Entry = Microcharts.ChartEntry;


namespace FinanceManager.ViewModels
{
    class StatsPageViewModel : BaseModel
    {
        private string _currentAppliedFilter = "OverView";
        public string CurrentAppliedFilter
        {
            get { return _currentAppliedFilter; }
            set
            {
                if (_currentAppliedFilter != value)
                {
                    _currentAppliedFilter = value;
                        ApplyeOverView.Execute(value);
                    OnPropertyChanged(nameof(CurrentAppliedFilter));
                }
            }
        }
        private float _balance;

        public float Balance {
            get { return _balance; }
            set
            {
                if (_balance != value)
                {
                    _balance = value;
                    OnPropertyChanged(nameof(CurrentAppliedFilter));
                }
            }
        }


        public Chart GrafData { get; set; }

        private DateTime _currentShowDate = DateTime.Now;
        public DateTime CurrentShowDate
        {
            get { return _currentShowDate; }
            set
            {
                if (_currentShowDate != value)
                {
                    _currentShowDate = value;
                    OnPropertyChanged(nameof(CurrentShowDate));
                }
            }
        }

        public ICommand NextMonth { get; private set; }
        public ICommand PrevMonth { get; private set; }
        public ICommand ApplyeOverView { get; private set; }


        private Style _colorNextButton = (Style)Application.Current.Resources["MainButtonUnChecked"];
        public Style ColorNextButton
        {
            get { return _colorNextButton; }
            set
            {
                if (_colorNextButton != value)
                {
                    _colorNextButton = value;
                    OnPropertyChanged(nameof(ColorNextButton));
                }
            }
        }

        private Style _colorPrevButton = (Style)Application.Current.Resources["MainButtonUnChecked"];
        public Style ColorPrevButton
        {
            get { return _colorPrevButton; }
            set
            {
                if (_colorPrevButton != value)
                {
                    _colorPrevButton = value;
                    OnPropertyChanged(nameof(ColorPrevButton));
                }
            }
        }


        public async void ValueChangeMethod(string grafType )
        {
            if (grafType == "OverView")
                GrafData = await Services.ChartGenerator.GetOverView(CurrentShowDate);
            else if (grafType == "Incoms")
                GrafData = await Services.ChartGenerator.GetIncomesGraf(CurrentShowDate);
            else if (grafType == "Expences")
                GrafData = await Services.ChartGenerator.GetExpencesCategory(CurrentShowDate);

            OnPropertyChanged(nameof(GrafData));
        }

        public StatsPageViewModel()
        {
            Task.Run(async () =>
            {
                GrafData = await Services.ChartGenerator.GetOverView(CurrentShowDate);
                var incomeSum = await Services.DatabaseConnection.GetFunctionResult($"SELECT SUM(Price) FROM \"Transaction\" WHERE Type = \"Income\" ");
                var expenseSum = await Services.DatabaseConnection.GetFunctionResult($"SELECT SUM(Price) FROM \"Transaction\" WHERE Type = \"Expense\" ");
                Balance = incomeSum - expenseSum;
            }).Wait();

            NextMonth = new Command(async =>
            {
                DateTime dt = new DateTime(DateTime.Now.Year, 12, 1);

                if (CurrentShowDate.Month != dt.Month)
                {
                    CurrentShowDate = CurrentShowDate.AddMonths(1);

                    ColorPrevButton = (Style)Application.Current.Resources["MainButtonUnChecked"];

                    if (CurrentShowDate.Month == dt.Month)
                        ColorNextButton = (Style)Application.Current.Resources["MainButtonChecked"];
                   ValueChangeMethod(CurrentAppliedFilter);
                }
            });

            PrevMonth = new Command(async =>
            {
                DateTime dt = new DateTime(DateTime.Now.Year, 1, 1);

                if (CurrentShowDate.Month != dt.Month)
                {
                    CurrentShowDate = CurrentShowDate.AddMonths(-1);
                    ColorNextButton = (Style)Application.Current.Resources["MainButtonUnChecked"];

                    if (CurrentShowDate.Month == dt.Month)
                        ColorPrevButton = (Style)Application.Current.Resources["MainButtonChecked"];
                    ValueChangeMethod(CurrentAppliedFilter);
                }
            });

            ApplyeOverView = new Command<string>(async value =>
                {
                    ValueChangeMethod(value);
                });
        }
    }
}