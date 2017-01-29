using OthelloConsole;
using System;
using System.Collections.Generic;

namespace Reversi { 
    public class Board : IPlayable
    {
        private int size; //board size
        private int wScore; //white score
        private int bScore; //black score
        private Dictionary<Tile,int[]> anchors; //anchor locations
        public Tile[,] board { get; set; }
        public bool isWhiteTurn { get; set; }

        public Board(int size = 8)
	    {
            this.size = size;
            anchors = new Dictionary<Tile, int[]>();
            board = new Tile[this.size, this.size];
            for (int i = 0; i < this.size; i++) {
                for (int j = 0; i < this.size; j++) {
                    board[i, j] = new Tile(i, j);
                }
            }
            // white is 1, black is 0
            board[3, 3].state = 1;
            board[4, 4].state = 1;
            board[3, 4].state = 0;
            board[4, 3].state = 0;
	    }

        public void updateScores() {
            wScore = 0;
            bScore = 0;
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; i < board.GetLength(1); j++)
                {
                    if (board[i, j].state == 0)
                        bScore++;
                    if (board[i, j].state == 1)
                        wScore++;
                }
            }

        }

        public bool IsPlayable(int column, int line, bool isWhite)
        {
            try
            {
                //check if the selected tile is empty
                if (board[line, column].state != -1) {
                    return false;
                }
                //check all 8 directions
                for (int i = -1; i <= 1; i++) {
                    for (int j = -1; j <= 1; j++) {
                        if (anchorExists(column, line, i, j, isWhite))
                            return true;
                    }
                }
                return false;
            }
            //if the targetted tile is out of the board, return invalid move
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }

        private bool anchorExists(int column, int line, int xdirection, int ydirection, bool isWhite) {
            //if direction is 0, end checking for anchors
            if (xdirection == 0 && ydirection == 0)
            {
                return false;
            }
            Tile neighbor = board[line + xdirection, column + ydirection];
            //if the neighbor has the same color as the caller, end checking for anchors in this direction
            if (neighbor.state == (isWhite ? 1 : 0))
                return false;
            int i = 0;
            //now to check in the given direction until out of board or an equivalent color to the caller is met
            while (neighbor.state != (isWhite ? 1 : 0)) {
                neighbor = board[line + (i++)*xdirection, column + (i++)*ydirection];
            }
            //if the reason that the previous loop ended is an empty tile, then return invalid
            if (neighbor.state == -1) {
                return false;
            }
            //set anchor
            anchors.Add(neighbor, new int[2] { xdirection,ydirection });
            //if all previous checks work, return true
            return true;
        }

        public bool PlayMove(int column, int line, bool isWhite)
        {
            //check if the move is playable if it is, it sets the anchors aswell
            if (!IsPlayable(column, line, isWhite))
            {
                return false;
            }
            //for each anchor
            foreach (KeyValuePair<Tile, int[]> entry in anchors) {
                int xdirection = entry.Value[0];
                int ydirection = entry.Value[1];
                Tile currentposition = board[line, column];
                //while the cursor is in between the start position and the anchor point
                while (currentposition != entry.Key) {
                    currentposition = board[line + xdirection, column + ydirection];
                    //flip the current piece
                    currentposition.state = (currentposition.state == 0 ? 1 : 0);
                }
            }
            //once we've flipped all the pieces for the move, empty the anchors for the current move
            anchors.Clear();
            updateScores();
            //announce successful move
            return true;
        }

        public Tuple<char, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            throw new NotImplementedException();
        }

        public int GetWhiteScore()
        {
            return wScore;
        }

        public int GetBlackScore()
        {
            return bScore;
        }
    }
}
