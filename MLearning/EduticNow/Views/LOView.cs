using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using ObjCRuntime;
using UIKit;
using Foundation;
using MLearning.UnifiedTouch.CustomComponents;
using System;
using CoreAnimation;
using CoreGraphics;
using Core.ViewModels;
using MLearning.Core.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using YComponents;
using System.Threading.Tasks;
using YComponents.YWidgets;

namespace MLearning.UnifiedTouch.Views
{
	[Register("LOView")]
	public class LOView : MvxViewController
	{
		string filepath = "efiles/";
		LOViewModel vm;

		YComponents.LoadingView initLoad ;
		public override async void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			View.BackgroundColor = UIColor.White;
			backImage = new UIImageView (new CGRect(0,0,1024,768));
			backImage.Image = UIImage.FromFile (filepath + "backgroundmuro.jpg");
			View.Add (backImage);	

			initLoad = new YComponents.LoadingView (View.Bounds); 
			initLoad.Layer.ZPosition = 100;
			View.Add (initLoad);

			await initView ();
		}

		public override void WillAnimateRotation (UIInterfaceOrientation toInterfaceOrientation, double duration)
		{
			base.WillAnimateRotation (toInterfaceOrientation, duration);

			switch (toInterfaceOrientation) {
			case UIInterfaceOrientation.LandscapeLeft:
				View.Transform = CGAffineTransform.MakeRotation (0);
				View.Frame = new CGRect (0, 0, 1024, 768);
				break;
			case UIInterfaceOrientation.LandscapeRight:
				View.Transform = CGAffineTransform.MakeRotation (0);
				View.Frame = new CGRect (0, 0, 1024, 768);
				break;
			case UIInterfaceOrientation.Portrait:

				View.Transform = CGAffineTransform.MakeTranslation (128, 0); 
				View.Transform = CGAffineTransform.MakeRotation ((nfloat)Math.PI / 2);
				View.Frame = new CGRect (0, 0, 768, 1024);
				break;
			case UIInterfaceOrientation.PortraitUpsideDown:
				View.Transform = CGAffineTransform.MakeTranslation (128, 0);
				View.Transform = CGAffineTransform.MakeRotation ((nfloat)Math.PI/2);
				View.Frame = new CGRect (0, 0, 768, 1024); 
				break; 
			}
		}


		UIImageView backImage ;
		IpadIndexView indexView ;

	

		async Task initView()
		{

			vm = ViewModel as LOViewModel;
			//init data

			await vm.InitLoad();

			indexView = new IpadIndexView ();
			indexView.LOsInCircle = vm.LOsInCircle;
			indexView.PageIndexSelected+= IndexView_PageIndexSelected;
			View.Add (indexView);

			UITapGestureRecognizer tapp = new UITapGestureRecognizer (() => {
				UIView.Animate (0.35, () => {
					readerview.Frame = new CGRect(0,0,1024,768); 
					readerview.ResetPositions();
				});
			});
			indexView.AddGestureRecognizer(tapp);


			addBackButton ();

			loadPagesDataSource ();
			 
			initLoad.Hide ();
		}

		UIButton backButton ;
		void addBackButton()
		{
			backButton = new UIButton (UIButtonType.Custom);
			backButton.Frame = new CGRect (56,40,42,42);
			backButton.SetImage (UIImage.FromFile ("efiles/reader/back.png"), UIControlState.Normal);
			Add (backButton);
			backButton.TouchUpInside+= (sender, e) => {
				vm.BackCommand.Execute(null);
			};
		}
 

		void IndexView_PageIndexSelected (object sender, int section, int index)
		{
			UIView.Animate (0.35, () => {
				readerview.Frame = new CGRect(0,0,1024,768);
				readerview.CurrentPage =  index ;
				readerview.ResetPositions();
				backButton.Hidden = true ;
			});
		}


		List<LOPageSource> pageListSource = new List<LOPageSource>();
		YReaderView readerview ;

