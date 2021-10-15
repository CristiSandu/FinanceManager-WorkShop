# Clipboard Timeline

## Add before start

**App.xaml**

```xml
<Application.Resources>
        <ResourceDictionary>
            <!--App Color Pallet-->
            <Color x:Key="AccentYellow">#F4E8C1</Color>
            <Color x:Key="AccentLiteGreen">#A0C1B9</Color>
            <Color x:Key="AccentDarkGreen">#70A0AF</Color>
            <Color x:Key="AccentLitePurple">#706993</Color>
            <Color x:Key="AccentDarkPurple">#331E38</Color>

            <!--Chart Color Pallet-->
            <Color x:Key="ChartColor1">#FF0000</Color>
            <Color x:Key="ChartColor2">#FF8700</Color>
            <Color x:Key="ChartColor3">#FFD300</Color>
            <Color x:Key="ChartColor4">#DEFF0A</Color>
            <Color x:Key="ChartColor5">#A1FF0A</Color>
            <Color x:Key="ChartColor6">#0AFF99</Color>
            <Color x:Key="ChartColor7">#0AEFFF</Color>
            <Color x:Key="ChartColor8">#147DF5</Color>
            <Color x:Key="ChartColor9">#580AFF</Color>
            <Color x:Key="ChartColor10">#BE0AFF</Color>

            <!--Background Colors-->
            <Color x:Key="BackgroundDark">#2E2E2E</Color>
            <Color x:Key="BackgroundLight">#FAFAFA</Color>
        </ResourceDictionary>
</Application.Resources>
```

## Welcome Page

**App.xaml -> ResourceDictionary**

```xml
    <Style x:Key="MainButtonPress" TargetType="Button">
        <Setter Property="CornerRadius" Value="20"/>
        <Setter Property="TextColor" Value="{AppThemeBinding  Dark={StaticResource BackgroundDark}, Light= {StaticResource BackgroundLight}}" />
        <Setter Property="BackgroundColor" Value="{AppThemeBinding Dark={StaticResource AccentYellow}, Light={StaticResource AccentDarkGreen}}" />
        <Setter Property="FontAttributes" Value="Bold"/>
        <Setter Property="FontSize" Value="16"/>
        <Setter Property="HorizontalOptions" Value="Center"/>
    </Style>

    <Style x:Key="MainButtonUnPress" TargetType="Button">
        <Setter Property="CornerRadius" Value="20" />
        <Setter Property="TextColor" Value="{AppThemeBinding Dark={StaticResource AccentYellow}, Light={StaticResource AccentDarkGreen}}"/>
        <Setter Property="BackgroundColor" Value="Transparent" />
        <Setter Property="BorderColor" Value="{AppThemeBinding Dark={StaticResource AccentYellow}, Light={StaticResource AccentDarkGreen}}"/>
        <Setter Property="BorderWidth" Value="2"/>
        <Setter Property="FontAttributes" Value="Bold"/>
        <Setter Property="FontSize" Value="16"/>
        <Setter Property="HorizontalOptions" Value="Center"/>
    </Style>

    <Style x:Key="DefaultFrame" TargetType="Frame">
        <Setter Property="CornerRadius" Value="5"/>
        <Setter Property="Padding" Value="10"/>
        <Setter Property="BorderColor" Value="{AppThemeBinding Dark={StaticResource AccentYellow}, Light={StaticResource AccentDarkGreen}}" />
        <Setter Property="BackgroundColor" Value="Transparent"/>
    </Style>

    <Style x:Key="TitleStyle" TargetType="Label">
        <Setter Property="FontAttributes" Value="Bold"/>
        <Setter Property="FontSize" Value="Medium"/>
        <Setter Property="TextColor" Value="{AppThemeBinding Light={StaticResource BackgroundLight}, Dark={StaticResource BackgroundDark}}"/>
        <Setter Property="VerticalOptions" Value="Center"/>
    </Style>

    <Style x:Key="TransactionListStyle" TargetType="Frame">
        <Setter Property="BorderColor" Value="{AppThemeBinding Dark={StaticResource AccentYellow}, Light={StaticResource AccentDarkGreen}}" />
        <Setter Property="Padding" Value="10,10,10,10" />
        <Setter Property="CornerRadius" Value="10"/>
        <Setter Property="BackgroundColor" Value="{AppThemeBinding Dark={StaticResource AccentYellow}, Light={StaticResource AccentDarkGreen}}" />
        <Setter Property="Opacity" Value="1"/>
        <Setter Property="HasShadow" Value="True"/>
    </Style>
```

