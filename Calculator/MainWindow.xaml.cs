using System.Windows;
using Calculator.Views;

namespace Calculator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Views.Normal normalPage = new Views.Normal();
        private Views.Engineering engPage = new Views.Engineering();

        public MainWindow()
        {
            InitializeComponent();
            FrameMain.Navigate(new Normal());
        }

        private void BtnSwap_Click(object sender, RoutedEventArgs e)
        {
            if (TbName.Text == "Обычный")
            {
                FrameMain.Navigate(engPage);
                TbName.Text = "Инженерный";
                this.Width = 500; 
            }
            else
            {
                FrameMain.Navigate(normalPage);
                TbName.Text = "Обычный";
                this.Width = 400;
            }
        }

        private void BtnHistory_Click(object sender, RoutedEventArgs e)
        {
            FrameMain.Navigate(new Views.History());
        }
    }
}
