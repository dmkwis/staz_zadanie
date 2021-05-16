using System;
using System.Collections;
using System.Collections.Generic;

namespace Intern_TicTacToe
{
    public class Board
    {
        private readonly int _n;
        private readonly int _size;
        private readonly int _k;
        private int _freeFields;
        private int _currentHash;
        private bool _turn;
        private readonly byte[,] _board;
        private readonly int[,] _powerOfThree;
        private readonly Move[,] _allMoves;
        private readonly Stack<Move> _lastMoves;

        public Board(int n)
        {
            _n = n;
            _board = new byte[n, n];
        }

        public Board(int n, int k)
        {
            //Set up variables
            _n = n;
            _size = n * n;
            _k = k;
            _freeFields = n * n;
            _turn = false;
            _currentHash = int.MinValue;
            _board = new byte[n, n];
            _lastMoves = new Stack<Move>();

            //precalculate powers of three -> for fast hashing,
            //for N=4 all hashes are contained in int
            _powerOfThree = new int[n, n];
            int pot = 1;
            for (int x = 0; x < _n; ++x)
            {
                for (int y = 0; y < _n; ++y)
                {
                    _powerOfThree[x, y] = pot;
                    pot *= 3;
                }
            }

            _allMoves = new Move[n, n];
            for (int x = 0; x < _n; ++x)
            {
                for (int y = 0; y < _n; ++y)
                {
                    _allMoves[x, y] = new Move(x, y);
                }
            }
        }

        public int GetSize()
        {
            return _size;
        }

        public int FreeFields()
        {
            return _freeFields;
        }

        public bool IsGameFinished()
        {
            return _freeFields == 0 || Winner() != 0;
        }

        public void Print()
        {
            for (int x = 0; x < _n; ++x)
            {
                for (int y = 0; y < _n; ++y)
                {
                    if (_board[x, y] == 0)
                    {
                        Console.Write(". ");
                    }
                    else if (_board[x, y] == 1)
                    {
                        Console.Write("O ");
                    }
                    else if (_board[x, y] == 2)
                    {
                        Console.Write("X ");
                    }
                }

                Console.Write("\n");
            }
        }

        //Function checks if last performed move was the winning one
        //Checking only last move allows O(K) computation instead higher complexity
        public byte Winner()
        {
            Move lastMove;
            try
            {
                lastMove = _lastMoves.Peek();
            }
            catch (InvalidOperationException)
            {
                return 0;
            }

            int x = lastMove.GetX();
            int y = lastMove.GetY();
            byte possibleWinner = _board[x, y];
            int howMany = 0;

            for (int currY = Math.Max(0, y - _k + 1); currY < Math.Min(_n, y + _k); ++currY)
            {
                if (_board[x, currY] == possibleWinner)
                {
                    ++howMany;
                }
                else
                {
                    howMany = 0;
                }

                if (howMany == _k)
                {
                    //Console.WriteLine("Y WIN");
                    return possibleWinner;
                }
            }

            howMany = 0;
            for (int currX = Math.Max(0, x - _k + 1); currX < Math.Min(_n, x + _k); ++currX)
            {
                if (_board[currX, y] == possibleWinner)
                {
                    ++howMany;
                }
                else
                {
                    howMany = 0;
                }

                if (howMany == _k)
                {
                    return possibleWinner;
                }
            }


            //Diagonal 1 check
            howMany = 0;
            int xDist = Math.Max(0, x - _k + 1);
            int yDist = Math.Max(0, y - _k + 1);
            int xDiff = x - xDist;
            int yDiff = y - yDist;
            int minDiff = Math.Min(xDiff, yDiff);
            int xDiag = x - minDiff;
            int yDiag = y - minDiff;
            for (; xDiag < _n && yDiag < _n; ++xDiag, ++yDiag)
            {
                if (_board[xDiag, yDiag] == possibleWinner)
                {
                    ++howMany;
                }
                else
                {
                    howMany = 0;
                }

                if (howMany == _k)
                {
                    return possibleWinner;
                }
            }

            //Diagonal 2 check
            howMany = 0;
            //xDist = 
            xDiff = (x + _k - 1 >= _n) ? _n - 1 - x : _k - 1;
            minDiff = Math.Min(xDiff, yDiff);
            xDiag = x + minDiff;
            yDiag = y - minDiff;
            for (; xDiag >= 0 && yDiag < _n; --xDiag, ++yDiag)
            {
                if (_board[xDiag, yDiag] == possibleWinner)
                {
                    ++howMany;
                }
                else
                {
                    howMany = 0;
                }

                if (howMany == _k)
                {
                    return possibleWinner;
                }
            }

            return 0;
        }

        private bool InRange(int coord)
        {
            return coord >= 0 && coord < _n;
        }

        public bool IsMovePossible(Move move)
        {
            int x = move.GetX();
            int y = move.GetY();
            return InRange(x) && InRange(y) && _board[x, y] == 0;
        }


        public void MakeMove(Move move)
        {
            if (!IsMovePossible(move))
            {
                return;
            }

            int x = move.GetX();
            int y = move.GetY();

            byte player = (byte) ((_turn) ? 2 : 1);
            //recalculation of hash when board changes
            _currentHash += player * _powerOfThree[x, y];
            _board[x, y] = player;
            _lastMoves.Push(move);
            _turn = !_turn;
            --_freeFields;
        }

        private static Random rng = new Random();

        void shuffle(ArrayList al)
        {
            int n = al.Count;
            while (n > 1)
            {
                int newInd = rng.Next(n--);
                Move tmp = (Move) al[n];
                al[n] = al[newInd];
                al[newInd] = tmp;
            }
        }

        public ArrayList GetPossibleMoves()
        {
            ArrayList possibleMoves = new ArrayList();
            for (int x = 0; x < _n; ++x)
            {
                for (int y = 0; y < _n; ++y)
                {
                    if (_board[x, y] == 0)
                    {
                        possibleMoves.Add(_allMoves[x, y]);
                    }
                }
            }
            //shuffling moves to make it a bit more human-like (no asymptotic increase)
            shuffle(possibleMoves);
            return possibleMoves;
        }

        public void RevertMove()
        {
            Move move;
            try
            {
                move = _lastMoves.Pop();
            }
            catch (InvalidOperationException)
            {
                return;
            }

            int x = move.GetX();
            int y = move.GetY();
            int player = _board[x, y];
            //recalculating hash when board changes
            _currentHash -= player * _powerOfThree[x, y];
            _board[x, y] = 0;
            ++_freeFields;
            _turn = !_turn;
        }

        //hash is calculated during changes to the board -> 
        //it makes the complexity smaller

        public override int GetHashCode()
        {
            return _currentHash;
        }
    }
}