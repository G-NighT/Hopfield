using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace Hopfield
{
    // Класс области для рисования
    public class DrawField
    {
        public const int Left = 170;
        public const int Top = 34;
        public const int Size = 180;
        public const int CellSize = 15;
        public const int CellsCount = Size / CellSize;
        private int[,] matrix;

        public DrawField()
        {
            matrix = new int[CellsCount, CellsCount];
            FillMatrixWith(-1);
        }

        // Получение матрицы из нарисованного
        public Matrix GetMatrix()
        {
            Matrix copy = new Matrix(m: CellsCount, n: CellsCount);
            for (int r = 0; r < CellsCount; ++r)
            {
                for (int c = 0; c < CellsCount; ++c)
                {
                    copy[r, c] = matrix[r, c];
                }
            }
            return copy;
        }

        private void FillMatrixWith(int value)
        {
            for (int r = 0; r < CellsCount; ++r)
            {
                for (int c = 0; c < CellsCount; ++c)
                {
                    matrix[r, c] = value;
                }
            }
        }

        public void Clear()
        {
            FillMatrixWith(-1);
        }

        public int this[int row, int column]
        {
            get { return matrix[row, column]; }
            set { matrix[row, column] = value; }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int r = 0; r < CellsCount; ++r)
            {
                for (int c = 0; c < CellsCount; ++c)
                {
                    sb.AppendFormat("{0} ", this[r, c]);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public string ToPrettyString()
        {
            var sb = new StringBuilder();
            for (int r = 0; r < CellsCount; ++r)
            {
                for (int c = 0; c < CellsCount; ++c)
                {
                    sb.AppendFormat("{0} ", this[r, c] == -1 ? '-' : '#');
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
