﻿using System;
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

namespace Reversi
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Board gameBoard;
        Timer[] playertimers;
        public Rectangle[,] tabRect;
        public MainWindow()
        {
            InitializeComponent();
            gameBoard = new Board();
            initializeGrid();
            initializeTimers();

        }
        private void initializeTimers()
        {
            playertimers = new Timer[2] { new Timer(1000), new Timer(1000) };
            for (int i = 0; i < 2; i++) {
                playertimers[i].Elapsed += OnTimedEvent;
            }
        }

        private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {

            Console.WriteLine("The Elapsed event was raised by {0} at {1:mm:ss}", source.ToString(),e.SignalTime);
        }

        private void initializeGrid()
        {
            tabRect = new Rectangle[8, 8];
            for (int i = 0; i < 8; i++){ 
                for (int j = 0; j < 8; j++)
                {
                    Rectangle tile = new Rectangle();
                    tile.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    tile.MouseEnter += new EventHandler(tile_MouseEnter);
                    tabRect[i, j] = tile;
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
            MessageBox.Show(string.Format("Grid clicked at column {0}, row {1}", point.X, point.Y));

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

            // row and col now correspond Grid's RowDefinition and ColumnDefinition mouse was 
            // over when clicked!
            if (gameBoard.isWhiteTurn)
            {
                playertimers[0].Stop();
                playertimers[1].Start();
                gameBoard.isWhiteTurn = false;
            }
            else
            {
                playertimers[1].Stop();
                playertimers[0].Start();
                gameBoard.isWhiteTurn = true;
            }
        }

        private void tile_MouseEnter(object sender, MouseEventArgs e)
        {

            var element = (UIElement)e.Source;

            int c = Grid.GetColumn(element);
            int r = Grid.GetRow(element);

            MessageBox.Show(string.Format("Grid clicked at column {0}, row {1}", r, c));

        }
    }
}
