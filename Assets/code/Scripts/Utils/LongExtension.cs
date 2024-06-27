public static class LongExtension
{
    public static string ToStringRupiahFormat(this long value) => "Rp" + value.ToString("N0");
    public static string ToStringRupiahFormat(this float value) => "Rp" + value.ToString("N0");
    public static string ToStringRupiahFormat(this int value) => "Rp" + value.ToString("N0");
}