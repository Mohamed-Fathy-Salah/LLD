public class TicTacToe(Player player1, Player player2, Board board)
{
    private Player[] _players { get; } = [player1, player2];
    private Board _board { get; } = board;
    private int _turn = 0;

    public void Run()
    {
        while (_board.GetGameStatus() == GameStatus.STILL)
        {
            _board.Print();
            int play = -1;
            do
            {
                play = _players[_turn].Play();
            } while (!_board.TrySet(play, _players[_turn].Symbol));
            _turn ^= 1;
        }

        _board.Print();
        Console.WriteLine(_board.GetGameStatus() == GameStatus.WIN
            ? $"{_players[_turn ^ 1]} wins!"
            : "Draw!");
    }
}
