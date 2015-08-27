using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace vk2.Commands
{
	public abstract class CommandBase : ViewModelBase, ICommand
	{
		#region Fields

		bool modIsExecuting;

		#endregion

		#region Properties

		public bool IsExecuting
		{
			get
			{
				return modIsExecuting;
			}

			set
			{
				modIsExecuting = value;
				RaisePropertyChanged("IsExecuting");
			}
		}

		public Func<Task> BeforeCommandAsyncAction
		{
			private get;
			set;
		}

		public Func<Task> PostCommandAsyncAction
		{
			private get;
			set;
		}

		public bool IsCanExecute
		{
			get;
			protected set;
		}

		#endregion

		#region Events

		public event EventHandler CanExecuteChanged;

		#endregion

		#region Abstract methods

		public abstract bool CanExecute(object parameter);

		public void Execute(object parameter)
		{
			if (IsExecuting)
				return;

			IsExecuting = true;

			if (BeforeCommandAsyncAction != null)
			{
				BeforeCommandAsyncAction();
			}

			ExecuteProtected(parameter);

			if (PostCommandAsyncAction != null)
			{
				 PostCommandAsyncAction();
			}

			IsExecuting = false;
		}

		#endregion

		#region Public methods

		public void RaiseCanExecuteChanged()
		{
			var handler = CanExecuteChanged;

			if (handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}

		#endregion

		#region Protected

		protected abstract void ExecuteProtected(object parameter);

		#endregion

		#region Overriden

		public override void Cleanup()
		{
			base.Cleanup();
			BeforeCommandAsyncAction = null;
			PostCommandAsyncAction = null;
		}

		#endregion
	}

	public class DelegateCommand<T> : CommandBase
	{
		#region Fields

		private readonly Action<T> modExecuteMethod = null;
		private readonly Func<T, bool> modCanExecuteMethod = null;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of <see cref="DelegateCommand{T}"/>.
		/// </summary>
		/// <param name="executeMethod">Delegate to execute when Execute is called on the command.  This can be null to just hook up a CanExecute delegate.</param>
		/// <remarks><seealso cref="CanExecute"/> will always return true.</remarks>
		public DelegateCommand(Action<T> executeMethod)
			: this(executeMethod, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="DelegateCommand{T}"/>.
		/// </summary>
		/// <param name="executeMethod">Delegate to execute when Execute is called on the command.  This can be null to just hook up a CanExecute delegate.</param>
		/// <param name="canExecuteMethod">Delegate to execute when CanExecute is called on the command.  This can be null.</param>
		/// <exception cref="ArgumentNullException">When both <paramref name="executeMethod"/> and <paramref name="canExecuteMethod"/> ar <see langword="null" />.</exception>
		public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
		{
			if (executeMethod == null && canExecuteMethod == null)
				throw new ArgumentNullException("executeMethod", "Delegate Command Delegates Cannot Be Null");

			this.modExecuteMethod = executeMethod;
			this.modCanExecuteMethod = canExecuteMethod;
		}

		#endregion

		#region Overriden

		///<summary>
		///Defines the method that determines whether the command can execute in its current state.
		///</summary>
		///<param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
		///<returns>
		///<see langword="true" /> if this command can be executed; otherwise, <see langword="false" />.
		///</returns>
		public override bool CanExecute(object parameter)
		{
			try
			{
				if (modCanExecuteMethod == null)
					return IsCanExecute = true;

				return IsCanExecute = modCanExecuteMethod((T)parameter);
			}
			catch (Exception)
			{
			}

			return false;
		}

		protected override void ExecuteProtected(object parameter)
		{
			if (modExecuteMethod == null)
				return;

			modExecuteMethod((T)parameter);
		}

		#endregion
	}

	/// <summary>
	/// An <see cref="ICommand"/> whose delegates can be attached for <see cref="Execute"/> and <see cref="CanExecute"/>.
	/// It also implements the <see cref="IActiveAware"/> interface, which is
	/// useful when registering this command in a <see cref="CompositeCommand"/>
	/// that monitors command's activity.
	/// </summary>
	public class DelegateCommand : CommandBase
	{
		#region Fields

		private readonly Action executeMethod = null;
		private readonly Func<bool> canExecuteMethod = null;
		//private bool _isActive;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of <see cref="DelegateCommand{T}"/>.
		/// </summary>
		/// <param name="executeMethod">Delegate to execute when Execute is called on the command.  This can be null to just hook up a CanExecute delegate.</param>
		/// <remarks><seealso cref="CanExecute"/> will always return true.</remarks>
		public DelegateCommand(Action executeMethod)
			: this(executeMethod, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="DelegateCommand{T}"/>.
		/// </summary>
		/// <param name="executeMethod">Delegate to execute when Execute is called on the command.  This can be null to just hook up a CanExecute delegate.</param>
		/// <param name="canExecuteMethod">Delegate to execute when CanExecute is called on the command.  This can be null.</param>
		/// <exception cref="ArgumentNullException">When both <paramref name="executeMethod"/> and <paramref name="canExecuteMethod"/> ar <see langword="null" />.</exception>
		public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
		{
			if (executeMethod == null && canExecuteMethod == null)
				throw new ArgumentNullException("executeMethod", "Delegate Command Delegates Cannot Be Null");

			this.executeMethod = executeMethod;
			this.canExecuteMethod = canExecuteMethod;
		}

		#endregion

		#region Public Methods

		///<summary>
		///Defines the method that determines whether the command can execute in its current state.
		///</summary>
		///<returns>
		///<see langword="true" /> if this command can be executed; otherwise, <see langword="false" />.
		///</returns>
		public override bool CanExecute(object parameter)
		{
			try
			{
				if (canExecuteMethod == null)
					return IsCanExecute = true;

				return IsCanExecute = canExecuteMethod();
			}
			catch (Exception)
			{
			}

			return false;
		}

		#endregion

		#region Overriden

		protected override void ExecuteProtected(object parameter)
		{
			if (executeMethod == null) return;
			executeMethod();
		}

		#endregion
	}
}
