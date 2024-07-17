using System.Collections.Generic;
using System;
using System.Collections;

public class ReadOnlySet<T> : IReadOnlyCollection<T>, ISet<T>
{
	private readonly ISet<T> _set;

	public int Count => _set.Count;

	public bool IsReadOnly => true;


	public ReadOnlySet(ISet<T> set) => _set = set;

	public IEnumerator<T> GetEnumerator() => _set.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	void ICollection<T>.Add(T item)
	{
		throw new NotSupportedException("Set is a read only set.");
	}

	public bool Add(T item)
	{
		throw new NotSupportedException("Set is a read only set.");
	}

	public bool Remove(T item)
	{
		throw new NotSupportedException("Set is a read only set.");
	}

	public void Clear()
	{
		throw new NotSupportedException("Set is a read only set.");
	}

	public void UnionWith(IEnumerable<T> other)
	{
		throw new NotSupportedException("Set is a read only set.");
	}

	public void IntersectWith(IEnumerable<T> other)
	{
		throw new NotSupportedException("Set is a read only set.");
	}

	public void ExceptWith(IEnumerable<T> other)
	{
		throw new NotSupportedException("Set is a read only set.");
	}

	public void SymmetricExceptWith(IEnumerable<T> other)
	{
		throw new NotSupportedException("Set is a read only set.");
	}

	public bool IsSubsetOf(IEnumerable<T> other) => _set.IsSubsetOf(other);

	public bool IsSupersetOf(IEnumerable<T> other) => _set.IsSupersetOf(other);

	public bool IsProperSupersetOf(IEnumerable<T> other) => _set.IsProperSupersetOf(other);

	public bool IsProperSubsetOf(IEnumerable<T> other) => _set.IsProperSubsetOf(other);

	public bool Overlaps(IEnumerable<T> other) => _set.Overlaps(other);

	public bool SetEquals(IEnumerable<T> other) => _set.SetEquals(other);

	public bool Contains(T item) => _set.Contains(item);

	public void CopyTo(T[] array, int arrayIndex) => _set.CopyTo(array, arrayIndex);
}