using System;

namespace Reversi { 
    public class Tile
    {
        public int column { get; }
        public int row { get; }
        public int state { get; set; }
        public Tile(int col, int row)
	    {
            this.column = col;
            this.row = row;
            state = -1;
	    }

        public static bool operator ==(Tile x, Tile y)
        {
            return (x.row == y.row && x.column == y.column);
        }
        public static bool operator !=(Tile x, Tile y)
        {
            return (x.row != y.row || x.column != y.column);
        }

    }
}
