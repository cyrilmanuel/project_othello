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
using System.IO;
using Microsoft.Win32;

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
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                String[] state = gameBoard.getBoard();
                foreach (String line in state)
                {
                    Console.WriteLine(line);
                }
            }
        }

        private void btnloadGame_EventClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                String state = File.ReadAllText(openFileDialog.FileName);
                StateGame st = JsonConvert.DeserializeObject<StateGame>(state);
                // Mise à jour du board.
                gb.setBoard(st.getCaseBoard());
                // Mise à jour du joueur
                this.gb.activePlayer = st.ActivePlayer;
                gb.majScores();
                MAJ();
                Console.WriteLine(state);

            }
        }
    }
}
