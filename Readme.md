# 💰 Finance Manager

## 📌 Introduction

In this workshop, we will create a mobile application with UI that looks good using Xamarin Forms.  
We will go through many notes. From the most introductory to the most complex and at the end we will have a Finance Manager that will look good and works as intended.

## 📑 Discussion Points

1. What is Xamarin ?
2. Application Design
3. Welcome Page
   - Add SVG - **Resizetizer.NT**
   - Styling - **Button** and **Labels**
   - Layouts - **StackLayout**
   - Pages - **ContentPage**
4. TabbedPage and Navigation
   - Create TabbedPage:
     - Positioning on bottom in Android
   - Navigation between WelcomePage and TabbedPage - **NavigationPage**
   - Tabs Icons
5. Expenses Page V1
   - **ContentPage - ToolbarItems** for top buttons
   - Default Color From Design in Android
   - Add page as Tab in TabbedPage
   - Filters
     - **BindableLayout** - **FlexLayout**
     - Global Styling - In **ResourceDictionary** App.xaml
   - DatabaseConnection - **SQLite**
   - **Collection View** - For Transactions
6. AddTransaction Page
   - Layout : **StackLayout + Grid**
   - **Picker** - Binding
   - **DatePicker**
   - **Frame** - Style
   - DataBinding
   - CodeBehind as View Model
7. Expenses Page V2
   - DataTemplate for Collection View Cell
   - Populate Collection View
   - Filters Design
   - Apply Filters in Backend
   - **Converters**
8. Stats Page
   - Charts generator
   - **MVVM** pattern
     - INotifyPropertyChanged
   - **Microcharts**
   - XAML Formatting
9. Accounts Page
   - Collection View
   - SQL complex Query
     ```sql
     SELECT IFNULL(a.ACC ,b.ACC) as Name , ( IFNULL(a.Balance,0) - IFNULL(b.Balance,0) ) as Balance
        FROM
        (SELECT Account as ACC ,SUM(Price) as Balance
            FROM \"Transaction\"
            WHERE Type = \"Income\"
         GROUP BY Account) a,
        (SELECT Account as ACC ,SUM(Price) as Balance
            FROM \"Transaction\"
            WHERE Type = \"Expense\"
         GROUP BY Account) b
     GROUP BY Name;
     ```
10. Settings
    - Theming: Dark/Light Mode
    - Xamarin.Essentials : **Preferences**
11. Get rid of the welcome screen after the first start
    - Xamarin.Essentials: **Version Tracking**
12. App Icon and Splash Screen

## 📖 Bibliography

- [Xamarin Forms Documentation](https://docs.microsoft.com/en-us/xamarin/get-started/what-is-xamarin)
- [Resizetizer.NT](https://www.nuget.org/packages/Resizetizer.NT/) - For adding SVGs
  - [Xamarin Developer Tutorial ](https://www.youtube.com/watch?v=zcUPh5cVWaE)
- [SQLite-Net-Pcl](https://www.nuget.org/packages/sqlite-net-pcl/)
  - [James Montemagno](https://www.youtube.com/channel/UCENTmbKaTphpWV2R2evVz2A) - [Tutorial](https://www.youtube.com/watch?v=XFP8Np-uRWc&t=904s)
  - [Microsoft Docs](https://docs.microsoft.com/en-us/xamarin/get-started/quickstarts/database?pivots=windows)
- [Microcharts.Forms](https://www.nuget.org/packages/Microcharts.Forms/)
  - [DevBlog - Tutorial](https://devblogs.microsoft.com/xamarin/microcharts-elegant-cross-platform-charts-for-any-app/)
  - [Gerald Versluis](https://www.youtube.com/channel/UCBBZ2kXWmd8eXlHg2wEaClw) - [Tutorial](https://www.youtube.com/watch?v=tLDxMKub5WA)
- Theming
  - [James Montemagno](https://www.youtube.com/channel/UCENTmbKaTphpWV2R2evVz2A) - [Tutorial](https://www.youtube.com/watch?v=4w8TQ8njd3w)

_powered by [MLSA UPB](https://facebook.com/profile.php?id=156173461134517&ref=content_filter)_
