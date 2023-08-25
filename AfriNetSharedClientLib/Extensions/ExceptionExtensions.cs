namespace AfriNetSharedClientLib.Extensions
{
    public static class ExceptionExtensions
    {
        public static T? ToDefault<T>(this Exception exception)
        {
            Console.WriteLine(exception.ToString());
            return default(T);  
        }
    }
}
