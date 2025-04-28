namespace StockAnalyzer.Shared.Utils;

public static class StockIndexHelper
{
    public static double CalculateRSI(double[] prices, int period)
    {
        if (prices.Length < period)
            throw new ArgumentException("Not enough data to calculate RSI.");

        double[] gains = new double[prices.Length];
        double[] losses = new double[prices.Length];

        for (int i = 1; i < prices.Length; i++)
        {
            double change = prices[i] - prices[i - 1];
            if (change > 0)
            {
                gains[i] = change;
                losses[i] = 0;
            }
            else
            {
                gains[i] = 0;
                losses[i] = -change;
            }
        }

        double avgGain = gains.Skip(1).Take(period).Average();
        double avgLoss = losses.Skip(1).Take(period).Average();

        double rs = avgGain / avgLoss;
        double rsi = 100 - (100 / (1 + rs));

        for (int i = period; i < prices.Length; i++)
        {
            avgGain = ((avgGain * (period - 1)) + gains[i]) / period;
            avgLoss = ((avgLoss * (period - 1)) + losses[i]) / period;

            rs = avgGain / avgLoss;
            rsi = 100 - (100 / (1 + rs));
        }

        return rsi;
    }
}