**.csproj**

```xml
  <ItemGroup>
    <SharedImage Include="Img\arrow_down.svg" BaseSize="21, 21" />
    <SharedImage Include="Img\arrow_up.svg" BaseSize="21, 21" />
    <SharedImage Include="Img\bar_chart_2.svg" BaseSize="35, 35" />
    <SharedImage Include="Img\dollar_sign.svg" BaseSize="35, 35" />
    <SharedImage Include="Img\personal_finance.svg" BaseSize="320, 320" />
    <SharedImage Include="Img\personal_finance_light.svg" BaseSize="320, 320" />
    <SharedImage Include="Img\shopping_bag.svg" BaseSize="35, 35" />
  </ItemGroup>
```

## AddAccountPage

**Account.cs**

```csharp
    public class Account
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public float Balance { get; set; }
    }
```

**Account.cs**

```csharp
    public class Transaction
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public float Price { get; set; }

        public string Category { get; set; }

        public DateTime Date { get; set; }

        public string Type { get; set; }

        public int Account { get; set; }
    }

```

**DatabaseConnection.cs**

```csharp
    public static class DatabaseConnection
    {
        static SQLiteAsyncConnection db;

        static async Task Init()
        {
            if (db != null)
                return;

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "expensesDb.db");
            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTableAsync<Account>();
            await db.CreateTableAsync<Transaction>();
        }

        // Account Operations
        public static async Task AddAccount(Account account)
        {
            await Init();
            await db.InsertAsync(account);
        }

        public static async Task<List<Account>> GetAccounts()
        {
            await Init();
            return await db.Table<Account>().ToListAsync();
        }

        public static async Task UpdateAccount(Account acc)
        {
            await Init();
            await db.UpdateAsync(acc);
        }

        public static async Task<List<Account>> GetAccountByName(string name)
        {
            await Init();
            string query = $"SELECT * FROM \"Account\" WHERE Name = \"{name}\"";
            var trans = await db.QueryAsync<Account>(query);

            return trans;
        }

        public static async Task<bool> VerifyIfAccExist(string name)
        {
            await Init();
            List<Account> trans = await GetAccountByName(name);

            return trans.Count == 0 ? true : false;
        }

        // Transaction Operations
        public static async Task AddTransaction(Transaction transaction)
        {
            await Init();
            await db.InsertAsync(transaction);
        }

        public static async Task<IEnumerable<Transaction>> GetGlobalTransactions()
        {
            await Init();
            return await db.Table<Transaction>().ToListAsync();
        }

        public static async Task<IEnumerable<Transaction>> GetIncomeTransactions()
        {
            await Init();
            var trans = await db.QueryAsync<Transaction>($"SELECT * FROM \"Transaction\" WHERE Type = \"Income\"");
            return trans;
        }

        public static async Task<IEnumerable<Transaction>> GetExpensesTransactions()
        {
            await Init();
            string query = "SELECT * FROM \"Transaction\" WHERE Type = \"Expense\"";
            var trans = await db.QueryAsync<Transaction>(query);
            return trans;
        }

        // Functions that return a float method
        public static async Task<float> GetFunctionResult(string query)
        {
            await Init();

            var incomeSum = await db.ExecuteScalarAsync<float>(query);
            return incomeSum;
        }
    }
```

**AddAccountPage.xaml.cs -> Button_Clicked**

