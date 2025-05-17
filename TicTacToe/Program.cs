var player1 = new HumanPlayer("player1", 'X');
var player2 = new HumanPlayer("player1", 'O');

var board = new Board();

var game = new TicTacToe(player1, player2, board);
game.Run();
