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
    public partial class Engineering : Page
    {
        private readonly ICalculationService _calcService = new CalculationService();
        private readonly IHistoryService _historyService = new FileHistoryService();

        public Engineering()
        {
            InitializeComponent();
        }

        private void Number_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            string content = button.Content.ToString();
            string text = TboxOut.Text;
            string ops = "+-*/^%";

            if (!string.IsNullOrEmpty(text))
            {
                char lastChar = text[text.Length - 1];

                if (ops.Contains(content) && ops.Contains(lastChar.ToString()))
                {
                    TboxOut.Text = text.Substring(0, text.Length - 1) + content;
                    return;
                }

                if (lastChar == '(' && ops.Contains(content) && content != "-") return;

                if (content == ")" && lastChar == '(') return;
                if (content == "." && (lastChar == '(' || ops.Contains(lastChar.ToString()))) return;
            }
            else
            {
                if (content == ")" || content == "%" || content == "^" || content == "*" || content == "/") return;
            }

            if (content == "1/х")
            {
                if (!string.IsNullOrEmpty(text) && (char.IsDigit(text[text.Length - 1]) || text.EndsWith(")")))
                {
                    TboxOut.Text += "*1/(";
                }
                else
                {
                    TboxOut.Text += "1/(";
                }
                return;
            }

            if (TboxOut.Text.Length < 22) TboxOut.Text += content;
        }

        private void AddFunction(string funcName)
        {
            string text = TboxOut.Text;

            if (!string.IsNullOrEmpty(text))
            {
                char lastChar = text[text.Length - 1];

                if (char.IsDigit(lastChar) || lastChar == ')')
                {
                    TboxOut.Text += "*" + funcName + "(";
                    return;
                }

                if (!"+-*/^(".Contains(lastChar.ToString())) return;

                if (text.EndsWith("(")) return;
            }

            if (TboxOut.Text.Length < 32)
                TboxOut.Text += funcName + "(";
        }

        private void BtnSin_Click(object sender, RoutedEventArgs e) => AddFunction("Sin");
        private void BtnCos_Click(object sender, RoutedEventArgs e) => AddFunction("Cos");
        private void BtnTan_Click(object sender, RoutedEventArgs e) => AddFunction("Tan");
        private void BtnCot_Click(object sender, RoutedEventArgs e) => AddFunction("Cot");

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
            if (string.IsNullOrEmpty(text) || "+-*/^(".Contains(text[text.Length - 1].ToString()))
            {
                TboxOut.Text += "0.";
                return;
            }

            if (!text.Split('+', '-', '*', '/', '(', ')', '^').Last().Contains("."))
                TboxOut.Text += ".";
        }
    }
}