using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.Data.Interfaces.Entities.Database;
using XamarinSocialApp.UI.Common.Implementations.Bases;
using Common.MVVM.Library;

namespace XamarinSocialApp.UI.Common.VVm.Implementations.ViewModels
{
	public class MessageVm : AdvancedPageViewModelBase<IMessage>
	{

		#region Services
		
		#endregion

		#region Fields

		private string mvContent;
		private string mvName;
		
		#endregion

		#region Properties

		public string Content
		{
			get
			{
				return mvContent;
			}

			set
			{
				mvContent = value;
				this.OnPropertyChanged();
			}
		}

		public string Name
		{
			get
			{
				return mvName;
			}

			set
			{
				mvName = value;
				this.OnPropertyChanged();
			}
		}

		#endregion

		#region Ctor

		public MessageVm(IMessage message)
		{
			EntityModel = message;
			Name = message.User.FirstName;
			Content = message.Content;
		}

		#endregion

		#region Commands

		#endregion

		#region Commands Execute Handlers

		#endregion

		#region Public Methods

		#endregion

		#region Private Methods

		#endregion

		#region Protected Methods

		#endregion
	}
}