```csharp
    if (Name == null || Name == "" || InitialBalance == null)
    {
        await DisplayAlert("Alert!", "Fill all fields", "Ok");
        return;
    }

    if (await Services.DatabaseConnection.VerifyIfAccExist(Name))
    {
        await Services.DatabaseConnection.AddAccount(
            new Models.Account
            {
                Name = Name,
                Balance = InitialBalance
            });
        await DisplayAlert("Success!", "Account Added", "Ok");
    }
    else
        await DisplayAlert("Alert!", "Account Exist, try another Name", "Ok");

    accountName.Text = "";
    accountBalance.Text = "0";

    Name = "";
    InitialBalance = 0;
```

## Tabbed Page

To Move TabbedBar to bottom on Android

```xml
<TabbedPage
    .....
    xmlns:android="clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific;assembly=Xamarin.Forms.Core"
    android:TabbedPage.ToolbarPlacement="Bottom"
    android:TabbedPage.IsSwipePagingEnabled="False"
    ..... >

    ......
</TabbedPage>

```

## Accounts Page

**AccountsPage.xaml -> CollectionView -> CollectionView.ItemTemplate**

```xml
<CollectionView.ItemTemplate>
    <DataTemplate>
        <Frame Style="{StaticResource TransactionListStyle}" Padding="5,5,2,5">
            <Grid Padding="10,10,10,10">
                <Grid.RowDefinitions>
                    <RowDefinition Height="Auto" />
                </Grid.RowDefinitions>

                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="1.5*" />
                    <ColumnDefinition Width="Auto" />
                </Grid.ColumnDefinitions>

                <Label Text="{Binding Name}"
                        Grid.Row="0"
                        FontAttributes="Bold"
                        FontSize="Medium"
                        TextColor="{AppThemeBinding Light={StaticResource BackgroundLight}, Dark={StaticResource BackgroundDark}}"
                        Style="{StaticResource TitleStyle}"/>

                <Label Grid.Column="1"
                        HorizontalOptions="EndAndExpand"
                        Text="{Binding Path=Balance,StringFormat='{0:F2} Lei'}"
                        TextColor="{AppThemeBinding Light={StaticResource BackgroundLight}, Dark={StaticResource BackgroundDark}}"
                        FontAttributes="Bold"
                        FontSize="Medium"
                        Style="{StaticResource TitleStyle}"/>
            </Grid>
        </Frame>
    </DataTemplate>
</CollectionView.ItemTemplate>

```

**AccountsPage.xaml -> CollectionView -> CollectionView.Header**

```xml
<CollectionView.Header>
    <ContentView>
        <Label x:Name="totalMoney"
                Padding="16"
                HorizontalOptions="Center"
                TextColor="{AppThemeBinding Light={StaticResource BackgroundDark}, Dark={StaticResource BackgroundLight}}"
                FontAttributes="Bold"
                FontSize="Medium"
                Style="{StaticResource TitleStyle}"/>
    </ContentView>
</CollectionView.Header>
```

**AccountsPage.xaml -> OnAppearing **

```csharp
protected async override void OnAppearing()
{
    base.OnAppearing();
    AccountsList = new ObservableCollection<Models.Account>(await Services.DatabaseConnection.GetAccounts());
    accountsList.ItemsSource = AccountsList;
    float sum = await Services.DatabaseConnection.GetFunctionResult("SELECT SUM(Balance) FROM \"Account\"");
    totalMoney.Text = sum.ToString("Total: 0 Lei");
}
```

## Expenses Page

**ExpensesPage.xaml**

**ExpensesPage.xaml -> CollectionView -> CollectionView.ItemTemplate**

