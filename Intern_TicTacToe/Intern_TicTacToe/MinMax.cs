using System;
using System.Collections.Generic;
using System.Collections;

namespace Intern_TicTacToe
{
    public class MinMax : IMoveStrategy
    {
        private readonly int _playerId;
        private readonly Board _board;
        private readonly Dictionary<Board, Move> _positionNextMove;
        private readonly Dictionary<Board, int> _positionEvaluationMaximazing;
        private readonly Dictionary<Board, int> _positionEvaluationMinimazing;
        public MinMax(int playerId, Board startingBoard)
        {
            _playerId = playerId;
            _board = startingBoard;
            _positionNextMove = new Dictionary<Board, Move>();
            _positionEvaluationMaximazing = new Dictionary<Board, int>();
            _positionEvaluationMinimazing = new Dictionary<Board, int>();
        }


        private int Search(bool player)
        {
            if (player && _positionEvaluationMaximazing.ContainsKey(_board))
            {
                return _positionEvaluationMaximazing[_board];
            }
            else if(!player && _positionEvaluationMinimazing.ContainsKey(_board))
            {
                return _positionEvaluationMinimazing[_board];
            }
            //check if the position is winning
            //if so return its evaluation
            int whoJustWon = _board.Winner();
            if (whoJustWon != 0)
            {
                //rescaling by big scalar so that we can easily calculate fastest win
                int boardEvaluation = ((whoJustWon == _playerId) ? 1 : -1) * _board.GetSize() * 2;
                _positionEvaluationMinimazing.Add(_board, boardEvaluation);
                _positionEvaluationMaximazing.Add(_board, boardEvaluation);
                return boardEvaluation;
            }
            
            //If there are no more free fields and no one has a winning position then its a tie
            if (_board.FreeFields() == 0)
            {
                int boardEvaluation = 0;
                _positionEvaluationMinimazing.Add(_board, boardEvaluation); 
                _positionEvaluationMaximazing.Add(_board, boardEvaluation);
                return boardEvaluation;
            }

            //min-max algorithm here:
            int currentBestValue = player? int.MinValue : int.MaxValue;
            ArrayList possibleMoves = _board.GetPossibleMoves();
            if (player)
            {
                //maximize for yourself
                Move currentBestMove = null;
                foreach (Move move in possibleMoves)
                {
                    _board.MakeMove(move);
                    int thisEval = Search(!player);
                    
                    if (thisEval != 0)
                    {
                        //decrementing value - punishment for making additional move
                        // will never be smaller than 0 because of scaling default win position by big scalar (see above)
                        --thisEval;
                    }
                    
                    if (thisEval > currentBestValue)
                    {
                        currentBestValue = thisEval;
                        currentBestMove = move;
                    }
                    _board.RevertMove();
                }
                _positionNextMove.Add(_board, currentBestMove); 
                _positionEvaluationMaximazing.Add(_board, currentBestValue);
                return currentBestValue;
            }
            else
            {
                //minimize like opponent would
                foreach (Move move in possibleMoves)
                {
                    _board.MakeMove(move);
                    int thisEval = Search(!player);
                    currentBestValue = Math.Min(thisEval, currentBestValue);
                    _board.RevertMove();
                }
                _positionEvaluationMinimazing.Add(_board, currentBestValue);
                return currentBestValue;
            }
        }
        
        //search for the best move and return it
        public Move GetMove()
        {
            if (!_positionEvaluationMaximazing.ContainsKey(_board))
            {
                Search(true);
            }
            return _positionNextMove[_board];
        }

    }
}