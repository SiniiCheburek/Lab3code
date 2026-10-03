using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp4
{
    public enum CellState
    {
        Empty,
        X,
        O
    }

    public enum GameResult
    {
        Continue,
        XWins,
        OWins,
        Draw
    }

    public enum Difficulty
    {
        Random = 1,
        BlockPlayer = 2,
        Smart = 3
    }

    public class TicTacToeGame
    {
        public const int BoardSize = 3;

        private readonly CellState[,] board;

        private readonly Random random = new Random();

        public TicTacToeGame()
        {
            board = new CellState[BoardSize, BoardSize];
        }

        public CellState[,] Board => board;

        public void NewGame()
        {
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    board[row, col] = CellState.Empty;
                }
            }
        }

        public bool MakeMove(int row, int col, CellState player)
        {
            
            if (row < 0 || row >= BoardSize || col < 0 || col >= BoardSize)
            {
                return false;
            }

            if (board[row, col] != CellState.Empty)
            {
                return false;
            }

            board[row, col] = player;

            return true;
        }

        public List<(int Row, int Col)> GetAvailableMoves()
        {
            List<(int Row, int Col)> moves = new List<(int Row, int Col)>();

            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    if (board[row, col] == CellState.Empty)
                    {
                        moves.Add((row, col));
                    }
                }
            }

            return moves;
        }

        public CellState CheckWinner()
        {
            for (int row = 0; row < BoardSize; row++)
            {
                if (board[row, 0] != CellState.Empty)
                {
                    bool win = true;

                    for (int col = 1; col < BoardSize; col++)
                    {
                        if (board[row, col] != board[row, 0])
                        {
                            win = false;
                            break;
                        }
                    }

                    if (win)
                        return board[row, 0];
                }
            }

            for (int col = 0; col < BoardSize; col++)
            {
                if (board[0, col] != CellState.Empty)
                {
                    bool win = true;

                    for (int row = 1; row < BoardSize; row++)
                    {
                        if (board[row, col] != board[0, col])
                        {
                            win = false;
                            break;
                        }
                    }

                    if (win)
                        return board[0, col];
                }
            }

            if (board[0, 0] != CellState.Empty)
            {
                bool win = true;

                for (int i = 1; i < BoardSize; i++)
                {
                    if (board[i, i] != board[0, 0])
                    {
                        win = false;
                        break;
                    }
                }

                if (win)
                    return board[0, 0];
            }

            if (board[0, BoardSize - 1] != CellState.Empty)
            {
                bool win = true;

                for (int i = 1; i < BoardSize; i++)
                {
                    if (board[i, BoardSize - 1 - i] != board[0, BoardSize - 1])
                    {
                        win = false;
                        break;
                    }
                }

                if (win)
                    return board[0, BoardSize - 1];
            }

            return CellState.Empty;
        }


        public bool IsDraw()
        {
            if (CheckWinner() != CellState.Empty)
                return false;

            return GetAvailableMoves().Count == 0;
        }

        public (int Row, int Col)? ChooseComputerMove(Difficulty difficulty)
        {
            List<(int Row, int Col)> moves = GetAvailableMoves();

            if (moves.Count == 0)
                return null;

            if (difficulty == Difficulty.Random)
            {
                return moves[random.Next(moves.Count)];
            }

            if (difficulty == Difficulty.BlockPlayer)
            {
                var blockingMove = FindWinningMove(CellState.X);

                if (blockingMove.HasValue)
                    return blockingMove.Value;

                return moves[random.Next(moves.Count)];
            }


            var winningMove = FindWinningMove(CellState.O);

            if (winningMove.HasValue)
                return winningMove.Value;

            var playerWinningMove = FindWinningMove(CellState.X);

            if (playerWinningMove.HasValue)
                return playerWinningMove.Value;

            return moves[random.Next(moves.Count)];
        }


        private (int Row, int Col)? FindWinningMove(CellState player)
        {
            List<(int Row, int Col)> moves = GetAvailableMoves();

            foreach (var move in moves)
            {
                board[move.Row, move.Col] = player;

                if (CheckWinner() == player)
                {
                    board[move.Row, move.Col] = CellState.Empty;
                    return move;
                }

                board[move.Row, move.Col] = CellState.Empty;
            }

            return null;
        }

        public GameResult GetResult()
        {
            CellState winner = CheckWinner();

            if (winner == CellState.X)
                return GameResult.XWins;

            if (winner == CellState.O)
                return GameResult.OWins;

            if (IsDraw())
                return GameResult.Draw;

            return GameResult.Continue;
        }
    }
}
