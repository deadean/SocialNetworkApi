﻿using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.Data.Common.Enums;
using XamarinSocialApp.Data.Interfaces.Entities.Database;
using XamarinSocialApp.Services.UI.Interfaces.Web;
using XamarinSocialApp.Services.UI.Interfaces.Web.OAuth;
using XamarinSocialApp.UI.Data.Implementations.Entities.Databases;
using MessagesUI = XamarinSocialApp.UI.Data.Implementations.Messages.Messages;


namespace XamarinSocialApp.UI.Services.Implementations.Web
{
	public class ApplicationWebService : IApplicationWebService
	{
		#region Services

		#endregion

		#region Fields

		private readonly IOAuthService modService;

		#endregion

		#region Properties


		#endregion

		#region Ctor

		public ApplicationWebService(IOAuthService service)
		{
			modService = service;
		}

		#endregion

		#region Commands

		#endregion

		#region Commands Execute Handlers

		#endregion

		#region Public Methods

		public async Task<bool> SendMessage(IUser user, IUser friend, string message)
		{
			bool result = await modService.SendMessage(user, friend, message, user.SocialNetwork);

			if (user.SocialNetwork == enSocialNetwork.Twitter)
			{
				var messages = new Message() { Sender = user, Content = message, Recipient = friend };
				Messenger.Default.Send<MessagesUI.MessageNewMyMessageWasSent>(new MessagesUI.MessageNewMyMessageWasSent(messages));
			}

			return result;
		}

		public Task<IUser> Login(enSocialNetwork socialNetwork)
		{
			return modService.Login(socialNetwork);
		}

		public Task<IEnumerable<IDialog>> GetDialogs(IUser user)
		{
			return modService.GetDialogs(user, user.SocialNetwork);
		}

		public Task<IDialog> GetDialogWithFriend(IUser user, IUser friend)
		{
			return modService.GetDialogWithFriend(user, user.SocialNetwork, friend);
		}

		public Task<IEnumerable<IUser>> GetUserFriends(IUser user)
		{
			return modService.GetUserFriends(user, user.SocialNetwork);
		}

		public Task<IUser> GetUserInfoRequest(IUser user)
		{
			return modService.GetUserInfoRequest(user, user.SocialNetwork);
		}

		public async Task RegisterInLongPoolServer(IUser user)
		{
			await modService.RegisterInLongPoolServer(user);
		}

		#endregion

		#region Private Methods

		#endregion

		#region Protected Methods

		#endregion
		
	}
}
