using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockChartMVC_SignalR.Hubs
{
    /// <summary>
    /// http://www.jarloo.com/yahoo_finance/
    /// </summary>
    public static class YahooFinance
    {
        public static List<Price> Parse(string csvData)
        {
            List<Price> prices = new List<Price>();

            string[] rows = csvData.Replace("\r", "").Split('\n');

            foreach (string row in rows)
            {
                if (string.IsNullOrEmpty(row)) continue;

                string[] cols = row.Split(',');

                Price p = new Price();
                p.Symbol = cols[0].Replace("\"", "");
                p.Name = cols[1].Replace("\"","");
                p.Bid = Convert.ToDecimal(cols[2]);
                p.Ask = Convert.ToDecimal(cols[3]);
                p.Open = Convert.ToDecimal(cols[4]);
                p.PreviousClose = Convert.ToDecimal(cols[5]);
                p.Last = Convert.ToDecimal(cols[6]);
                p.Volume = Convert.ToDecimal(cols[7]);
                prices.Add(p);
            }

            return prices;
        }
    }

    public class Price
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public decimal Open { get; set; }
        public decimal PreviousClose { get; set; }
        public decimal Last { get; set; }
        public decimal Volume { get; set; }
    }
}