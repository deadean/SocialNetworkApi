﻿using Common.MVVM.Library;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XamarinSocialApp.Data.Interfaces.Entities.Database;
using XamarinSocialApp.Services.UI.Interfaces.Web;
using XamarinSocialApp.UI.Common.Implementations.Bases;
using XamarinSocialApp.UI.Common.Interfaces.ViewModels;
using XamarinSocialApp.UI.Data.Implementations.Entities.Databases;
using XamarinSocialApp.UI.Data.Implementations.Navigation;
using MessagesUI = XamarinSocialApp.UI.Data.Implementations.Messages.Messages;

namespace XamarinSocialApp.UI.Common.VVm.Implementations.ViewModels
{
	public class PageDialogWithFriendVm : AdvancedPageViewModelBase, IPageDialogWithFriendVm
	{

		#region Fields

		private readonly IApplicationWebService modIWebService;
		private IUser modUser;
		private IUser modFriend;
		private string mvMessage;

		#endregion

		#region Commands

		public AsyncCommand SendMessageCommand
		{
			get;
			private set;
		}

		#endregion

		#region Properties

		public ObservableCollection<MessageVm> Messages { get; set; }

		public string Message
		{
			get
			{
				return mvMessage;
			}

			set
			{
				mvMessage = value;
				this.OnPropertyChanged();
				this.RefreshCommands();
			}
		}
		

		#endregion

		#region Ctor

		public PageDialogWithFriendVm()
			:this(ServiceLocator.Current.GetInstance<IApplicationWebService>())
		{
		}

		[PreferredConstructor]
		public PageDialogWithFriendVm(IApplicationWebService iWebService)
		{
			modIWebService = iWebService;
			SendMessageCommand = new AsyncCommand(OnSendMessageCommand, () => { return !String.IsNullOrWhiteSpace(Message); });
		}
		
		#endregion

		#region Public Methods

		#endregion

		#region Private Methods

		private void OnNewMessageWasSentToMe(MessagesUI.MessageNewMessageWasSentToMe msg)
		{
			try
			{
				var message = new Message() 
				{ 
					Sender = msg.Sender.Sender, 
					Content = msg.Sender.Content, 
					Recipient = msg.Sender.Recipient 
				};

				this.Messages.Insert(0, new MessageVm(message) { Name = msg.Sender.Sender.Uid == modUser.Uid ?
																												 String.Format("{0} {1}", modUser.FirstName, modUser.LastName) :
																												 String.Format("{0} {1}", modFriend.FirstName, modFriend.LastName) });
			}
			catch (Exception ex)
			{
			}
		}

		private void OnMessageNewMyMessageWasSent(MessagesUI.MessageNewMyMessageWasSent msg)
		{
			try
			{
				var message = new Message()
				{
					Sender = msg.Sender.Sender,
					Content = msg.Sender.Content,
					Recipient = msg.Sender.Recipient
				};

				this.Messages.Insert(0, new MessageVm(message) { Name = msg.Sender.Sender.Uid == modUser.Uid ?
																												 String.Format("{0} {1}", modUser.FirstName, modUser.LastName) :
																												 String.Format("{0} {1}", modFriend.FirstName, modFriend.LastName) });
			}
			catch (Exception ex)
			{

			}
		}
		
		#endregion

		#region Protected Methods

		public override async Task OnNavigatedTo(object navigationParameter)
		{
			PageDialogWithFriendNavParams param = navigationParameter as PageDialogWithFriendNavParams;
			if (param.HasNotValue())
				return;

			modUser = param.User;

			if (modUser.HasNotValue())
				return;

			modFriend = param.Friend;

			IsBusy = true;

			IDialog dialog = await modIWebService.GetDialogWithFriend(modUser, modFriend);
			this.Messages = new ObservableCollection<MessageVm>(dialog.Messages.Select(x => new MessageVm(x)));
			this.OnPropertyChanged(x => x.Messages);

			Messenger.Default.Register<MessagesUI.MessageNewMessageWasSentToMe>(this, OnNewMessageWasSentToMe);
			Messenger.Default.Register<MessagesUI.MessageNewMyMessageWasSent>(this, OnMessageNewMyMessageWasSent);

			IsBusy = false;
		}

		public override void RefreshCommands()
		{
			this.SendMessageCommand.RaiseCanExecuteChanged();
		}

		#endregion

		#region Command Execute Handlers

		private async Task OnSendMessageCommand()
		{
			try
			{
				bool isSent = await modIWebService.SendMessage(modUser, modFriend, Message);
				var message = new Message() { Sender = modUser, Content = Message, Recipient = modFriend };


			}
			catch (Exception ex)
			{

			}
		}

		#endregion

	}
}
