using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using System.Drawing;

namespace Hopfield
{
    // Класс для расшения возможностей работы над картинкой
    public static class BitmapExtensions
    {
        // Перевод картинки в матрицу
        public static Matrix ToMatrix(this Bitmap pic)
        {
            Matrix matrix = new Matrix(m: pic.Height, n: pic.Width);
            for (int r = 0; r < pic.Height; ++r)
            {
                for (int c = 0; c < pic.Width; ++c)
                {
                    matrix[r, c] = pic.GetPixel(c, r).R > 0 ? -1 : +1;
                }
            }
            return matrix;
        }
    }
}
