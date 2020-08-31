using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace MathModel
{
    // S = alpha * G * ((T2 - beta * A)^N + eta * exp(T1 + T2)^N + delta * (T2 - T1))
    // alpha, beta, eta, delta = 1
    // G = 1 кг/ч, A = 1 КПа, N = 2 шт.

    public static class Solver // TODO: Move all constants to fields and assign them in contructor
    {
        public static Solution Solve(double G, double A, int N)
        {
            const double T1Min = -18; 
            const double T1Max = 7; 
            const double T2Min = -8; 
            const double T2Max = 8;
            const double precision = 1;
            var condition = new Func<double, double, bool>((t1, t2) => t1 + t2 <= 4);
            const double multiplier = 10;

            var (T1, T2, MaxS, SValues) = GetRawSolution(T1Min, T1Max, T2Min, T2Max, precision, condition, G, A, N);
            return new Solution(T1, T2, MaxS * multiplier, SValues.Select(s => s * multiplier));
        }

        private static (double T1, double T2, double MaxS, IEnumerable<double> SValues) GetRawSolution(double T1Min,
                                                                                                       double T1Max,
                                                                                                       double T2Min,
                                                                                                       double T2Max,
                                                                                                       double precision,
                                                                                                       Func<double, double, bool> condition,
                                                                                                       double G,
                                                                                                       double A,
                                                                                                       int N)
        {
            double maxS = double.MinValue;
            double bestT1 = -1, bestT2 = -1;
            List<double> sValues = new List<double>();
            for (double t1 = T1Min; t1 < T1Max; t1 += precision)
            {
                for (double t2 = T2Min; t2 < T2Max; t2 += precision)
                {
                    if (condition.Invoke(t1, t2))
                    {
                        var calculatedS = CalculateS(t1, t2, G, A, N);
                        sValues.Add(calculatedS);
                        if (maxS < calculatedS)
                        {
                            maxS = calculatedS;
                            bestT1 = t1;
                            bestT2 = t2;
                        }
                    }
                }
            }
            return (bestT1, bestT2, maxS, sValues);
        }

        private static double CalculateS(double T1, double T2, double G, double A, int N)
        {
            double alpha = 1;
            double beta = 1;
            double eta = 1;
            double delta = 1;
            double S = alpha * G * (Math.Pow((T2 - beta * A), N) + eta * Math.Pow(Math.Exp(T1 + T2), N) + delta * (T2 - T1));
            return S;
        }
    }
}
