using System;

namespace Sudoku
{
    internal class SudokuData
    {
        public string Name { get; set; }
        public short[] Board { get; set; }
        public bool Solvable { get; set; }
        public DateTime DateTimeStamp { get; set; }
    }
}
