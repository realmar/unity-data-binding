using System;
using System.Collections.Generic;

namespace DataBindings.Editor.Misc
{
	internal class Pool<T>
	{
		private static readonly Lazy<Pool<T>> _instance = new Lazy<Pool<T>>();
		internal static Pool<T> Instance => _instance.Value;

		internal sealed class Item : IDisposable
		{
			private Pool<T> _pool;
			internal T Value { get; }

			public Item(Pool<T> pool, T value)
			{
				_pool = pool;
				Value = value;
			}

			public void Dispose()
			{
				_pool.Return(this);
			}
		}

		private readonly Queue<Item> _items = new Queue<Item>();

		internal Item Rent()
		{
			if (_items.Count > 0)
			{
				return _items.Dequeue();
			}
			else
			{
				var instance = Activator.CreateInstance<T>();
				return new Item(this, instance);
			}
		}

		internal void Return(Item item)
		{
			_items.Enqueue(item);
		}
	}
}
