using System;
using System.Collections.Generic;
using Realmar.DataBindings.Editor.BCL.System;
using static Realmar.DataBindings.Editor.Exceptions.YeetHelpers;

namespace Realmar.DataBindings.Editor.IoC
{
	internal class ServiceLocator
	{
		private struct BindingConfiguration
		{
			internal Type From;
			internal Type To;
			internal ServiceLifetime Lifetime;
			internal object Id;
		}

		private readonly Dictionary<int, BindingConfiguration> _bindings = new Dictionary<int, BindingConfiguration>();
		private readonly Dictionary<int, object> _singletons = new Dictionary<int, object>();

		private static ServiceLocator _current;

		internal static ServiceLocator Current
		{
			get
			{
				if (_current == null)
				{
					Reset();
				}

				return _current;
			}
		}

		internal static void Reset()
		{
			_current = new ServiceLocator();
		}

		internal void RegisterType<TFrom>(ServiceLifetime lifetime = ServiceLifetime.Transient, object id = null)
		{
			RegisterType<TFrom, TFrom>(lifetime, id);
		}

		internal void RegisterType<TFrom, TTo>(ServiceLifetime lifetime = ServiceLifetime.Transient, object id = null)
		{
			RegisterType(typeof(TFrom), typeof(TTo), lifetime, id);
		}

		internal void RegisterType<TFrom>(TFrom obj, object id = null)
		{
			YeetIfNull(obj, nameof(obj));

			var from = typeof(TFrom);
			var hash = HashCode.Combine(from, id);

			YeetIfDuplicateBinding(from, hash);

			_singletons[hash] = obj;
			_bindings[hash] = new BindingConfiguration
			{
				From = from,
				To = from,
				Lifetime = ServiceLifetime.Singleton,
				Id = id
			};
		}

		private void RegisterType(Type from, Type to, ServiceLifetime lifetime, object id = null)
		{
			var hash = HashCode.Combine(from, id);

			YeetIfDuplicateBinding(from, hash);

			var defaultCtor = to.GetConstructor(Type.EmptyTypes);
			var locatorCtor = to.GetConstructor(new[] { typeof(ServiceLocator) });

			if (defaultCtor == null && locatorCtor == null)
			{
				throw new ArgumentException($"To type must have a default constructor or a constructor taking {nameof(ServiceLocator)} as its sole argument. Type = {to.FullName}");
			}

			_bindings[hash] = new BindingConfiguration
			{
				From = from,
				To = to,
				Lifetime = lifetime,
				Id = id
			};
		}

		internal T Resolve<T>(object id = null)
		{
			var hash = HashCode.Combine(typeof(T), id);
			if (_bindings.TryGetValue(hash, out var configuration) == false)
			{
				throw new ArgumentException($"Cannot find binding for type {typeof(T).FullName}", nameof(T));
			}

			switch (configuration.Lifetime)
			{
				case ServiceLifetime.Transient:
					return (T) CreateInstance(configuration.To);
				case ServiceLifetime.Singleton:
					return (T) ResolveSingleton(configuration.To, hash);
				default:
					throw new ArgumentOutOfRangeException("Unknown service lifetime");
			}
		}

		private object ResolveSingleton(Type type, int hash)
		{
			if (_singletons.TryGetValue(hash, out var obj) == false)
			{
				obj = CreateInstance(type);
				_singletons[hash] = obj;
			}

			return obj;
		}

		private object CreateInstance(Type type)
		{
			object result;

			if (type.GetConstructor(Type.EmptyTypes) == null)
			{
				result = Activator.CreateInstance(type, this);
			}
			else
			{
				result = Activator.CreateInstance(type);
			}

			return result;
		}

		private void YeetIfDuplicateBinding(Type type, int hash)
		{
			if (_bindings.ContainsKey(hash))
			{
				throw new ArgumentException($"Duplicate binding for {type.FullName}", nameof(type));
			}
		}
	}
}
