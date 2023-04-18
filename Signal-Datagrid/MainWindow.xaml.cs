using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Signal_Datagrid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            // Create the items
            var btc = new Item("BTC");
            var eth = new Item("ETH");
            var ltc = new Item("LTC");

            // Add the items to the collection
            Items.Add(btc);
            Items.Add(eth);
            Items.Add(ltc);

            // Start the price update thread
            var thread = new Thread(new ThreadStart(UpdatePrices));
            thread.IsBackground = true;
            thread.Start();
        }

        // The collection of items
        public ObservableCollection<Item> Items { get; } = new ObservableCollection<Item>();

        // The price update thread
        private void UpdatePrices()
        {
            var random = new Random();

            while (true)
            {
                foreach (var item in Items)
                {
                    // Randomly adjust the price up or down
                    var priceChange = random.NextDouble() * 10 - 5;
                    item.Price += priceChange;

                    // Update the trend
                    if (priceChange > 0)
                    {
                        item.Trend = "up";
                    }
                    else
                    {
                        item.Trend = "down";
                    }
                }

                Thread.Sleep(random.Next(3000, 5000));
            }
        }

        public class Item : INotifyPropertyChanged
        {
            public Item(string name)
            {
                Name = name;
            }

            private string _name;

            public string Name
            {
                get { return _name; }
                set
                {
                    _name = value;
                    NotifyPropertyChanged(nameof(Name));
                }
            }

            private double _price;

            public double Price
            {
                get { return _price; }
                set
                {
                    _price = value;
                    NotifyPropertyChanged(nameof(Price));
                }
            }

            private string _trend;

            public string Trend
            {
                get { return _trend; }
                set
                {
                    _trend = value;
                    NotifyPropertyChanged(nameof(Trend));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            private void NotifyPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    //public class TrendColorConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        if (value is string trend)
    //        {
    //            switch (trend)
    //            {
    //                case "Up":
    //                    return new SolidColorBrush(Colors.Green);
    //                case "Down":
    //                    return new SolidColorBrush(Colors.Red);
    //            }
    //        }

    //        return new SolidColorBrush(Colors.Black);
    //    }
    //    public object ConvertBack(object value, Type targetType, object parameter,
    //            System.Globalization.CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    public class TrendColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string trend)
            {
                if (trend == "up")
                {
                    return new SolidColorBrush(Colors.Green);
                }
                else if (trend == "down")
                {
                    return new SolidColorBrush(Colors.Red);
                }
            }
            return new SolidColorBrush(Colors.Black); // Default color if trend value is not recognized
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //public class TrendBackgroundConverter : IMultiValueConverter
    //{
    //    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (values.Length == 2 && values[0] is string trend && values[1] is double price)
    //        {
    //            if (trend == "up")
    //            {
    //                return new SolidColorBrush(Colors.Yellow);
    //            }
    //            else if (trend == "down")
    //            {
    //                return new SolidColorBrush(Colors.Gray);
    //            }
    //        }
    //        return new SolidColorBrush(Colors.Transparent); // Default background color
    //    }

    //    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
    public class TrendBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string trend)
            {
                if (trend == "up")
                {
                    return new SolidColorBrush(Colors.Yellow);
                }
                else if (trend == "down")
                {
                    return new SolidColorBrush(Colors.Gray);
                }
            }
            return new SolidColorBrush(Colors.Black); // Default background color
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}
