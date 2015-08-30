﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinSocialApp.UI.Common.Interfaces.ViewModels
{
	public interface IPageUserDialogsVm
	{
		string UserName { get; }
		IList<string> Items { get; }
	}
}
