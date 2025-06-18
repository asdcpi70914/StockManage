using System.Drawing;
using System.Text;

namespace SRC.Backend.Models.Brain
{
    public class HomeLogic
    {
        private Serilog.ILogger Logger { get; set; }

        public HomeLogic(Serilog.ILogger logger) 
        {
            Logger = logger;
        }
    }
}
