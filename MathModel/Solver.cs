using System;

namespace MathModel
{
    public static class Solver
        // TODO: Move all constants to fields and assign them in contructor
    {
        public static Tuple<double, double> Solve()
        {
            const double T1Min = -18; 
            const double T1Max = 7; 
            const double T2Min = -8; 
            const double T2Max = 8;
            const double precision = 1;
            var condition = new Func<double, double, bool>((t1, t2) => t1 + t2 <= 4);

            return FindTemperatures(T1Min, T1Max, T2Min, T2Max, precision, condition);
        }

        private static Tuple<double, double> FindTemperatures(double T1Min, double T1Max, double T2Min, double T2Max, double precision, Func<double, double, bool> condition)
        {
            double maxS = double.MinValue;
            double bestT1 = -1, bestT2 = -1;
            for (double t1 = T1Min; t1 < T1Max; t1 += precision)
            {
                for (double t2 = T2Min; t2 < T2Max; t2 += precision)
                {
                    if (condition.Invoke(t1, t2))
                    {
                        var calculatedS = CalculateS(t1, t2);
                        if (maxS < calculatedS)
                        {
                            maxS = calculatedS;
                            bestT1 = t1;
                            bestT2 = t2;
                        }
                    }
                }
            }
            return new Tuple<double, double>(bestT1, bestT2);
        }

        private static double CalculateS(double T1, double T2)
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
