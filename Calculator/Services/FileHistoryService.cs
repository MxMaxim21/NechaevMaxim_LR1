using System;
using System.Collections.Generic;
using System.IO;

namespace Calculator.Services
{
    public class FileHistoryService : Interfaces.IHistoryService
    {
        private static readonly string _filePath = System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "Calculator_history.log"
        );

        public void SaveResult(string expression, string result)
        {
            if (string.IsNullOrWhiteSpace(expression) || string.IsNullOrWhiteSpace(result)) return;
            try
            {
                File.AppendAllText(_filePath, $"{expression} = {result}{Environment.NewLine}");
            }
            catch { }
        }

        public List<string> LoadHistory()
        {
            if (!File.Exists(_filePath)) return new List<string>();
            return new List<string>(File.ReadAllLines(_filePath));
        }

        public void ClearHistory()
        {
            if (File.Exists(_filePath)) File.Delete(_filePath);
        }
    }
}