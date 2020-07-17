using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using APILibrary;   // So we can access the classes responsible for calling APIs etc.

namespace CurrencyConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // For instantiating our API classes.
        private APIHelper apiHelper;
        private ConvertionProcessor processor;

        // Global variables for storing useful data.
        private string currency_1 = string.Empty;
        private string currency_2 = string.Empty;
        private float amount = 0.0f;

        public MainWindow()
        {
            InitializeComponent();

            // Create and initialize the APIHelper object.
            apiHelper = new APIHelper();
            apiHelper.InitializeClient();
            processor = new ConvertionProcessor();

            // We load this method to initialize the boxes with the available currencies.
            LoadListBoxes();
        }

        // Asynchronous loading of available currencies.
        private async void LoadListBoxes ()
        {
            string[] currencies = await processor.LoadCurrencies(apiHelper);
            string output = string.Empty;
            foreach (string str in currencies)
            {
                listBox1.Items.Add(str);
                listBox2.Items.Add(str);
            }
        }

        // This lets the user pick a currency.
        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currency_1 = listBox1.SelectedItem.ToString();
        }

        // This lets the user pick a currency.
        private void listBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currency_2 = listBox2.SelectedItem.ToString();
        }

        // Asynchronous button handler for convertion.
        private async void Convert(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            // This shit didn't work. To bad!
            //string a = currencyAmount.Text;
            //a = Regex.Replace(a, "[a-z]", string.Empty);
            //a = Regex.Replace(a, "[A-Z]", string.Empty);
            //amount = float.Parse(a);

            if (currency_1 != null || currency_2 != null)
                output = await processor.ConvertCurrency(apiHelper, currency_1, currency_2, amount);

            outputValue.Text = $"1.0 {currency_1} converts to {output} {currency_2}";
        }

        private void currencyAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            // TODO: Stop being a small boy and actually fix the amount feature.
        }
    }
}