```xml
<CollectionView.ItemTemplate>
    <DataTemplate>
        <Frame Style="{StaticResource TransactionListStyle}" Padding="5,5,2,5"  >

            <Grid Padding="10,10,10,10">
                <Grid.RowDefinitions>
                    <RowDefinition Height="Auto" />
                    <RowDefinition Height="Auto" />
                </Grid.RowDefinitions>

                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="1.5*" />
                    <ColumnDefinition Width="1.5*" />
                    <ColumnDefinition Width="Auto" />
                    <ColumnDefinition Width="Auto" />
                </Grid.ColumnDefinitions>

                <Label Text="{Binding Name}"
                        FontAttributes="Bold"
                        FontSize="Medium"
                        Style="{StaticResource TitleStyle}"/>

                <Label Grid.Row="1"
                        HorizontalOptions="StartAndExpand"
                        Text="{Binding Date,StringFormat='{0:dd/MM/yyyy}'}"
                        FontSize="Small"
                        FontAttributes="Bold"
                        Style="{StaticResource TitleStyle}"
                        VerticalOptions="End"/>

                <Label Grid.Row="1"
                        Grid.Column="1"

                        Text="{Binding Path=Category}"
                        HorizontalOptions="Start"
                        FontAttributes="Bold"
                        FontSize="Small"
                        Style="{StaticResource TitleStyle}"/>

                <Label Grid.RowSpan="2"
                        Grid.Column="2"
                        HorizontalOptions="EndAndExpand"
                        Text="{Binding Path=Price,StringFormat='{0:F2} Lei'}"
                        FontAttributes="Bold"
                        FontSize="Medium"
                        Style="{StaticResource TitleStyle}"/>

                <Image Grid.RowSpan="2"
                        Grid.Column="3" Source="{Binding Type, Converter={StaticResource transactionTypeConvertor}}"/>
            </Grid>
        </Frame>
    </DataTemplate>
</CollectionView.ItemTemplate>

```

**ExpensesPage.xaml -> CollectionView -> CollectionView.EmptyView**

```xml
<CollectionView.EmptyView>
    <ContentView>
        <StackLayout HorizontalOptions="CenterAndExpand"
                        VerticalOptions="CenterAndExpand">
            <StackLayout Orientation="Horizontal" HorizontalOptions="Center" VerticalOptions="Center">
                <Label Text="Press "
                        TextColor="{AppThemeBinding Light={StaticResource BackgroundDark}, Dark={StaticResource BackgroundLight}}"
                        FontAttributes="Bold"
                        FontSize="18"
                        HorizontalOptions="Center"
                        VerticalOptions="Center"
                        HorizontalTextAlignment="Center" />

                <Label Text="+"
                        TextColor="{AppThemeBinding Light={StaticResource BackgroundDark}, Dark={StaticResource BackgroundLight}}"
                        WidthRequest="20"
                        FontAttributes="Bold"
                        FontSize="34"
                        VerticalOptions="Center"
                        HorizontalTextAlignment="Center" />
            </StackLayout>
            <Label  Text="for adding new Transaction"
                    TextColor="{AppThemeBinding Light={StaticResource BackgroundDark}, Dark={StaticResource BackgroundLight}}"
                    FontAttributes="Bold"
                    FontSize="18"
                    VerticalOptions="Center"
                    HorizontalTextAlignment="Center" />

        </StackLayout>
    </ContentView>
</CollectionView.EmptyView>
```

**DropDown Code**

