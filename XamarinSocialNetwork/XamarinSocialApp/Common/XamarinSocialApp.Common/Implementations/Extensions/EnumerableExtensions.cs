using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Joins the specified source.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="delimeter">The delimeter.</param>
		/// <returns></returns>
		public static string Join(this IEnumerable source, string delimeter)
		{
			StringBuilder strBuilder = new StringBuilder();
			bool isSecond = false;

			foreach (var obj in source)
			{
				if (isSecond)
					strBuilder.Append(delimeter);
				else
					isSecond = true;

				strBuilder.Append(obj);
			}

			return strBuilder.ToString();
		}

		/// <summary>
		/// Fors the each.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		/// <param name="action">The action.</param>
		[DebuggerStepThrough]
		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			if (source == null || action == null)
				return;

			foreach (var item in source)
			{
				action(item);
			}
		}

		[DebuggerStepThrough]
		public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
		{
			if (source == null || action == null)
				return;

			for (int i = 0; i < source.Count(); i++)
			{
				action(source.ElementAt(i), i);
			}
		}

		/// <summary>
		/// Fors the each.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		/// <param name="action">The action.</param>
		[DebuggerStepThrough]
		public static void ForEach(this IEnumerable source, Action<object> action)
		{
			if (source == null || action == null)
				return;

			foreach (var item in source)
			{
				action(item);
			}
		}

		/// <summary>
		/// Fors the each.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		/// <param name="action">The action.</param>
		[DebuggerStepThrough]
		public static void ForEach(this IEnumerable source, Action<object, int> action)
		{
			if (source == null || action == null)
				return;

			var i = 0;

			foreach (var item in source)
			{
				action(item, i);
				i++;
			}
		}

		[DebuggerStepThrough]
		public static async Task ForAllAsync<T>(this IEnumerable<T> source, Func<T, Task> action)
		{
			if (source == null || action == null)
				return;

			await Task.WhenAll(source.Select(x => Task.Run(() => action(x))));
		}

		[DebuggerStepThrough]
		public static Task ForAllAsync(this IEnumerable source, Func<object, Task> action)
		{
			if (source == null || action == null)
				return Task.FromResult<object>(null);

			return Task.WhenAll(source.Cast<object>().Select(x => Task.Run(() => action(x))));
		}

		[DebuggerStepThrough]
		public static Task<TResult[]> ForAllAsync<T, TResult>(this IEnumerable<T> source, Func<T, Task<TResult>> action)
		{
			if (source == null || action == null)
				return Task.FromResult<TResult[]>(null);

			return Task.WhenAll(source.Select(x => action(x)));
		}

		[DebuggerStepThrough]
		public static Task<TResult[]> ForAllAsync<TResult>(this IEnumerable source, Func<object, Task<TResult>> action)
		{
			if (source == null || action == null)
				return Task.FromResult<TResult[]>(null);

			return Task.WhenAll(source.Cast<object>().Select(x => action(x)));
		}


		[DebuggerStepThrough]
		public static async Task ForEachAsync(this IEnumerable source, Func<object, Task> action)
		{
			if (source == null || action == null)
				return;

			foreach (var item in source)
			{
				await action(item);
			}
		}

		//[DebuggerStepThrough]
		public static async Task ForEachAsync(this IEnumerable source, Func<object, Task> action, CancellationToken token)
		{
			if (source == null || action == null)
				return;

			foreach (var item in source)
			{
				if (token.IsCancellationRequested)
					return;

				await action(item);
			}
		}

		[DebuggerStepThrough]
		public static async Task ForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> action)
		{
			if (source == null || action == null)
				return;

			foreach (var item in source)
			{
				await action(item);
			}
		}

		[DebuggerStepThrough]
		public static async Task<List<TResult>> ForEachAsync<T, TResult>(this IEnumerable<T> source, Func<T, Task<TResult>> action)
		{
			if (source == null || action == null)
				return null;

			var results = new List<TResult>();

			foreach (var item in source)
			{
				results.Add(await action(item));
			}

			return results;
		}

		[DebuggerStepThrough]
		public static async Task<IList<TResult>> ForEachAsync<TResult>(this IEnumerable source, Func<object, Task<TResult>> action)
		{
			if (source == null || action == null)
				return null;

			var results = new List<TResult>();

			foreach (var item in source)
			{
				results.Add(await action(item));
			}

			return results;
		}

		/// <summary>
		/// Indexes the of.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj">The obj.</param>
		/// <param name="compareFunction">The compare function.</param>
		/// <returns></returns>
		public static int IndexOf<T>(this IEnumerable<T> collection, Predicate<T> compareFunction)
		{
			return IndexOf<T>(collection, compareFunction, -1);
		}

		/// <summary>
		///   Gets index of specified item with specified comparer
		/// </summary>
		/// <typeparam name="T">Target type</typeparam>
		/// <param name="collection">Source collection</param>
		/// <param name="item">Item to get index of</param>
		/// <param name="comparer">Target type item comparer</param>
		/// <returns>Index of specified item in source collection</returns>
		public static int IndexOf<T>(this IEnumerable<T> source, T item, IEqualityComparer<T> comparer)
		{
			if (source == null || item == null)
				return -1;

			var itemComparer = comparer ?? EqualityComparer<T>.Default;

			return source.IndexOf(x => itemComparer.Equals(x, item));
		}

		/// <summary>
		/// Indexes the of.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection">The collection.</param>
		/// <param name="compareFunction">The compare function.</param>
		/// <param name="startIndex">The start index.</param>
		/// <returns></returns>
		public static int IndexOf<T>(this IEnumerable<T> collection, Predicate<T> compareFunction, int startIndex)
		{
			int index = -1;

			foreach (T element in collection)
			{
				index++;

				bool isCompare = compareFunction(element);

				if (isCompare && index > startIndex)
					return index;
			}

			return -1;
		}

		/// <summary>
		/// Produces a sequence of items using a seed value and iteration 
		/// method.
		/// </summary>
		/// <typeparam name="T">The type of the sequence.</typeparam>
		/// <param name="value">The initial value.</param>
		/// <param name="next">The iteration function.</param>
		/// <returns>A sequence of items using a seed value and iteration 
		/// method.</returns>
		public static IEnumerable<T> Iterate<T>(T value, Func<T, T> next)
		{
			do
			{
				yield return value;
				value = next(value);
			}
			while (true);
		}

		/// <summary>
		/// Prepend an item to a sequence.
		/// </summary>
		/// <typeparam name="T">The type of the sequence.</typeparam>
		/// <param name="that">The sequence to append the item to.</param>
		/// <param name="value">The item to append to the sequence.</param>
		/// <returns>A new sequence.</returns>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Code is linked into multiple projects.")]
		public static IEnumerable<T> Prepend<T>(this IEnumerable<T> that, T value)
		{
			if (that == null)
			{
				throw new ArgumentNullException("that");
			}

			yield return value;
			foreach (T item in that)
			{
				yield return item;
			}
		}

		/// <summary>
		/// Accepts two sequences and applies a function to the corresponding 
		/// values in the two sequences.
		/// </summary>
		/// <typeparam name="T0">The type of the first sequence.</typeparam>
		/// <typeparam name="T1">The type of the second sequence.</typeparam>
		/// <typeparam name="R">The return type of the function.</typeparam>
		/// <param name="enumerable0">The first sequence.</param>
		/// <param name="enumerable1">The second sequence.</param>
		/// <param name="func">The function to apply to the corresponding values
		/// from the two sequences.</param>
		/// <returns>A sequence of transformed values from both sequences.</returns>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Code is linked into multiple projects.")]
		public static IEnumerable<R> Zip<T0, T1, R>(IEnumerable<T0> enumerable0, IEnumerable<T1> enumerable1, Func<T0, T1, R> func)
		{
			IEnumerator<T0> enumerator0 = enumerable0.GetEnumerator();
			IEnumerator<T1> enumerator1 = enumerable1.GetEnumerator();
			while (enumerator0.MoveNext() && enumerator1.MoveNext())
			{
				yield return func(enumerator0.Current, enumerator1.Current);
			}
		}

		/// <summary>
		/// Determines whether [is all same] [the specified sequence].
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sequence">The sequence.</param>
		/// <param name="comparer">The comparer.</param>
		/// <returns>
		///   <c>true</c> if [is all same] [the specified sequence]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsAllSame<T>(this IEnumerable<T> sequence, IEqualityComparer<T> comparer)
		{
			var result = new List<T>();
			var enumerator = sequence.GetEnumerator();

			if (comparer == null)
				comparer = EqualityComparer<T>.Default;

			while (enumerator.MoveNext())
			{
				var first = enumerator.Current;

				result.Add(enumerator.Current);

				if (enumerator.MoveNext())
				{
					if (!comparer.Equals(first, enumerator.Current))
					{
						return false;
					}
				}
				else
				{
					break;
				}
			}

			return result.Count == 1 || result.IsAllSame(comparer);
		}

		/// <summary>
		/// Determines whether [is all same] [the specified sequence].
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sequence">The sequence.</param>
		/// <returns>
		///   <c>true</c> if [is all same] [the specified sequence]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsAllSame<T>(this IEnumerable<T> sequence)
		{
			return IsAllSame(sequence, null);
		}

		public static void Do(int times, Action actionToDo)
		{
			for (int i = 0; i < times; i++)
			{
				actionToDo();
			}
		}

		public static IEnumerable<T> DoWithResult<T>(int times, Func<T> funcToDo)
		{
			for (int i = 0; i < times; i++)
			{
				yield return funcToDo();
			}
		}
	}
}
