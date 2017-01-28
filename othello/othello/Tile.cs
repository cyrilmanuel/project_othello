using System;

namespace Reversi { 
    public class Tile
    {
        private int column;
        private int row;
        public int state { get; set; }
        public Tile(int col, int row)
	    {
            this.column = col;
            this.row = row;
            state = -1;
	    }
        
    }
}
