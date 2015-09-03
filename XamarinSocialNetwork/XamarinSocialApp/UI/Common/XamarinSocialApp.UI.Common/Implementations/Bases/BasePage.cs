using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinSocialApp.Services.UI.Interfaces.NavigationService;
using XamarinSocialApp.UI.Common.Interfaces.Views.Bases;

namespace XamarinSocialApp.UI.Common.Implementations.Bases
{
	//[ContentProperty("InternalContent")]
	public class BasePage : ContentPage, IBasePage
	{
		#region Fields

		private ContentView modInternalContainer;
		private ContentView modCentralPanel;
		private ContentView modToolbarPanel;
		private INavigationServiceCommon mvNavigationService;

		#endregion

		#region Bindables

		public static readonly BindableProperty OnNavigateBackCommandProperty =
			BindableProperty.Create<BasePage, ICommand>(x => x.OnNavigateBackCommand, default(ICommand));

		public static readonly BindableProperty BackButtonIsHiddenProperty =
			BindableProperty.Create<BasePage, bool>(x => x.BackButtonIsHidden, default(bool));

		#endregion

		#region Properties

		public bool BackButtonIsHidden
		{
			get
			{
				return (bool)GetValue(BackButtonIsHiddenProperty);
			}

			set
			{
				SetValue(BackButtonIsHiddenProperty, value);
			}
		}

		public ICommand OnNavigateBackCommand
		{
			get
			{
				return (ICommand)GetValue(OnNavigateBackCommandProperty);
			}

			set
			{
				SetValue(OnNavigateBackCommandProperty, value);
			}
		}

		public View InternalContent
		{
			get
			{
				return modInternalContainer.Content;
			}

			set
			{
				modInternalContainer.Content = value;
			}
		}

		public View CentralPanelContent
		{
			get
			{
				return modCentralPanel.Content;
			}

			set
			{
				modCentralPanel.Content = value;
			}
		}

		public View Toolbar
		{
			get
			{
				return modToolbarPanel.Content;
			}

			set
			{
				modToolbarPanel.Content = value;
			}
		}

		public INavigationServiceCommon NavigationService
		{
			get
			{
				return mvNavigationService ?? (mvNavigationService = ServiceLocator.Current.GetInstance<INavigationServiceCommon>());
			}
		}

		#endregion

		#region Constructors

		public BasePage()
		{
			//InitContent();
			NavigationPage.SetHasNavigationBar(this, false);
		}

		void InitContent()
		{
			//BackgroundColor = Color.FromHex("#FFA4BBC1");
			Padding = 0;

			#region Content grid

			var contentGrid = new Grid
			{
				RowDefinitions =
				{
					new RowDefinition { Height = 50d },
					new RowDefinition()
				}
			};

			#region Main content controller

			modInternalContainer = new ContentView();
			Grid.SetRow(modInternalContainer, 1);
			contentGrid.Children.Add(modInternalContainer);

			#endregion

			#region Panel row

			var isBackButtonEnabled = Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.iOS;

			//var backButton = new ImageButton
			//{
			//	ImageHeight = 45,
			//	ImageWidth = 45,
			//	HasNoText = true,
			//	IsVisible = isBackButtonEnabled,
			//	IsEnabled = isBackButtonEnabled,
			//	VerticalOptions = LayoutOptions.CenterAndExpand,
			//	Image = IconExtension.LoadIcon(IconNames.csBackButton),
			//};

			//backButton.SetBinding(GridHideableChildBehavior.IsHiddenProperty, new Binding(BackButtonIsHiddenProperty.PropertyName, source: this));
			//backButton.Clicked += OnBackClicked;

			//var pageIconImage = new Image
			//{
			//	VerticalOptions = LayoutOptions.CenterAndExpand
			//};
			//pageIconImage.SetBinding(Image.SourceProperty,
			//	new Binding(
			//		IconProperty.PropertyName,
			//		source: this,
			//		converter: new UniversalConverter<ImageSource, ImageSource>(ims => ims ?? IconExtension.LoadIcon(IconNames.csAppIcon))));
			//Grid.SetColumn(pageIconImage, 1);

			var pageTitleLabel = new Label
			{
				VerticalOptions = LayoutOptions.CenterAndExpand,
				TextColor = Color.White,
				FontSize = 20d
			};
			Grid.SetColumn(pageTitleLabel, 2);
			pageTitleLabel.SetBinding(Label.TextProperty, new Binding(TitleProperty.PropertyName, source: this));

			modCentralPanel = new ContentView
			{
				VerticalOptions = LayoutOptions.CenterAndExpand
			};
			Grid.SetColumn(modCentralPanel, 3);

			modToolbarPanel = new ContentView
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.End
			};
			Grid.SetColumn(modToolbarPanel, 4);

			var panelGrid = new Grid
			{
				//BackgroundColor = Color.FromHex("#FF81A2AA"),
				ColumnDefinitions =
				{
					new ColumnDefinition
					{
						Width = isBackButtonEnabled ? GridLength.Auto : 0
					},
					new ColumnDefinition { Width = GridLength.Auto },
					new ColumnDefinition { Width = GridLength.Auto },
					new ColumnDefinition
					{
						Width = new OnIdiom<GridLength>
						{
							Phone = 0,
							Tablet = GridLength.Auto
						}
					}
				},

				Children =
				{
					//backButton,
					//pageIconImage,
					pageTitleLabel,
					modCentralPanel,
					modToolbarPanel
				}
			};

			contentGrid.Children.Add(panelGrid);

			#endregion

			//Content = contentGrid;

			#endregion
		}

		#endregion

		#region Protected methods

		protected bool InvokeContentPageBackButtonPressed()
		{
			//GoBackAsync();
			return base.OnBackButtonPressed();
		}

		protected virtual async Task PostInitizlizationProtected()
		{

		}

		#endregion

		#region Overriden

		protected override bool OnBackButtonPressed()
		{
			//GoBackAsync();
			return false;
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();

			await Task.Run(async () => await NavigationService.OnAppearing(this.BindingContext));
		}

		protected async override void OnDisappearing()
		{
			await Task.Run(async () => await NavigationService.OnNavigatedFromAsync(this.BindingContext));
		}

		#endregion

		#region Private methods

		//async void GoBackAsync()
		//{
		//	if (Device.OS == TargetPlatform.Windows)
		//	{
		//		if (OnNavigateBackCommand is CommandBase)
		//		{
		//			var commandBase = (CommandBase)OnNavigateBackCommand;

		//			commandBase.PostCommandAsyncAction = async () =>
		//			{
		//				await NavigationService.NavigateBackAsync();
		//				commandBase.PostCommandAsyncAction = null;
		//			};
		//		}
		//		else
		//		{
		//			await NavigationService.NavigateBackAsync();
		//		}
		//	}

		//	OnNavigateBackCommand.With(x => x.Execute(null));
		//}

		void OnBackClicked(object sender, EventArgs e)
		{
			OnBackButtonPressed();
		}

		#endregion

		#region Public Methods

		public async Task PostInitizlization()
		{
			Device.BeginInvokeOnMainThread(async () => { await PostInitizlizationProtected(); });
		}

		#endregion


	}
}
