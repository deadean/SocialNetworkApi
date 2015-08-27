using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Common.MVVM.Library
{
	public static class NotifyPropertyChangedBaseExtensions
	{
		/// <summary>
		/// Raises PropertyChanged event.
		/// To use: call the extension method with this: this.OnPropertyChanged(n => n.Title);
		/// </summary>
		/// <typeparam name="T">Property owner</typeparam>
		/// <typeparam name="TProperty">Type of property</typeparam>
		/// <param name="observableBase"></param>
		/// <param name="expression">Property expression like 'n => n.Property'</param>
		//public static void OnPropertyChanged<T, TProperty>(this T observableBase, Expression<Func<T, TProperty>> expression)
		//	where T : INotifyPropertyChangedWithRaise
		//{
		//	observableBase.RaisePropertyChanged1(GetPropertyName(expression));
		//}

		public static void OnPropertyChanged<T>(this T observableBase, [CallerMemberName] string propertyName = null)
			where T : INotifyPropertyChangedWithRaise
		{
			observableBase.RaisePropertyChanged1(propertyName);
		}

		/// <summary>
		/// Raises PropertyChanged event.
		/// To use: call the extension method with this: this.OnPropertyChanged(n => n.Title, n => n.Text);
		/// </summary>
		/// <typeparam name="T">Property owner</typeparam>
		/// <param name="observableBase"></param>
		/// <param name="expressions">Property expressions like 'n => n.Property1, n => n.Property2, ...'</param>
		public static void OnPropertyChanged<T>(this T observableBase, params Expression<Func<T, object>>[] expressions)
			where T : INotifyPropertyChangedWithRaise
		{
			foreach (var expression in expressions)
			{
				OnPropertyChanged(observableBase, GetPropertyName(expression));
			}
		}

		public static string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> expression) where T : INotifyPropertyChangedWithRaise
		{
			if (expression == null)
				throw new ArgumentNullException("expression");

			var memberExpression = expression.Body as MemberExpression;

			if (memberExpression == null)
				throw new ArgumentNullException("expression");

			return memberExpression.Member.Name;
		}
	}
}
