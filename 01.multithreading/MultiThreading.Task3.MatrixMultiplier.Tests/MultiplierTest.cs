using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiThreading.Task3.MatrixMultiplier.Matrices;
using MultiThreading.Task3.MatrixMultiplier.Multipliers;

namespace MultiThreading.Task3.MatrixMultiplier.Tests
{
    [TestClass]
    public class MultiplierTest
    {
        [TestMethod]
        public void MultiplyMatrix3On3Test()
        {
            TestMatrix3On3(new MatricesMultiplier());
            TestMatrix3On3(new MatricesMultiplierParallel());
        }

        [TestMethod]
        [DataRow(50, 50, DisplayName = "ParallelEfficiencyTest (50,50) First run")]
        [DataRow(50, 50, DisplayName = "ParallelEfficiencyTest (50,50) Second run")]
        [DataRow(50, 50, DisplayName = "ParallelEfficiencyTest (50,50) Third run")]
        [DataRow(100, 50)]
        [DataRow(100, 100)]
        [DataRow(25, 25)]
        [DataRow(30, 10)]
        [DataRow(10, 10)]
        [DataRow(500, 200)]
        [DataRow(40, 40)]
        [DataRow(30, 30)]
        //[DataRow(1000, 1000)]
        public void ParallelEfficiencyTest(int rows, int cols)
        {
            IMatricesMultiplier m = new MatricesMultiplier();
            Stopwatch stopwatch = new Stopwatch();

            var firstMatrix = new Matrix(rows, cols, true);
            var secondMatrix = new Matrix(rows, cols, true);

            stopwatch.Start();
            m.Multiply(firstMatrix, secondMatrix);
            stopwatch.Stop();

            var sequentialTime = stopwatch.ElapsedMilliseconds;
            var sequentialTicks = stopwatch.ElapsedTicks;
            Console.WriteLine($"Sequential loop time in milliseconds: {sequentialTime}");
            Console.WriteLine($"Sequential loop time in ticks: {sequentialTicks}");

            stopwatch.Reset();
            m = new MatricesMultiplierParallel();

            stopwatch.Start();
            m.Multiply(firstMatrix, secondMatrix);
            stopwatch.Stop();

            var parallelTime = stopwatch.ElapsedMilliseconds;
            var parallelTicks = stopwatch.ElapsedTicks;
            Console.WriteLine($"Parallel loop time in milliseconds: {parallelTime}");
            Console.WriteLine($"Parallel loop time in ticks: {parallelTicks}");

            Assert.IsTrue(parallelTicks < sequentialTicks);
        }

        #region private methods

        void TestMatrix3On3(IMatricesMultiplier matrixMultiplier)
        {
            if (matrixMultiplier == null)
            {
                throw new ArgumentNullException(nameof(matrixMultiplier));
            }

            var m1 = new Matrix(3, 3);
            m1.SetElement(0, 0, 34);
            m1.SetElement(0, 1, 2);
            m1.SetElement(0, 2, 6);

            m1.SetElement(1, 0, 5);
            m1.SetElement(1, 1, 4);
            m1.SetElement(1, 2, 54);

            m1.SetElement(2, 0, 2);
            m1.SetElement(2, 1, 9);
            m1.SetElement(2, 2, 8);

            var m2 = new Matrix(3, 3);
            m2.SetElement(0, 0, 12);
            m2.SetElement(0, 1, 52);
            m2.SetElement(0, 2, 85);

            m2.SetElement(1, 0, 5);
            m2.SetElement(1, 1, 5);
            m2.SetElement(1, 2, 54);

            m2.SetElement(2, 0, 5);
            m2.SetElement(2, 1, 8);
            m2.SetElement(2, 2, 9);

            var multiplied = matrixMultiplier.Multiply(m1, m2);
            Assert.AreEqual(448, multiplied.GetElement(0, 0));
            Assert.AreEqual(1826, multiplied.GetElement(0, 1));
            Assert.AreEqual(3052, multiplied.GetElement(0, 2));

            Assert.AreEqual(350, multiplied.GetElement(1, 0));
            Assert.AreEqual(712, multiplied.GetElement(1, 1));
            Assert.AreEqual(1127, multiplied.GetElement(1, 2));

            Assert.AreEqual(109, multiplied.GetElement(2, 0));
            Assert.AreEqual(213, multiplied.GetElement(2, 1));
            Assert.AreEqual(728, multiplied.GetElement(2, 2));
        }

        #endregion
    }
}
