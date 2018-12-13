using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using System.Diagnostics;

namespace Hopfield
{
    // Класс распознаватель
    public static class Recognizer
    {
        // Создание матрицы весов
        public static Matrix GenerateWeightsMatrix(Vector[] samples)
        {
            int samplesCount = samples.Length;
            int vectorLength = samples[0].Length;
            Matrix weights = new Matrix(m: vectorLength, n: vectorLength);
            for (int i = 0; i < weights.RowCount; ++i)
            {
                for (int j = 0; j < weights.ColumnCount; ++j)
                {
                    if (i == j) continue;
                    for (int k = 0; k < samplesCount; ++k)
                    {
                        weights[i, j] += samples[k][i] * samples[k][j];
                    }
                }
            }
            return weights;
        }

        // Асинхронное распознавание
        public static Matrix RecognizeAsynchronously(Matrix weights, Vector input)
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            Vector oldOutput = input.Clone();
            Vector newOutput = input.Clone();
            //int iterations = 0;
            while (true)
            {
                //Debug.WriteLine("Итерация: {0}", iterations++);
                //Debug.WriteLine(oldOutput);
                for (int i = 0; i < input.Length; ++i)
                {
                    // биполярное кодирование
                    newOutput[i] = weights.GetRowVector(i).ArrayMultiply(newOutput).Sum() >= 0 ? +1 : -1;
                }
                //Debug.WriteLine(newOutput);
                if (newOutput.SequenceEqual(oldOutput))
                {
                    //Debug.WriteLine("Старые и новые выходы равны");
                    Matrix recognized = newOutput.ToMatrixByColumns(columnSize: (int)Math.Sqrt(input.Length));
                    return recognized;
                }
                oldOutput = newOutput.Clone();
            }
        }

        //// Синхронное распознавание
        //public static int RecognizeSynchronously(Matrix weights, Vector input, Vector[] samples)
        //{
        //    //Debug.WriteLine("Recognize()");
        //    //Debug.WriteLine("На вход:");
        //    Vector oldOutput = input.Clone();
        //    int recognizedSampleIndex;
        //    int iterations = 0;
        //    while (true)
        //    {
        //        iterations += 1;
        //        //Debug.WriteLine("Итерация: {0}", iterations);
        //        Vector newOutput = oldOutput.Clone();
        //        for (int r = 0; r < weights.RowCount; ++r)
        //        {
        //            Vector weightsRow = weights.GetRowVector(r);
        //            newOutput[r] = weightsRow.ArrayMultiply(oldOutput).Sum() >= 0 ? +1 : -1;
        //        }
        //        if (newOutput.SequenceEqual(oldOutput))
        //        {
        //            for (int j = 0; j < samples.Length; ++j)
        //            {
        //                if (samples[j].SequenceEqual(newOutput))
        //                {
        //                    recognizedSampleIndex = j;
        //                    //Debug.WriteLine("Распознано - {0}", j);
        //                    return recognizedSampleIndex;
        //                }
        //            }
        //        }
        //        //Console.WriteLine("output:");
        //        //Console.WriteLine(newOutput.ToPrettyString());
        //        oldOutput = newOutput.Clone();
        //        if (iterations > 100)
        //        {
        //            //Debug.WriteLine("Не удалось распознать");
        //            return -1;
        //        }
        //    }
        //}
    }
}
