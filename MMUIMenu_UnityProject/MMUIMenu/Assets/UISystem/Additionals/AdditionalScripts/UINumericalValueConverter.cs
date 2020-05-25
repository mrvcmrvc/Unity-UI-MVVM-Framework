namespace MVVM
{
    public static class UINumericalValueConverter
    {
        private static string[] suffix = new string[] { "", "k", "M", "B", "T",
        "AA", "BB", "CC", "DD", "EE", "FF", "GG", "HH", "II", "JJ", "KK",
        "LL", "MM", "NN", "OO", "PP", "QQ", "RR", "SS", "TT", "UU", "WW", "XX", "YY", "ZZ",
        "AAA", "BBB", "CCC", "DDD", "EEE", "FFF", "GGG", "HHH", "III", "JJJ", "KKK",
        "LLL", "MMM", "NNN", "OOO", "PPP", "QQQ", "RRR", "SSS", "TTT", "UUU", "WWW", "XXX", "YYY", "ZZZ"};

        public static string NumericalValueToString(double valueToConvert)
        {
            int scale = 0;

            double v = valueToConvert;

            while (v >= 1000d)
            {
                v /= 1000d;
                scale++;
                if (scale >= suffix.Length)
                    return valueToConvert.ToString("e2"); // overflow, can't display number, fallback to exponential
            }
            return v.ToString("0.#") + suffix[scale];
        }
    }
}
