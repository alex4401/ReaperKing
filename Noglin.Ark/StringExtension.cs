namespace Noglin.Ark
{
    public static class StringExtension
    {
        public static string GetArkClassName(this string blueprintPath)
        {
            int index = blueprintPath.LastIndexOf('.');
            return blueprintPath.Substring(index + 1);
        }
    }
}