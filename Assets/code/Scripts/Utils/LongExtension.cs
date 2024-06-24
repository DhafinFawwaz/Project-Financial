public static class LongExtension
{
    public static string ToStringRupiahFormat(this long value) => "Rp" + value.ToString("N0");
}