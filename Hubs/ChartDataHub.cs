using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockChartMVC_SignalR.Hubs
{
    [HubName("chartData")]
    public class ChartDataHub : Hub
    {
        private readonly ChartData _pointer;

        public ChartDataHub() : this(ChartData.Instance) { }

        public ChartDataHub(ChartData pointer)
        {
            _pointer = pointer;
        }

        public IEnumerable<Price> initData()
        {
            return _pointer.initData();
        }
    }
}