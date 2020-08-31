using System.Collections;
using System.Collections.Generic;

namespace MathModel
{
    public class Solution
    {
        internal Solution(double temperature1, double temperature2, double s, IEnumerable<double> profitValues)
        {
            this.Temperature1 = temperature1;
            this.Temperature2 = temperature2;
            this.Profit = s;
            this.ProfitValues = profitValues;
        }

        public double Temperature1 { get; }
        public double Temperature2 { get; }
        public double Profit { get; }
        public IEnumerable<double> ProfitValues { get; }
    }
}