		void loadPagesDataSource()
		{
			//var vm = ViewModel as LOViewModel;
			bool white_style = false, is_main = true;
			for (int i = 0; i < vm.LOsInCircle.Count; i++)
			{
				var s_list = vm.LOsInCircle[i].stack.StacksList;
				for (int j = 0; j < s_list.Count; j++)
				{
					for (int k = 0; k < s_list[j].PagesList.Count; k++)
					{
						LOPageSource page = new LOPageSource();
						var content = s_list[j].PagesList[k].content;

						if (s_list [j].PagesList [k].cover_bytes != null)
							page.Cover = WidgetsUtil.ToUIImage (s_list [j].PagesList [k].cover_bytes);
 

						page.PageIndex = k;
						page.StackIndex = j;
						page.LOIndex = i;

						var slides = s_list[j].PagesList[k].content.lopage.loslide;

						page.Slides = new List<LOSlideSource>();

						for (int m = 0; m < slides.Count; m++)
						{
							LOSlideSource slidesource = new LOSlideSource();
							var _id = vm.LOsInCircle [i].lo.color_id;

							//slidesource.Style = getSlideStyle (white_style, is_main, _id, slides[m].lotype);
							//slidesource.Style.ID = _id + 1;
							is_main = !is_main;

							if (slides[m].lotype != 0 && slides[m].lotype != 6)
								white_style = !white_style;
							slidesource.Type = slides[m].lotype;
							if (slides[m].lotitle != null) slidesource.Title = slides[m].lotitle;
							if (slides[m].loparagraph != null) slidesource.Paragraph = slides[m].loparagraph;
							if (slides[m].loimage != null) slidesource.ImageUrl = slides[m].loimage;
							if (slides[m].lotext != null) slidesource.Paragraph = slides[m].lotext;
							if (slides[m].loauthor != null) slidesource.Author = slides[m].loauthor;
							if (slides[m].lovideo != null) slidesource.VideoUrl = slides[m].lovideo;

							//slidesource.Color = WidgetsUtil.themes[ vm.LOsInCircle [i].lo.color_id % 6];
							slidesource.Color = WidgetsUtil.themes[j%6] ;

							if (slides[m].loitemize != null)
							{
								slidesource.Itemize = new ObservableCollection<LOItemSource>();
								var items = slides[m].loitemize.loitem;
								for (int n = 0; n < items.Count; n++)
								{
									LOItemSource item = new LOItemSource();
									if (items[n].loimage != null) item.ImageUrl = items[n].loimage;
									//if (items[n].lotext != null) item.Text = items[n].lotext;  @todo : comente esto :S
									slidesource.Itemize.Add(item);
								}
							}
							page.Slides.Add(slidesource);
						} 
						pageListSource.Add(page);
					}
				}
			}
			//readerView.Source = pageListSource;

			readerview = new YReaderView (new CGRect (1024,0,1024,768));
			readerview.PagesList = pageListSource;
			View.Add (readerview); 

			readerview.BackButton.TouchUpInside+= (sender, e) => {
				UIView.Animate (0.35, () => {
					readerview.Frame = new CGRect(1024,0,1024,768);
				}, ()=>{
					backButton.Hidden = false ;
				});
			};
		}
		 






			
		/**
		//data source
		BookDataSource bookSource;

		//itemsScroll
		IGroupList lo_list;

		//top menu
		LOMenuController menuController;

		//background images
		ControlScrollView backgroundScroll;

		//bottom menu
		ControlDownMenu menu;

		//to load items just once
		bool needsLoading;

		//items
		List <IStackItem> stackItems;

		//just one view should be opened
		bool viewWasTouched;

		//data source for the reader
		List<LOPageSource> pageListSource;

		//reader view
		LOReaderScroll readerView;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// ios7 layout
			if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
				EdgesForExtendedLayout = UIRectEdge.None;

			needsLoading = true;
			viewWasTouched = false;
			stackItems = new List<IStackItem> ();
			pageListSource = new List<LOPageSource> ();
			initBackground ();
			init ();
			initReader ();

		}

		#region Initialization
		private void initBackground ()
		{
			backgroundScroll = new ControlScrollView ();
			Add (backgroundScroll);

			menuController = new LOMenuController ();
			menuController.Layer.ZPosition = 100;
			Add (menuController);
			menuController.CloseViewController += (object sender) => 
			{ 
				var vm = ViewModel as LOViewModel;
				vm.BackCommand.Execute (null);
			};
		}

		private void init ()
		{
			bookSource = new BookDataSource ();

			lo_list = new IGroupList ();
			lo_list.StackItemCreated += HandleStackItemCreated;
			Add (lo_list);

			menu = new ControlDownMenu();
			menu.Layer.ZPosition = 100;
			Add(menu);
			menu.ControlDownElementSelected += HandleControlDownElementSelected;

			loadLOsInCircle (0);
			menu.Source = bookSource;
			lo_list.Source = bookSource;
			var vm = this.ViewModel as LOViewModel;

			menuController.setColor (bookSource.Chapters [vm.LOCurrentIndex].ChapterColor);
			menu.SelectElement (vm.LOCurrentIndex);
			lo_list.animateToChapter (vm.LOCurrentIndex, true);
			backgroundScroll.setToIndex(vm.LOCurrentIndex);

			vm.PropertyChanged += vm_PropertyChanged;
			lo_list.ChapterHasChanged += HandleChapterHasChanged;
			lo_list.StackItemFullAnimationStarted += HandleStackItemFullAnimationStarted;
			lo_list.StackItemFullAnimationCompleted += HandleStackItemFullAnimationCompleted;
			lo_list.StackItemThumbAnimationStarted += HandleStackItemThumbAnimationStarted;
			lo_list.StackItemThumbAnimationCompleted += HandleStackItemThumbAnimationCompleted;
		}

		void initReader()
		{
			readerView = new LOReaderScroll (stackItems);
			readerView.Layer.ZPosition = 100;
			readerView.ItemHasChanged += HandleItemHasChanged;
		}
		#endregion
	
		#region Items Scroll Events
		void HandleStackItemCreated (object sender, int index)
		{
			stackItems.Insert (index, sender as IStackItem);
		}

		void HandleStackItemThumbAnimationCompleted (object sender, int chapter, int section, int page)
		{
			viewWasTouched = false;

			//shouldContinue allows moveToFullScreenAnimation in an item (when 2 items are touched, one should not continue growing
			foreach (var item in stackItems)
				item.ShouldContinue = true;
		}

		void HandleStackItemThumbAnimationStarted (object sender, int chapter, int section, int page)
		{
		}

		void HandleStackItemFullAnimationCompleted (object sender, int chapter, int section, int page)
		{
			if (!LOReaderScroll.IsVisible) 
			{
				readerView.SetContentOffset (new CGPoint (lo_list.SelectedStackItem.IndexInBook * Constants.DeviceWidth, 0), false);
				readerView.SetVisible ();
				Add (readerView);
				View.BringSubviewToFront (readerView);

				//adding controllers for readerView
				Add (readerView.BackArrow);
			}
		}


		void HandleStackItemFullAnimationStarted (object sender, int chapter, int section, int page)
		{
			var stackItem = sender as IStackItem;
			//if readerView is not visible, the second item touched must show its information
			if (viewWasTouched && !LOReaderScroll.IsVisible) 
			{
				stackItem.ShouldContinue = false;
				stackItem.showInfo ();
			}
			viewWasTouched = true;

			//data source is loaded just once
			if (needsLoading) 
				loadPagesDataSource ();

			needsLoading = false;
		}

		void HandleChapterHasChanged (object sender, int chapter)
		{
			menu.SelectElement (chapter);
			HandleControlDownElementSelected (sender, chapter);
		}

		#endregion
			
		#region PropertyChanged
		void vm_PropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			var vm = this.ViewModel as LOViewModel;

			if (e.PropertyName == "LOsInCircle") 
				if (vm.LOsInCircle != null)
					vm.LOsInCircle.CollectionChanged += LOsInCircle_CollectionChanged;
		}

		void LOsInCircle_CollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			loadLOsInCircle (e.NewStartingIndex);
			var vm = this.ViewModel as LOViewModel;
			menu.SelectElement (vm.LOCurrentIndex);
			bookSource.TemporalColor = Constants.GetColorByIndex (vm.LOCurrentIndex);
		}
		#endregion

		#region Loading Data Sources
		void loadLOsInCircle (int index)
		{
			var vm = this.ViewModel as LOViewModel;
			if (vm.LOsInCircle != null) 
			{
				bookSource.TemporalColor = Constants.GetColorByIndex (0);
				for (int i = index; i < vm.LOsInCircle.Count; i++) 
				{
					var newChapter = new ChapterDataSource ();
					newChapter.Title = vm.LOsInCircle [i].lo.title;
					newChapter.Author = vm.LOsInCircle [i].lo.name + " " + vm.LOsInCircle [i].lo.lastname;
					newChapter.Description = vm.LOsInCircle [i].lo.description;

					newChapter.ChapterColor = Constants.GetColorByIndex(i % 6);
					newChapter.TemporalColor = bookSource.TemporalColor;

					if (vm.LOsInCircle[i].background_bytes != null)
						newChapter.BackgroundImage = UIImage.LoadFromData ((NSData.FromArray (vm.LOsInCircle[i].background_bytes)));

					vm.LOsInCircle[i].PropertyChanged += (s1, e1) =>
					{
						if (e1.PropertyName == "background_bytes")
						{
							newChapter.BackgroundImage = UIImage.LoadFromData ((NSData.FromArray (vm.LOsInCircle[i].background_bytes)));
						}
					};

					//loading the stacks
					if (vm.LOsInCircle[i].stack.IsLoaded)
					{
						var s_list = vm.LOsInCircle[i].stack.StacksList;
						for (int j = 0; j < s_list.Count; j++)
						{
							SectionDataSource stack = new SectionDataSource();
							stack.Name = s_list[j].TagName;

							for (int k = 0; k < s_list[j].PagesList.Count; k++)
							{
								var page = new PageDataSource();
								page.BorderColor = newChapter.ChapterColor;
								page.Name = s_list[j].PagesList[k].page.title;
								page.Description = s_list[j].PagesList[k].page.description;
								if (s_list[j].PagesList[k].cover_bytes != null)
									page.ImageContent = UIImage.LoadFromData ((NSData.FromArray (s_list[j].PagesList[k].cover_bytes)));
																	
								s_list[j].PagesList[k].PropertyChanged += (s2, e2) =>
								{
									if (e2.PropertyName == "cover_bytes")
										page.ImageContent = UIImage.LoadFromData ((NSData.FromArray ((s2 as MLearning.Core.ViewModels.LOViewModel.page_wrapper).cover_bytes)));
								};
								stack.Pages.Add(page);
							}
							stack.SectionColor = newChapter.ChapterColor;

							newChapter.Sections.Add(stack);
						}

					}
					else
					{

						vm.LOsInCircle[i].stack.PropertyChanged += (s3, e3) =>
						{
							var s_list = vm.LOsInCircle[i].stack.StacksList;
							for (int j = 0; j < s_list.Count; j++)
							{
								SectionDataSource stack = new SectionDataSource();

								stack.Name = s_list[j].TagName;
								for (int k = 0; k < s_list[j].PagesList.Count; k++)
								{
									PageDataSource page = new PageDataSource();
									page.BorderColor = newChapter.ChapterColor;
									page.Name = s_list[j].PagesList[k].page.title;
									page.Description = s_list[j].PagesList[k].page.description;
									if (s_list[j].PagesList[k].cover_bytes != null)
										page.ImageContent = UIImage.LoadFromData ((NSData.FromArray (s_list[j].PagesList[k].cover_bytes)));

									s_list[j].PagesList[k].PropertyChanged += (s2, e2) =>
									{
										if (e2.PropertyName == "cover_bytes")
											page.ImageContent = UIImage.LoadFromData ((NSData.FromArray (s_list[j].PagesList[k].cover_bytes)));
									};
									stack.Pages.Add(page);
								}
								stack.SectionColor = newChapter.ChapterColor;

								newChapter.Sections.Add(stack);
							}
						};
					}
					//LoadStack's end
					bookSource.Chapters.Add (newChapter);
				}
					
				backgroundScroll.Source = bookSource;

			}
		}


		void loadPagesDataSource()
		{
			var vm = ViewModel as LOViewModel;
			bool white_style = false, is_main = true;
			for (int i = 0; i < vm.LOsInCircle.Count; i++)
			{
				var s_list = vm.LOsInCircle[i].stack.StacksList;
				for (int j = 0; j < s_list.Count; j++)
				{
					for (int k = 0; k < s_list[j].PagesList.Count; k++)
					{
						LOPageSource page = new LOPageSource();
						var content = s_list[j].PagesList[k].content;

						//if (s_list [j].PagesList [k].cover_bytes != null)
						//	page.CoverBytes = s_list [j].PagesList [k].cover_bytes;

						var currentPage = s_list [j].PagesList [k];
						currentPage.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => {
							//if (e.PropertyName == "cover_bytes")
							//	page.CoverBytes = currentPage.cover_bytes;
						};

						page.PageIndex = k;
						page.StackIndex = j;
						page.LOIndex = i;

						var slides = s_list[j].PagesList[k].content.lopage.loslide;

						page.Slides = new List<LOSlideSource>();

						for (int m = 0; m < slides.Count; m++)
						{
							LOSlideSource slidesource = new LOSlideSource();
							var _id = vm.LOsInCircle [i].lo.color_id;

							slidesource.Style = getSlideStyle (white_style, is_main, _id, slides[m].lotype);
							slidesource.Style.ID = _id + 1;
							is_main = !is_main;

							if (slides[m].lotype != 0 && slides[m].lotype != 6)
								white_style = !white_style;
							slidesource.Type = slides[m].lotype;
							if (slides[m].lotitle != null) slidesource.Title = slides[m].lotitle;
							if (slides[m].loparagraph != null) slidesource.Paragraph = slides[m].loparagraph;
							if (slides[m].loimage != null) slidesource.ImageUrl = slides[m].loimage;
							if (slides[m].lotext != null) slidesource.Paragraph = slides[m].lotext;
							if (slides[m].loauthor != null) slidesource.Author = slides[m].loauthor;
							if (slides[m].lovideo != null) slidesource.VideoUrl = slides[m].lovideo;

							if (slides[m].loitemize != null)
							{
								slidesource.Itemize = new ObservableCollection<LOItemSource>();
								var items = slides[m].loitemize.loitem;
								for (int n = 0; n < items.Count; n++)
								{
									LOItemSource item = new LOItemSource();
									if (items[n].loimage != null) item.ImageUrl = items[n].loimage;
									if (items[n].lotext != null) item.Text = items[n].lotext;
									slidesource.Itemize.Add(item);
								}
							}
							page.Slides.Add(slidesource);
						} 
						pageListSource.Add(page);
					}
				}
			}
			readerView.Source = pageListSource;


		}
		#endregion

		#region Extra Events
		void HandleControlDownElementSelected(object sender, int index)
		{
			backgroundScroll.setToIndex(index);
			menuController.animateToColor(bookSource.Chapters[index].ChapterColor);
			if(sender.GetType() == typeof (ControlDownMenu) || sender.GetType () == typeof (IGroupList))
				lo_list.animateToChapter (index, true);
			else
				lo_list.animateToChapter (index, false);

			var vm = ViewModel as LOViewModel;
			vm.LOCurrentIndex = index;
		}

		void HandleItemHasChanged (object sender, int prevChapter, int chapter, int section, int page)
		{
			lo_list.SetToItem (chapter, section, page);
			if(prevChapter != chapter)
				HandleChapterHasChanged (sender, chapter);

			lo_list.ScrollToShowItem ();
		}
		#endregion

		#region Styles
		LOSlideStyle getSlideStyle(bool iswhite,bool ismain, int colorid, int type)
		{ 
			LOSlideStyle style = new LOSlideStyle();
			//default
			style.BorderColor = Constants.GetSecondColorByIndex(colorid); 
			style.ContentColor = UIColor.Black ;

			if (ismain) style.TitleColor = Constants.GetColorByIndex(colorid); else style.TitleColor = Constants.GetSecondColorByIndex(colorid);
			if (ismain) style.ColorNumber = 1; else style.ColorNumber = 2;
			style.Background = UIColor.White;
			if (!iswhite)
			{
				if (ismain) style.Background = Constants.GetColorByIndex(colorid); else style.Background = Constants.GetSecondColorByIndex(colorid);
				if (type != 0) style.TitleColor = UIColor.White;
				style.ColorNumber = 0;
				if (type == 6) style.BorderColor = UIColor.White;
			}

			//especial cases
			if (type == 0 || type == 6) style.ContentColor = UIColor.White;
			if (type == 6) style.TitleColor = UIColor.White;
			if (type == 5) style.TitleColor = UIColor.Black;


			return style;
		}
		#endregion

		*/
	}
}