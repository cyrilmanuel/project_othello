using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Reversi
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Board gameBoard;
        public MainWindow()
        {
            InitializeComponent();
            gameBoard = new Board();
        }

       

        private void btnNewGame_EventClick(object sender, RoutedEventArgs e)
        {
            this.gameBoard = new Board();
        }

        private void btnCloseGame_EventClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnSaveGame_EventClick(object sender, RoutedEventArgs e)
        {
            String[] state = gameBoard.getBoard();
            foreach (String line in state) {
                Console.WriteLine(line);
            }
        }

        private void btnloadGame_EventClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
