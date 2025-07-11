using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottReece.BorderCompare.App
{
    public static class MyGeometry
    {
        public static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        public static double SolveForHypotenuse(double sideA, double sideB)
        {
            return Math.Sqrt(Math.Pow(sideA, 2) + Math.Pow(sideB, 2));
        }
    }
}
