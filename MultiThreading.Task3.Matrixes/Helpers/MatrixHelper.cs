using MultiThreading.Task3.MatrixMultiplier.Matrices;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiThreading.Task3.MatrixMultiplier.Helpers
{
    public static class MatrixHelper
    {
        public static void PopulateMatrix(Matrix matrix)
        {
            var random = new Random();

            for (int i = 0; i < matrix.RowCount; i++)
            {
                for (int j = 0; j < matrix.ColCount; j++)
                {
                    matrix.SetElement(i, j, random.Next(0, 100));
                }
            }

        }
    }
}
