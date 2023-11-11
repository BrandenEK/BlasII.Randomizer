using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlasII.Randomizer.Metrics
{
    public class MetricsHandler
    {
        public async Task SendSettings(RandomizerSettings settings)
        {
            if (!Main.Randomizer.ConfigHandler.GetProperty<bool>("SendMetrics"))
                return;

            Main.Randomizer.Log("Sending metrics!");
        }
    }
}
