using System.Collections.Generic;

public static class SetExtensions
{
	public static ReadOnlySet<T> AsReadOnly<T>(this ISet<T> set)
	{
		return new ReadOnlySet<T>(set);
	}
}