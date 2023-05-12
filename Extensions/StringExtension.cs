namespace Acau_Playground.Extensions
{
    public static class StringExtension
    {
        public static string ToMoneyComma(this int str)
        {
            return $"{str:N0}";
        }
    }
}