```xml
<Frame Style="{StaticResource FrameAddNews}" Padding="8">
    <StackLayout >
        <StackLayout Orientation="Horizontal" VerticalOptions="Center" HeightRequest="26" Padding="5,0,0,0">
            <Label Text="Filters:"
                   FontSize="Large"
                   FontAttributes="Bold"
                   TextColor="{AppThemeBinding Dark={StaticResource BackgroundLight}, Light={StaticResource AccentDarkPurple}}"
                   VerticalOptions="Center"
                   HorizontalOptions="Start"
                   Padding="0,-3,0,0"/>
            <Label x:Name="selectedFilter"
                   IsVisible="False"
                   Text="Global"
                   TextColor="{AppThemeBinding Dark={StaticResource BackgroundLight}, Light={StaticResource AccentDarkPurple}}"
                   FontSize="Large"
                   HorizontalOptions="Start"
                   FontAttributes="Italic"
                   VerticalOptions="Center"
                   Padding="0,-3,0,0"/>
            <Button x:Name="hideButton"
                    Text="-"
                    FontAttributes="Bold"
                    FontSize="24"
                    TextTransform="Uppercase"
                    CornerRadius="20"
                    Padding="-5"
                    BackgroundColor="Transparent"
                    BorderWidth="2"
                    BorderColor="{StaticResource AccentDarkGreen}"
                    TextColor="{StaticResource AccentDarkGreen}"
                    HeightRequest="26"
                    Clicked="hideFilters_Clicked"
                    HorizontalOptions="EndAndExpand"
                    WidthRequest="26"/>
        </StackLayout>
        <FlexLayout x:Name="filtersList"
                    BindableLayout.ItemsSource="{Binding BillListFilters}"
                    Wrap="Wrap"
                    VerticalOptions="Start"
                    JustifyContent="Start"
                    AlignItems="Start" >
            <BindableLayout.ItemTemplate>
                <DataTemplate>
                    <StackLayout Padding="5,5,5,5">
                        <Button Text="{Binding .}" Style="{StaticResource MainButtonChecked}"  Clicked="Filter_Clicked"/>
                    </StackLayout>
                </DataTemplate>
            </BindableLayout.ItemTemplate>
        </FlexLayout>
    </StackLayout>
</Frame>

```

**ExpensesPage.xaml -> Filter_Clicked **

```csharp
    Button btn = sender as Button;

    if (btn != null && btn.Text != CurrentCheck.Text)
    {
        btn.Style = (Style)Application.Current.Resources["MainButtonUnChecked"];
        CurrentCheck.Style = (Style)Application.Current.Resources["MainButtonChecked"];
        CurrentCheck = btn;

        switch(btn.Text)
        {
            case "Income":
                TransactionsList = new ObservableCollection<Models.Transaction>(await Services.DatabaseConnection.GetIncomeTransactions());
                break;
            case "Expenses":
                TransactionsList = new ObservableCollection<Models.Transaction>(await Services.DatabaseConnection.GetExpensesTransactions());
                break;
            default:
                TransactionsList = new ObservableCollection<Models.Transaction>(await Services.DatabaseConnection.GetGlobalTransactions());
                break;
        }

        expensesList.ItemsSource = TransactionsList;
        selectedFilter.Text = btn.Text;

        CurrentCheck = btn;
    }
```

## AddTransactionPage.xaml

