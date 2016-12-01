using System;
using FileSizeCounter.Constants;

namespace FileSizeCounter.Extensions
{
    internal static class DoubleEx
    {
        /// <summary>
        /// Compare if two doubles are nearly the same value
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool AlmostEqualTo(this double left, double right)
        {
            return Math.Abs(left - right) < Consts.FloatCompareTolerance;
        }
    }
}
