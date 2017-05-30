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
                p.Bid = cols[2] != "N/A" ? Convert.ToDecimal(cols[2]) : 0;
                p.Ask = cols[3] != "N/A" ? Convert.ToDecimal(cols[3]) : 0;
                p.Open = cols[4] != "N/A" ? Convert.ToDecimal(cols[4]) : 0; 
                p.PreviousClose = Convert.ToDecimal(cols[5]);
                p.Last = Math.Round(Convert.ToDecimal(cols[6]),2);
                p.Volume = Convert.ToDecimal(cols[7]);
                p.Change = Convert.ToString(cols[8]).Replace("\"", "");
                p.ChangePercent = Convert.ToString(cols[9]).Replace("\"", "");
                p.StockExchange = cols[10].Replace("\"", ""); ;
                p.PERatio = cols[11];
                prices.Add(p);
            }

            return prices;
        }
    }

    public class Price
    {
        public string StockExchange { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public decimal Open { get; set; }
        public decimal PreviousClose { get; set; }
        public decimal Last { get; set; }
        public decimal Volume { get; set; }
        public string Change { get; set; }
        public string ChangePercent { get; set; }
        public string PERatio { get; set; }
    }
}