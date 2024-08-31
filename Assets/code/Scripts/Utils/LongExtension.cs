public static class LongExtension
{
    public static string ToStringRupiahFormat(this long value) => "Rp" + value.ToString("N0");
    public static string ToStringCurrencyFormat(this long value) => value.ToString("N0");
    public static string ToStringCurrencyFormat(this int value) => value.ToString("N0");
    public static string ToStringCurrencyFormat(this float value) => value.ToString("N0");
    public static string ToStringRupiahFormat(this float value) => "Rp" + value.ToString("N0");
    public static string ToStringRupiahFormat(this int value) => "Rp" + value.ToString("N0");
}