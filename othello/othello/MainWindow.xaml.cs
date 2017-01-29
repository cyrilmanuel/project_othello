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
using System.Timers;
using System.Windows.Threading;

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
            initializeGrid();
            initializeTimer();

        }


        private void initializeTimer()
        {

            DispatcherTimer updateTimer = new DispatcherTimer(DispatcherPriority.SystemIdle);
            updateTimer.Tick += new EventHandler(OnUpdateTimerTick);
            updateTimer.Interval = TimeSpan.FromMilliseconds(1000);
            updateTimer.Start();
        }

        private void OnUpdateTimerTick(object sender, EventArgs e)
        {
            if (gameBoard.isWhiteTurn)
                gameBoard.wTime++;
            else { gameBoard.bTime++; }
            updateDisplayedTime();
        }

        private void updateDisplayedTime()
        {
            player1TimePlayed.Content = string.Format("{0:00} : {1:00}", gameBoard.bTime / 60, gameBoard.bTime % 60);
            player2TimePlayed.Content = string.Format("{0:00} : {1:00}", gameBoard.wTime / 60, gameBoard.wTime % 60);
        }

        private void initializeGrid()
        {
            for (int i = 0; i < 8; i++){ 
                for (int j = 0; j < 8; j++)
                {
                    Rectangle tile = new Rectangle();
                    switch (gameBoard.board[i, j].state) {
                        case -1:
                            tile.Fill = Brushes.Green;
                            break;
                        case 0:
                            //tile.Fill = Brushes.Black;
                            tile.Fill = new ImageBrush
                            {
                                ImageSource = new BitmapImage(new Uri("../imgGame/black.png", UriKind.RelativeOrAbsolute))
                            };
                            break;
                        case 1:
                            tile.Fill = new ImageBrush
                            {
                                ImageSource = new BitmapImage(new Uri("../imgGame/white.png", UriKind.RelativeOrAbsolute))
                            };
                            break;
                    }
                    tile.MouseEnter += new MouseEventHandler(tile_MouseEnter);
                    tile.MouseLeave += new MouseEventHandler(tile_MouseLeave);
                    Grid.SetColumn(tile, j);
                    Grid.SetRow(tile, i);
                    BoardGrid.Children.Add(tile);
                }
            }
            
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
                String[] data = File.ReadAllLines(openFileDialog.FileName);
                String turn = data[0];
                String[] state = new String[data.Length - 1];
                for (int i = 1; i < data.Length; i++)
                {
                    state[i - 1] = data[i];
                }
                // gameboard avec les données
                this.gameBoard = new Board(state, (turn == "true" ? true : false));
            }
        }

        private void BoardGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var point = Mouse.GetPosition(BoardGrid);

            int row = 0;
            int col = 0;
            double accumulatedHeight = 0.0;
            double accumulatedWidth = 0.0;
        
            // calc row mouse was over
            foreach (var rowDefinition in BoardGrid.RowDefinitions)
            {
                accumulatedHeight += rowDefinition.ActualHeight;
                if (accumulatedHeight >= point.Y)
                    break;
                row++;
            }

            // calc col mouse was over
            foreach (var columnDefinition in BoardGrid.ColumnDefinitions)
            {
                accumulatedWidth += columnDefinition.ActualWidth;
                if (accumulatedWidth >= point.X)
                    break;
                col++;
            }

            gameBoard.isWhiteTurn = !gameBoard.isWhiteTurn;

            // row and col now correspond Grid's RowDefinition and ColumnDefinition mouse was 
            // over when clicked!
            
        }

        private void tile_MouseEnter(object sender, MouseEventArgs e)
        {
            var element = (UIElement)e.Source;
            int c = Grid.GetColumn(element);
            int r = Grid.GetRow(element);

            if (gameBoard.IsPlayable(c, r, gameBoard.isWhiteTurn))
            {
                if (gameBoard.isWhiteTurn)
                {
                    ((Rectangle)sender).Fill = new ImageBrush
                    {
                        ImageSource = new BitmapImage(new Uri("../imgGame/black.png", UriKind.RelativeOrAbsolute))
                    };
                }
                else {
                    ((Rectangle)sender).Fill = new ImageBrush
                    {
                        ImageSource = new BitmapImage(new Uri("../imgGame/white.png", UriKind.RelativeOrAbsolute))
                    };
                }  
            }
        }

        private void tile_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Rectangle)sender).Fill = Brushes.Green;
        }
    }
}
