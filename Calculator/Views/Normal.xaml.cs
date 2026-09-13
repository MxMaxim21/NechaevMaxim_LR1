using Calculator.Services;
using Calculator.Services.Interfaces;
using NCalc;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Calculator.Views
{
    /// <summary>
    /// Логика взаимодействия для Normal.xaml
    /// </summary>
    public partial class Normal : Page
    {
        private readonly ICalculationService _calcService = new CalculationService();
        private readonly IHistoryService _historyService = new FileHistoryService();
        
        public Normal()
        {
            InitializeComponent();
        }

        private void Number_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            string content = button.Content.ToString();
            string text = TboxOut.Text;

            string ops = "+-*/";

            if (ops.Contains(content))
            {
                if (string.IsNullOrEmpty(text)) return;

                char lastChar = text[text.Length - 1];

                if (ops.Contains(lastChar.ToString()) || lastChar == '.')
                {
                    TboxOut.Text = text.Substring(0, text.Length - 1) + content;
                    return;
                }
            }

            if (TboxOut.Text.Length < 21)
            {
                TboxOut.Text += content;
            }
        }

        private void Equ_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string rawInput = TboxOut.Text;
                string result = _calcService.Calculate(rawInput);

                if (result != "Ошибка" && result != "Деление на 0")
                {
                    _historyService.SaveResult(rawInput, result);
                }

                TboxOut.Text = result;
            }
            catch
            {
                TboxOut.Text = "Ошибка";
            }
        }


        private bool _isDark = false;

        private void BtnColor_Click(object sender, RoutedEventArgs e)
        {
            if (!_isDark)
            {
                Application.Current.Resources["PrimaryBackground"] = new SolidColorBrush(Color.FromRgb(24, 28, 43));
                Application.Current.Resources["ButtonBackground"] = new SolidColorBrush(Color.FromRgb(38, 45, 71)); 
                Application.Current.Resources["ButtonBackgroundTwo"] = new SolidColorBrush(Color.FromRgb(53, 59, 89));
                Application.Current.Resources["ButtonBackgroundHover"] = new SolidColorBrush(Color.FromRgb(76, 86, 131));
                Application.Current.Resources["TextForeground"] = new SolidColorBrush(Color.FromRgb(220, 220, 225));
                _isDark = true;
            }
            else
            {
                Application.Current.Resources["PrimaryBackground"] = Brushes.OldLace;
                Application.Current.Resources["ButtonBackground"] = Brushes.SandyBrown;
                Application.Current.Resources["ButtonBackgroundTwo"] = Brushes.Peru;
                Application.Current.Resources["ButtonBackgroundHover"] = Brushes.DarkSalmon;
                Application.Current.Resources["TextForeground"] = Brushes.DarkGoldenrod;
                _isDark = false;
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            TboxOut.Clear();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (TboxOut.Text.Length > 0)
                TboxOut.Text = TboxOut.Text.Substring(0, TboxOut.Text.Length - 1);
        }

        private void AddSub_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TboxOut.Text)) return;
            TboxOut.Text = TboxOut.Text.StartsWith("-") ? TboxOut.Text.Substring(1) : "-" + TboxOut.Text;
        }

        private void Point_Click(object sender, RoutedEventArgs e)
        {
            string text = TboxOut.Text;
            string ops = "+-*/";

            if (string.IsNullOrEmpty(text) || ops.Contains(text[text.Length - 1].ToString()))
            {
                TboxOut.Text += "0.";
                return;
            }

            string[] parts = text.Split(new char[] { '+', '-', '*', '/' });
            string lastPart = parts[parts.Length - 1];

            if (!lastPart.Contains("."))
            {
                TboxOut.Text += ".";
            }
        }
    }
}