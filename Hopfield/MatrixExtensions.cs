using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using System.Drawing;

namespace Hopfield
{
    // Класс для дополнительных действий над матрицами
    public static class MatrixExtensions
    {
        // В вектор по столбцам
        public static Vector ToVectorByColumns(this Matrix m)
        {
            List<double> vector = new List<double>();
            for (int i = 0; i < m.ColumnCount; ++i)
            {
                vector.AddRange(m.GetColumnVector(i).ToArray());
            }
            return new Vector(vector.ToArray());
        }

        // В картинку
        public static Bitmap ToBitmap(this Matrix m)
        {
            Bitmap pic = new Bitmap(width: m.ColumnCount, height: m.RowCount);
            for (int r = 0; r < m.RowCount; ++r)
            {
                for (int c = 0; c < m.ColumnCount; ++c)
                {
                    Color color = m[r, c] > 0 ? Color.Black : Color.White;
                    pic.SetPixel(c, r, color);
                }
            }
            return pic;
        }

        // В строку
        public static string ToPrettyString(this Matrix m)
        {
            var sb = new StringBuilder();
            for (int r = 0; r < m.RowCount; ++r)
            {
                for (int c = 0; c < m.ColumnCount; ++c)
                {
                    sb.AppendFormat("{0} ", m[r, c] == -1 ? '-' : '#');
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
