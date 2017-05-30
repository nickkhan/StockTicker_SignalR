using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StockChartMVC_SignalR.Startup))]
namespace StockChartMVC_SignalR
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
