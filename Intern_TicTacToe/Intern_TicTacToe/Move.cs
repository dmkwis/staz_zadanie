using System;

namespace Intern_TicTacToe
{
    public class Move
    {
        private int _x_coord;
        private int _y_coord;

        public Move(int x, int y)
        {
            _x_coord = x;
            _y_coord = y;
        }

        public int GetX()
        {
            return _x_coord;
        }

        public int GetY()
        {
            return _y_coord;
        }

        public override string ToString()
        {
            return "MOVE {row:" + _x_coord + ", col:" + _y_coord + "}";
        }
    }
}