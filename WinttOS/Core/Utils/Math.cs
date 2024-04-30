namespace WinttOS.Core.Utils
{
    public static class Math
    {
        public static double Persentage(double value, double max, double min = 0) =>
            ((value-min) / (max-min)) * 100;
    }
}
