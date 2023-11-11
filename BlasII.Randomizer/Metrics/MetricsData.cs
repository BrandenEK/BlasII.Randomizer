
namespace BlasII.Randomizer.Metrics
{
    internal class MetricsData
    {
        public int sw;
        public int rq;

        public MetricsData(RandomizerSettings settings)
        {
            sw = settings.startingWeapon;
            rq = settings.requiredKeys;
        }
    }
}
