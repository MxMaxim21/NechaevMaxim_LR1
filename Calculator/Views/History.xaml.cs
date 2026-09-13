using Calculator.Services;
using Calculator.Services.Interfaces;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Calculator.Views
{
    public partial class History : Page
    {
        private readonly IHistoryService _historyService = new FileHistoryService();

        public History()
        {
            InitializeComponent();
            RefreshList();
        }

        private void RefreshList()
        {
            historyList.ItemsSource = _historyService.LoadHistory();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            _historyService.ClearHistory();
            RefreshList();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack) NavigationService.GoBack();
        }
    }
}
