namespace AfriNetRouterLib.Extensions
{
    public static class LanguageExtExtensions
    {
        public static TryAsync<IEnumerable<B>> Concat<B>(this TryAsync<IEnumerable<B>> first, TryAsync<IEnumerable<B>> second)
        => TryAsync(async () =>
        {
            var firstResult = new List<B>();
            await first.Match<IEnumerable<B>>(result => firstResult = result.ToList(), ex => throw ex);

            var secondResult = new List<B>();
            await second.Match<IEnumerable<B>>(result => secondResult = result.ToList(), ex => throw ex);

            return firstResult.Concat(secondResult);
        }); 
    }
}
