using OthelloConsole;
using System;
using System.Collections.Generic;

namespace Reversi { 
    public class Board : IPlayable
    {
        private int size; //board size
        private int wScore; //white score
        private int bScore; //black score
        public int wTime { get; set; }
        public int bTime { get; set; }
        private Dictionary<Tile,int[]> anchors; //anchor locations
        public Tile[,] board { get; set; }
        public bool isWhiteTurn { get; set; }

        public Board(int size = 8)
	    {
            this.size = size;
            wTime = 0;
            bTime = 0;
            anchors = new Dictionary<Tile, int[]>();
            board = new Tile[this.size, this.size];
            isWhiteTurn = false;
            for (int i = 0; i < this.size; i++) {
                for (int j = 0; j < this.size; j++) {
                    board[i, j] = new Tile(i, j);
                }
            }
            // white is 1, black is 0
            board[3, 3].state = 1;
            board[4, 4].state = 1;
            board[3, 4].state = 0;
            board[4, 3].state = 0;
            updateScores();
	    }

        public Board(String[] state, bool isWhiteTurn) {
            this.isWhiteTurn = isWhiteTurn;
            anchors = new Dictionary<Tile, int[]>();
            this.size = state.Length;
            board = new Tile[this.size, this.size];
            for (int j = 0; j < state.Length; j++) {
                String[] members = state[j].Split(',');
                for (int i = 0; i < members.Length; i++) { 
                    board[i, j] = new Tile(i, j);
                    board[i, j].state = Int32.Parse(members[i]);
                }
            }
            wTime = 0;
            bTime = 0;
            updateScores();
        }

        public void updateScores() {
            wScore = 0;
            bScore = 0;
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
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
            anchors.Clear();
            //check if the selected tile is empty
            if (board[line, column].state != -1) {
                return false;
            }
            //check all 8 directions
            for (int i = -1; i <= 1; i++) {
                for (int j = -1; j <= 1; j++) {
                    if (anchorExists(column, line, i, j, isWhite))
                        continue;
                }
            }
            if (anchors.Count > 0)
            {
                foreach (KeyValuePair<Tile, int[]> entry in anchors)
                {
                    Console.WriteLine("anchors: col-{0} row-{1} dirx-{2} diry-{3}", entry.Key.column, entry.Key.row, entry.Value[0], entry.Value[1]);
                }
                
                return true;
            }
            return false;
        }

        private bool anchorExists(int column, int line, int xdirection, int ydirection, bool isWhite) {
            //if direction is 0, end checking for anchors
            if (xdirection == 0 && ydirection == 0)
            {
                return false;
            }
            int nCoordx = line + xdirection;
            int nCoordy = column + ydirection;
            Tile neighbor;
            if (nCoordx < 0 || nCoordy < 0 || nCoordy > 7 || nCoordx > 7)
                return false;
            else {
                neighbor = board[nCoordx, nCoordy];
                //if the neighbor has the same color as the caller, end checking for anchors in this direction
                if (neighbor.state != (isWhite ? 1 : 0))
                    return false;
                //now to check in the given direction until out of board or an equivalent color to the caller is met
                while (neighbor.state == (isWhite ? 1 : 0)) {
                    nCoordx += xdirection;
                    nCoordy += ydirection;
                    if (nCoordx < 0 || nCoordy < 0 || nCoordy > 7 || nCoordx > 7)
                        return false;
                    else
                        neighbor = board[nCoordx, nCoordy];
                }
                //if the reason that the previous loop ended is an empty tile, then return invalid
                if (neighbor.state == -1) {
                    anchors.Clear();
                    return false;
                }
                //set anchor
                anchors.Add(neighbor, new int[2] { xdirection,ydirection });
                
                //if all previous checks work, return true
                return true;
            }
        }

        public bool PlayMove(int column, int line, bool isWhite)
        {
            //check if the move is playable if it is, it sets the anchors aswell
            if (!IsPlayable(column, line, isWhite))
            {
                return false;
            }
            Console.WriteLine("play caller: col: {0} lin: {1}", column, line);
            //add new tile
            board[line, column].state = (isWhite ? 0 : 1);
            //for each anchor
            foreach (KeyValuePair<Tile, int[]> entry in anchors) {
                int xdirection = entry.Value[0];
                int ydirection = entry.Value[1];
                int nCoordx = line + xdirection;
                int nCoordy = column + ydirection;
                Tile currentposition = board[line, column];
                
                //while the cursor is in between the start position and the anchor point
                while (currentposition != entry.Key) {
                    //flip the current piece
                    board[nCoordx, nCoordy].state = (currentposition.state == 0 ? 0 : 1);
                    nCoordx += xdirection;
                    nCoordy += ydirection;
                    currentposition = board[nCoordx, nCoordy];
                        
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

        public String[] getBoard() {
            String[] state = new String[size+1];
            state[0] = isWhiteTurn ? "true" : "false";
            for (int i = 1; i < size+1; i++)
            {
                for (int j = 1; j < size+1; j++)
                {
                    if (j == 8)
                        state[i] += board[i-1, j-1].state.ToString();
                    else
                        state[i] += board[i-1, j-1].state.ToString() + ",";
                }
            }
            return state;
        }
    }
}
