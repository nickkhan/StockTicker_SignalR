using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;

namespace StockChartMVC_SignalR.Hubs
{
    public class ChartData
    {
        private readonly static Lazy<ChartData> _instance = new Lazy<ChartData>(() => new ChartData());
        private readonly ConcurrentQueue<Price> _points = new ConcurrentQueue<Price>();
        private readonly int _updateInterval = 250; //ms        
        private Timer _timer;
        private readonly object _updatePointsLock = new object();
        private bool _updatingData = false;
        private readonly Random _updateOrNotRandom = new Random();
        
        private ChartData()
        {
        }

        public static ChartData Instance
        {
            get
            {
                return _instance.Value;
            }
        }


        /// <summary>
        /// To initialize timer and data
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Price> initData()
        {
            _timer = new Timer(TimerCallBack, null, _updateInterval, _updateInterval);
            return _points.ToArray();
        }

        /// <summary>
        /// Timer callback method
        /// </summary>
        /// <param name="state"></param>
        private void TimerCallBack(object state)
        {
            // This function must be re-entrant as it's running as a timer interval handler
            if (_updatingData)
            {
                return;
            }
            lock (_updatePointsLock)
            {
                if (!_updatingData)
                {
                    _updatingData = true;

                    var data = UpdateData();

                    // check if exchange is closed
                    if (DateTime.Now.Hour < 9.5 || DateTime.Now.Hour > 16)
                    {
                        // random multiplier
                        data.First().Last *= (decimal)((new Random(DateTime.Now.Millisecond)).NextDouble());
                        data.First().Last = Math.Round(data.First().Last, 2);
                    }
                    // only update chart if the last prices has changed
                    if (Prices == null || data.First().Last != Prices.First().Last)
                    {
                        BroadcastChartData(data);
                    }
                    else
                    {
                        Console.WriteLine("no change in last price");
                    }
                    Prices = data;

                    _updatingData = false;
                }
            }
        }

        public List<Price> Prices { get; set; }
        /// <summary>
        /// To update data (Generate random point in our case)
        /// </summary>
        /// <returns></returns>
        private List<Price> UpdateData()
        {
            string csvData;

            using (WebClient web = new WebClient())
            {
                csvData = web.DownloadString("http://finance.yahoo.com/d/quotes.csv?s=MSFT&f=snbaopl1v");
            }

            List<Price> prices = YahooFinance.Parse(csvData);

            return prices;
        }


        private void BroadcastChartData(List<Price> prices)
        {
            foreach (var price in prices)
            {
                GetClients().All.updateData(price);   
            }
        }

        private static dynamic GetClients()
        {
            return GlobalHost.ConnectionManager.GetHubContext<ChartDataHub>().Clients;
        }
    }
}