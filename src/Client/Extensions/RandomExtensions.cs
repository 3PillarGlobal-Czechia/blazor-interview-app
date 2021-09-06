
namespace InterviewApp.Client.Extensions;

public static class RandomExtensions
{
    public static T Random<T>(this IEnumerable<T> enumerable, Random random)
    {
        if (enumerable is null)
        {
            throw new ArgumentNullException(nameof(enumerable));
        }

        var list = enumerable as IList<T> ?? enumerable.ToList();

        return list.Count == 0 ? default! : list[random.Next(0, list.Count)];
    }
}
