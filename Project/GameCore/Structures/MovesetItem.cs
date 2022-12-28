namespace ProjectOrigin
{
    /// <summary>Stores a move and a level that the move is learned at.</summary>
    public class MovesetItem : BasicMove
    {
        public int LearnLevel { get; set; } = 0;
        public BasicMove Move { get; set; } = null;

        public MovesetItem() : base()
        {

        }

        public MovesetItem(int level, BasicMove move)
        {
            LearnLevel = level;
            Move = move;
        }

    }
}