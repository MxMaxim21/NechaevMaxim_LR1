using System;
using System.Linq;
using System.Text.RegularExpressions;
using Calculator.Services.Interfaces;

namespace Calculator.Services
{
    public class CalculationService : ICalculationService
    {
        public string Calculate(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression)) return string.Empty;

            try
            {
                string exprStr = expression.Replace(",", ".");

                int openBrackets = exprStr.Count(f => f == '(');
                int closeBrackets = exprStr.Count(f => f == ')');
                while (openBrackets > closeBrackets)
                {
                    exprStr += ")";
                    closeBrackets++;
                }

                string junk = "+-*/^.√";
                while (exprStr.Length > 0 && junk.Contains(exprStr[exprStr.Length - 1].ToString()))
                {
                    exprStr = exprStr.Substring(0, exprStr.Length - 1);
                }

                exprStr = Regex.Replace(exprStr, @"(\d+\.?\d*)\s*([\+\-])\s*(\d+\.?\d*)%", m => $"{m.Groups[1].Value}{m.Groups[2].Value}({m.Groups[1].Value}*{m.Groups[3].Value}/100)");
                exprStr = Regex.Replace(exprStr, @"(\d+\.?\d*)%", "($1/100.0)");
                exprStr = Regex.Replace(exprStr, @"√(\d+\.?\d*|\([^)]*\))", "Sqrt($1)");
                exprStr = Regex.Replace(exprStr, @"(\d+\.?\d*|\([^)]*\))\^(\d+\.?\d*|\([^)]*\))", "Pow($1, $2)");
                string safeExpression = Regex.Replace(exprStr, @"(?<![\d.])\b\d+\b(?![\d.])", "$0.0");

                var expr = new NCalc.Expression(safeExpression);
                var evalResult = expr.Evaluate();
                double doubleResult = Convert.ToDouble(evalResult, System.Globalization.CultureInfo.InvariantCulture);

                if (double.IsInfinity(doubleResult) || double.IsNaN(doubleResult))
                    return "Деление на 0";

                return doubleResult.ToString("G15", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                return "Ошибка";
            }
        }
    }
}