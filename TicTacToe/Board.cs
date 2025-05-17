public class Board
{
    private const char _emptyCell = ' ';
    private const int _side = 3;
    private char[] _matrix = Enumerable.Repeat(_emptyCell, _side * _side).ToArray();
    /*
     * 0 1 2
     * 3 4 5
     * 6 7 8
    */
    private static readonly int[][] _winPatterns =
    {
        [ 0, 1, 2 ],
        [ 3, 4, 5 ],
        [ 6, 7, 8 ],
        [ 0, 4, 8 ],
        [ 2, 4, 6 ],
        [ 0, 3, 6 ],
        [ 1, 4, 7 ],
        [ 2, 5, 8 ],
    };

    public void Print()
    {
        for (int i = 0; i < _side; i++)
        {
            for (int j = 0; j < _side; j++)
            {
                var idx = i * _side + j;
                char cell = _matrix[idx];
                Console.Write((cell == _emptyCell ? idx.ToString() : cell) + " ");
            }
            Console.WriteLine();
        }
    }

    public GameStatus GetGameStatus()
    {
        if (IsWin()) return GameStatus.WIN;
        if (HasEmpty()) return GameStatus.STILL;
        return GameStatus.DRAW;
    }

    public bool TrySet(int idx, char symbol)
    {
        if (idx < 0 || idx >= _matrix.Length)
        {
            Console.WriteLine("this is not a valid cell!");
            return false;
        }
        if (_matrix[idx] != ' ')
        {
            Console.WriteLine("this cell is already choosen!");
            return false;
        }
        _matrix[idx] = symbol;
        return true;
    }

    private bool IsWin() =>
        _winPatterns.Any(f => _matrix[f[0]] != _emptyCell &&
                _matrix[f[0]] == _matrix[f[1]] &&
                _matrix[f[1]] == _matrix[f[2]]);

    private bool HasEmpty() =>
        _matrix.Any(f => f == _emptyCell);
}
