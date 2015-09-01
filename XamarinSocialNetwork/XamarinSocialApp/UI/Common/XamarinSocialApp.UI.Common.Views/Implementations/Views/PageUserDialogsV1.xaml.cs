using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using XamarinSocialApp.UI.Common.Interfaces.Views.Bases;

namespace XamarinSocialApp.UI.Common.Views.Implementations.Views
{
	public partial class PageUserDialogsV1 : ContentPage, IBasePage
	{
		public PageUserDialogsV1()
		{
			InitializeComponent();
		}

		public PageUserDialogsV1(object p)
			: this()
		{

		}

		public async Task PostInitizlization()
		{
		}
	}
}
