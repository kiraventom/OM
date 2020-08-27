using System;

namespace MathModel
{
    public static class Solver
    {
        public static double Solve(double T1, double T2)
        {
            double alpha = 1;
            double beta = 1;
            double eta = 1;
            double delta = 1;
            double G = 1;
            double A = 1;
            double N = 2;
            double S = alpha * G * (Math.Pow((T2 - beta * A), N) + eta * Math.Pow(Math.Exp(T1 + T2), N) + delta * (T2 - T1));
            return S;
        }
    }
}
