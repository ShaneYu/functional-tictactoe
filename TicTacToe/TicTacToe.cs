using System;

namespace TicTacToe
{
    public static class TicTacToe
    {
        public static void Run(Action<string> write, Func<string> getInput, Action clear = null)
        {
            char[,] board = {
                {'1', '2', '3'},
                {'4', '5', '6'},
                {'7', '8', '9'}
            };

            char? winner = null;
            int playerTurn = 1;

            while (!winner.HasValue)
            {
                Print(write, board, playerTurn, clear);

                if (PlayMove(board, getInput, playerTurn))
                {
                    winner = CheckForWinner(board);
                    playerTurn = playerTurn == 1 ? 2 : 1;
                }
            }

            AnnounceWinner(write, board, winner.Value, clear);
        }

        public static void PrintBoard(Action<string> write, char[,] board)
        {
            for (int y = 0; y < board.GetLength(0); y++)
            {
                if (y > 0)
                    write($"{new string('-', 11)}\n");

                for (int x = 0; x < board.GetLength(1); x++)
                {
                    if (x > 0)
                        write("|");

                    write($" {board[y, x]} ");
                }

                write("\n");
            }
        }

        public static void Print(Action<string> write, char[,] board, int? playerTurn, Action clear = null)
        {
            clear?.Invoke();

            write("Tic Tac Toe\n\n");
            PrintBoard(write, board);

            if (playerTurn.HasValue)
                write($"\nTurn: Player {playerTurn}\n");
        }

        public static bool PlayMove(char[,] board, Func<string> getInput, int playerTurn)
        {
            int num;
            var input = getInput.Invoke();

            if (!string.IsNullOrWhiteSpace(input) && int.TryParse(input, out num) && num >= 0 && num <= 9)
            {
                for (int y = 0; y < board.GetLength(0); y++)
                {
                    for (int x = 0; x < board.GetLength(1); x++)
                    {
                        if (board[y, x] == input[0])
                        {
                            board[y, x] = playerTurn == 1 ? 'X' : 'O';
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static char? CheckForWinner(char[,] board)
        {
            char? result;

            // Check Rows
            result = IsThreeInARow(board, 0, 0, 0, 1);
            if (result.HasValue) return result.Value;

            // Check Columns
            result = IsThreeInARow(board, 0, 0, 1, 0);
            if (result.HasValue) return result.Value;

            // Check Diagonal TL -> BR
            result = IsThreeInARow(board, 0, 0, 1, 1);
            if (result.HasValue) return result.Value;

            // Check Diagonal TR -> BL
            result = IsThreeInARow(board, 2, 0, -1, 1);
            if (result.HasValue) return result.Value;

            int spacesFilled = 0;
            for (int y = 0; y < board.GetLength(0); y++)
            {
                for (int x = 0; x < board.GetLength(1); x++)
                {
                    if (board[y, x] == 'X' || board[y, x] == 'O')
                    {
                        spacesFilled++;
                    }
                }
            }

            if (spacesFilled == 9)
            {
                return '-';
            }

            return null;
        }

        public static char? IsThreeInARow(char[,] board, int startX, int startY, int dX, int dY)
        {
            char firstChar = board[startY, startX];
            if (char.IsWhiteSpace(firstChar)) return null;

            for (var i = 0; i < 3; i++)
            {
                if (board[startY + dY * i, startX + dX * i] != firstChar) return null;
            }

            return firstChar;
        }

        public static void AnnounceWinner(Action<string> write, char[,] board, char winner, Action clear = null)
        {
            clear?.Invoke();
            
            Print(write, board, null, clear);
            write($"\nWinner is: {(winner == '-' ? "No one" : $"Player {(winner == 'X' ? 1 : 2)}")}.");
        }
    }
}