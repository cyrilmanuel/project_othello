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
            saveFileDialog.FileName = "partieOthello.txt";
            saveFileDialog.Filter = "Text File | *.txt";
            if (saveFileDialog.ShowDialog() == true)
            {
                StreamWriter writer = new StreamWriter(saveFileDialog.OpenFile());


                String[] state = gameBoard.getBoard();
                foreach (String line in state)
                {
                    writer.WriteLine(line);
                }

                writer.Dispose();
                writer.Close();
            }

           
            
        }

        private void btnloadGame_EventClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                String[] state = File.ReadAllLines(openFileDialog.FileName);

                // gameboard avec les données
                //this.gameBoard = new Board(state);
            }
        }
    }
}
