
namespace InterviewApp.Client.Extensions;

public static class RandomExtensions
{
    private static Random _random = new Random();

    public static T Random<T>(this IEnumerable<T> enumerable)
    {
        if (enumerable is null)
        {
            throw new ArgumentNullException(nameof(enumerable));
        }

        return enumerable.OrderBy(_ => _random.Next()).First();
    }

    public static IEnumerable<T> Random<T>(this IEnumerable<T> enumerable, int count)
    {
        if (enumerable is null)
        {
            throw new ArgumentNullException(nameof(enumerable));
        }

        return enumerable.OrderBy(_ => _random.Next()).Take(count);
    }
}
