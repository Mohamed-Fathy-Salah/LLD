public class WeightSensor
{
    private Random r = new Random();

    public int GetWeightInGrams()
    {
        return r.Next(1000000);
    }
}
