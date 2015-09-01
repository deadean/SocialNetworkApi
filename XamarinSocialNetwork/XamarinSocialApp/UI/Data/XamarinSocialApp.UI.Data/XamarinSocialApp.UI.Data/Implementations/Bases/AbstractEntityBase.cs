using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.Data.Common.Interfaces.Entities;
using XamarinSocialApp.Services.Common.Interfaces.Cache;
using XamarinSocialApp.Services.UI.Interfaces.Model;

namespace XamarinSocialApp.UI.Data.Implementations.Bases
{
	public abstract class AbstractEntityBase : IEntity
	{

		#region Fields

		private readonly IInternalModelService modModelService;
		private readonly ICacheService modCacheService;

		#endregion

		#region Properties

		protected IInternalModelService ModelService { get { return modModelService; } }
		protected ICacheService CacheService { get { return modCacheService; } }

		public string ClassType { get; private set; }

		public string IdEntity
		{
			get { return PrivateGetEntityId(); }
		}

		protected abstract string PrivateGetEntityId();

		#endregion

		#region Ctor

		protected AbstractEntityBase()
		{
			GenerateId();
			modModelService = ServiceLocator.Current.GetInstance<IInternalModelService>();
			modCacheService = ServiceLocator.Current.GetInstance<ICacheService>();
		}

		#endregion

		#region Public Methods

		#endregion

		#region Private Methods

		#endregion

		#region Protected Methods

		protected abstract void GenerateId();

		protected void GenerateIdAndClassType(string classType)
		{
			ClassType = classType;
		}

		#endregion

	}
}
