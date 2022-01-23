using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    internal class SudokuBoard
    {
        private bool isValidBoardData = default(bool);
        private short[,] board = new short[9, 9];
        private ILogger logger;

        public SudokuBoard(SudokuData data, ILogger logger)
        {
            this.logger = logger;

            isValidBoardData = IsValidBoardData(data.Board);

            BuildBoard(data);
        }

        private bool IsValidBoardData(short[] items)
        {
            if (items == null || items.Length == 0) return default(bool);
            if (items.Length != 81) return default(bool);

            var hasNoRepeats = true;
            var i = 0;
            var splits = from item in items
                         group item by i++ % 9 into part
                         select part.ToList();
            splits.ToList().ForEach(s =>
            {
                hasNoRepeats = hasNoRepeats && HasNoRepeats(s);
            });
            for (i = 0; i < 9; i++)
            {
                var s = items.Skip(i * 9).Take(9).ToList();
                hasNoRepeats = hasNoRepeats && HasNoRepeats(s);
            }

            return hasNoRepeats;
        }

        private bool HasNoRepeats(List<short> blocks)
        {
            var distincts = new List<short>();
            foreach (var item in blocks)
            {
                if (item == 0) continue;
                if (item > 9) return false;
                if (item < 0) return false;
                if (distincts.Contains(item)) return false;
                distincts.Add(item);
            }
            return true;
        }

        internal SudokuData SolveSudoku(SudokuData sudokuData)
        {
            if (!isValidBoardData)
            {
                sudokuData.Solvable = isValidBoardData;
                return sudokuData;
            }

            sudokuData.Solvable = SolveBoard(); // solver algo
            sudokuData.Board = FlattenBoard();
            sudokuData.DateTimeStamp = DateTime.UtcNow;
           
            return sudokuData;
        }

        private bool SolveBoard()
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] == 0)
                    {
                        for (short d = 1; d <= 9; d++)
                        {
                            if (IsValidBoard(i, j, d))
                            {
                                board[i, j] = d;
                                if (SolveBoard()) return true; // valid entry
                                board[i, j] = 0; // reset board entry
                            }
                        }
                        return false;
                    }
                }
            }
            return true;
        }

        private bool IsValidBoard(int row, int col, short d)
        {
            for (int i = 0; i < 9; i++)
            {
                if (board[i, col] != 0 && board[i, col] == d) return false;
                if (board[row, i] != 0 && board[row, i] == d) return false;
                // blockwise check
                if (board[3 * (row / 3) + i / 3, 3 * (col / 3) + i % 3] != 0 && board[3 * (row / 3) + i / 3, 3 * (col / 3) + i % 3] == d)
                    return false;
            }
            return true;
        }

        private short[,] BuildBoard(SudokuData sudokuData)
        {
            int k = 0; // accumulator for j
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    board[i, j] = sudokuData.Board[i + k];
                    if (j != 8) k++; // do not accumulate at max j
                }
            }
            return board;
        }

        private short[] FlattenBoard()
        {
            short[] flattened = new short[81];
            int k = 0; // accumulator for j
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    flattened[i + k] = board[i, j];
                    if (j != 8) k++; // do not accumulate at max j
                }
            }
            return flattened;
        }
    }
}
