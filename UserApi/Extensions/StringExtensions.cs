namespace UserApi.Extensions
{
    public static class StringExtensions
    {
        public static int? AsInt(this string str)
        {
            int intVal;
            if (int.TryParse(str, out intVal))
            {
                return intVal;
            }

            return null;
        }
    }
}