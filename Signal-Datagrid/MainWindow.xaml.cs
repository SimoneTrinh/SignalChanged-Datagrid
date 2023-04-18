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
            var btc = new Item("BTC", 0);
            var eth = new Item("ETH", 0);
            var ltc = new Item("LTC", 0);

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
                    double minValue = 0.0000;
                    double maxValue = 1000.0000;
                    double randomNumber = minValue + (random.NextDouble() * (maxValue - minValue));
                    randomNumber = Math.Round(randomNumber, 4, MidpointRounding.AwayFromZero);

                    //double roundedNumber = Math.Floor(randomNumber * 100) / 100.000;
                    // Randomly adjust the price up or down
                    //var priceChange = double.Parse(Math.Round((random.NextDouble() * 10 - 5), 4, MidpointRounding.AwayFromZero).ToString("0.0000"));
                    var currentPrice = item.Price;
                    var newPrice = randomNumber;
                    item.Price = newPrice;

                    // Update the trend
                    if (currentPrice < newPrice)
                    {
                        item.Trend = "up";
                    }
                    else
                    {
                        item.Trend = "down";
                    }
                    //Fix the rounding because it plus from previous number so it can not use the pure rounding.
                    //TODO: Add % changed and regarding with text color, need to manage data context

                }

                Thread.Sleep(random.Next(1000, 3000));
            }
        }

        public class Item : INotifyPropertyChanged
        {
            public Item(string name, double price)
            {
                Name = name;
                Price = price;
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