```xml
<ContentPage.Resources>
    <ResourceDictionary>
        <Style x:Key="TextColorLabel" TargetType="Label">
            <Setter Property="TextColor" Value="{AppThemeBinding Dark={StaticResource BackgroundLight}, Light={StaticResource BackgroundDark}}"/>
        </Style>
        <Style x:Key="TextColorEntry" TargetType="Entry">
            <Setter Property="TextColor" Value="{AppThemeBinding Dark={StaticResource BackgroundLight}, Light={StaticResource BackgroundDark}}"/>
        </Style>
    </ResourceDictionary>
</ContentPage.Resources>
<ContentPage.Content>
    <ScrollView>
        <StackLayout Spacing="24">
            <StackLayout Padding="16,16,16,0" Spacing="8">
                <Frame Style="{StaticResource FrameAddNews}" >
                    <StackLayout Spacing="0">
                        <Label x:Name="DescriprionName" Text="Name" Style="{StaticResource TextColorLabel}"/>
                        <Entry x:Name="TransactionName" Style="{StaticResource TextColorLabel}" Text="{Binding Name}"/>
                    </StackLayout>
                </Frame>

                <Frame Style="{StaticResource FrameAddNews}" >
                    <StackLayout Spacing="0">
                        <Label x:Name="DescriprionPrice" Text="Price" Style="{StaticResource TextColorLabel}" />
                        <Entry x:Name="TransactionPrice" Text="{Binding Price}" Style="{StaticResource TextColorLabel}" Keyboard="Numeric"/>
                    </StackLayout>
                </Frame>

                <Grid>
                    <Grid.ColumnDefinitions>
                        <ColumnDefinition Width="* "/>
                        <ColumnDefinition Width="*"/>
                    </Grid.ColumnDefinitions>
                    <Grid.RowDefinitions>
                        <RowDefinition Height="Auto"/>
                        <RowDefinition Height="*"/>
                    </Grid.RowDefinitions>
                    <Frame Grid.Row="0" Grid.Column="0" Style="{StaticResource FrameAddNews}" >
                        <StackLayout Spacing="0">
                            <Label x:Name="DescriptionCard" Text="Account" Style="{StaticResource TextColorLabel}"/>

                            <Picker ItemsSource="{Binding Accounts}"
                                    ItemDisplayBinding="{Binding Name}"
                                    SelectedItem="{Binding SelectedAccount}"
                                    Style="{StaticResource TextColorLabel}"/>

                        </StackLayout>
                    </Frame>
                    <Frame Grid.Row="0" Grid.Column="1" Style="{StaticResource FrameAddNews}" >
                        <StackLayout Spacing="0">
                            <Label x:Name="DescriptionCategory" Text="Category" Style="{StaticResource TextColorLabel}"/>
                            <Picker ItemsSource="{Binding Categorys}"
                                    ItemDisplayBinding="{Binding .}"
                                    SelectedItem="{Binding SelectedCategory}"  Style="{StaticResource TextColorLabel}" />
                        </StackLayout>
                    </Frame>
                </Grid>

                <Grid>
                    <Grid.ColumnDefinitions>
                        <ColumnDefinition Width="* "/>
                        <ColumnDefinition Width="*"/>
                    </Grid.ColumnDefinitions>
                    <Grid.RowDefinitions>
                        <RowDefinition Height="Auto"/>
                        <RowDefinition Height="*"/>
                    </Grid.RowDefinitions>
                    <Frame Grid.Row="0" Grid.Column="0" Style="{StaticResource FrameAddNews}" >
                        <StackLayout Spacing="0">
                            <Label x:Name="DescriptionDate" Text="Date" Style="{StaticResource TextColorLabel}" />
                            <DatePicker x:Name="TransactionDate" TextColor="Gray" Date="{Binding Date}"/>
                        </StackLayout>
                    </Frame>
                    <Frame Grid.Row="0" Grid.Column="1" Style="{StaticResource FrameAddNews}" >
                        <StackLayout Spacing="0">
                            <Label x:Name="DescriptionType" Text="Type" Style="{StaticResource TextColorLabel}" />
                            <Picker ItemsSource="{Binding Types}"
                                    ItemDisplayBinding="{Binding .}"
                                    SelectedItem="{Binding SelectedTypes}"
                                        Style="{StaticResource TextColorLabel}"/>
                        </StackLayout>
                    </Frame>
                </Grid>
                <Frame Style="{StaticResource FrameAddNews}" >
                    <StackLayout Spacing="0">
                        <Label x:Name="DescriptionDesc" Text="Description" Style="{StaticResource TextColorLabel}"/>
                        <Editor x:Name="TransactionDescription" Style="{StaticResource TextColorLabel}" HeightRequest="100" Text="{Binding Description}"/>
                    </StackLayout>
                </Frame>
            </StackLayout>
            <StackLayout Orientation="Horizontal" Spacing="20" HorizontalOptions="Center">
                <Button x:Name="SaveBtn" Text="Save" Style="{StaticResource MainButtonUnChecked}" Clicked="SaveBtn_Clicked"/>
                <Button x:Name="CancelBtn" Text="Cancel" Style="{StaticResource MainButtonChecked}" Clicked="CancelBtn_Clicked" />
            </StackLayout>
        </StackLayout>
    </ScrollView>
</ContentPage.Content>
```

## Stats Page

**BaseViewModel.cs**

```csharp
public class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        var handler = PropertyChanged;
        if (handler != null)
            handler(this, new PropertyChangedEventArgs(propertyName));
    }
}
```
