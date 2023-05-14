namespace Acau_Playground.Extensions
{
    public static class ComparerExtension
    {
        public static bool DefaultEquals<T>(this T value, ref T variable)
        {
            if (EqualityComparer<T>.Default.Equals(value, variable)) return true;

            variable = value;
            return false;
        }
    }
}
