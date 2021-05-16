namespace Intern_TicTacToe
{
    public class Ai
    {
        private readonly IMoveStrategy _strategy;
        private readonly int _playerId;
        public Ai(int playerId, IMoveStrategy strategy)
        {
            _playerId = playerId;
            _strategy = strategy;
        }


        public Move FindMove()
        {
            //plays the move given by strategy
            return _strategy.GetMove();
        }


        public override string ToString()
        {
            return "AI: " + _playerId;
        }
    }
}