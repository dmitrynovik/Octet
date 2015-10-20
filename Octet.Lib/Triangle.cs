using System;

namespace Octet.Lib
{
    public static class Triangle
    {
        public static double GetArea(int a, int b, int c)
        {
            ValidateTriangle(a, b, c);

            // see http://en.wikipedia.org/wiki/Triangle#Using_Heron.27s_formula
            var s = (a + b + c) / 2;
            return Math.Sqrt(s * (s - a) * (s - b) * (s - c));
        }

        private static void ValidateTriangle(int a, int b, int c)
        {
            var sortedEdges = new[] { a, b, c };
            Array.Sort(sortedEdges);

            if (sortedEdges[0] <= 0)
                throw new InvalidTriangleException(InvalidTriangleException.ErrorCode.NonPositiveEdge, "Triangle edges must be positive numbers");

            if (sortedEdges[0] + sortedEdges[1] <= sortedEdges[2])
            {
                // see http://en.wikipedia.org/wiki/Triangle_inequality
                throw new InvalidTriangleException(InvalidTriangleException.ErrorCode.TriangleInequality, "The triangle is not valid: the longest edge should be smaller than the sum of other two.");
            }
        }
    }

    public class InvalidTriangleException : Exception
    {
        public enum ErrorCode
        {
            NonPositiveEdge,
            TriangleInequality
        }

        public ErrorCode Error { get; private set; }

        public InvalidTriangleException(ErrorCode code, string message) : base(message)
        {
            Error = code;
        }
    }
}
