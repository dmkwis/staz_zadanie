using System;
using System.Collections;

namespace Intern_TicTacToe
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            //Define constants
            const int N = 4;
            const int K = 4;
            //Create Board
            Board board = new Board(N, K);
            //Create array for players and players
            Ai[] ai = new Ai[2];
            for (int i = 0; i < 2; ++i)
            {
                int playerIndex = i + 1;
                ai[i] = new Ai(playerIndex, new MinMax(playerIndex, board));
            }
            
            //turn counter
            int turn = 1;
            //keeping track which player makes a move
            byte currentPlayer = 0;


            while (!board.IsGameFinished())
            {
                Console.WriteLine("TURN " + turn);
                
                //player decides which move will he play
                Move madeMove = ai[currentPlayer].FindMove();
                
                Console.WriteLine(ai[currentPlayer] + " " + madeMove);
                
                //decided move is played on a board
                board.MakeMove(madeMove);
            
                board.Print();
                
                
                ++currentPlayer;
                currentPlayer %= 2;
                
                ++turn;
            }
            
            int winner = board.Winner();
            if (winner == 0)
            {
                Console.WriteLine("TIE");
            }
            else
            {
                Console.WriteLine(winner + " WON");
            }
            board.Print();
        }
    }
}