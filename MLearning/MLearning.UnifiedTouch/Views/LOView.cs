using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using ObjCRuntime;
using UIKit;
using Foundation; 
using System;
using CoreAnimation;
using CoreGraphics;
using Core.ViewModels;
using MLearning.Core.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ComponentsApp;
using DataSource;
using MLReader;

namespace MLearning.iPhone
{
	[Register("LOView")]
	public class LOView : MvxViewController
	{
		BookDataSource booksource = new BookDataSource();

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			 
			initReader ();
			var backButton = new UIButton (new CGRect (20 , 20, 40, 40));//{BackgroundColor = UIColor.Red} ;
			backButton.SetImage(UIImage.FromFile("MLResources/backbt.png"), UIControlState.Normal);
			View.Add(backButton);
			backButton.TouchUpInside += (sender, e) => {
				var vm1 = ViewModel as LOViewModel ;
				vm1.BackCommand.Execute(null);
			};

		}
 

		GTReaderView readerView;
		void initReader()
		{
			readerView = new GTReaderView ();
			View.Add (readerView);

			var vm = ViewModel as LOViewModel;
			if(vm.LOsInCircle!=null)
				loadLOsInCircle(0);
			vm.PropertyChanged += (sender, e) => {
				if (e.PropertyName == "LOsInCircle")
				{
					if (vm.LOsInCircle != null)
					{
						vm.LOsInCircle.CollectionChanged += LOsInCircle_CollectionChanged;

						LoadPagesDataSource ();

					}
				}
			};




		}

		void LOsInCircle_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			loadLOsInCircle(e.NewStartingIndex);
			//var vm = ViewModel as LOViewModel;
			//menu.SelectElement(vm.LOCurrentIndex);
			//booksource.TemporalColor = Util.GetColorbyIndex(vm.LOCurrentIndex);
		}


		void loadLOsInCircle(int index)
		{
			var vm = ViewModel as LOViewModel;
			if (vm.LOsInCircle != null)
			{

				for (int i = index; i < vm.LOsInCircle.Count; i++)
				{
					ChapterDataSource newchapter = new ChapterDataSource();
					newchapter.Title = vm.LOsInCircle[i].lo.title;
					newchapter.Author = vm.LOsInCircle[i].lo.name + "\n" + vm.LOsInCircle[i].lo.lastname;
					newchapter.Description = vm.LOsInCircle[i].lo.description;
					newchapter.ChapterColor = MLConstants.getUIColor (i % 6 , 255);
					newchapter.TemporalColor = MLConstants.getUIColor (0 , 255);
					// newchapter.BackgroundImage =
					if (vm.LOsInCircle[i].background_bytes != null)
						newchapter.BackgroundImage = MLConstants.BytesToUIImage(vm.LOsInCircle[i].background_bytes);

					vm.LOsInCircle[i].PropertyChanged += (s1, e1) =>
					{
						if (e1.PropertyName == "background_bytes") 
							newchapter.BackgroundImage = MLConstants.BytesToUIImage(
								(s1 as MLearning.Core.ViewModels.LOViewModel.lo_by_circle_wrapper).background_bytes);
							//newchapter.BackgroundImage = MLConstants.BytesToUIImage(vm.LOsInCircle[i].background_bytes); 
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
								page.Name = s_list[j].PagesList[k].page.title;
								page.Description = s_list[j].PagesList[k].page.description;
								page.BorderColor = MLConstants.getUIColor(k%6,255);
								/*********************
								if (s_list[j].PagesList[k].cover_bytes != null)
									page.ImageContent = Constants.ByteArrayToImageConverter.Convert(s_list[j].PagesList[k].cover_bytes);
								s_list[j].PagesList[k].PropertyChanged += (s2, e2) =>
								{
									if (e2.PropertyName == "cover_bytes")
										page.ImageContent = Constants.ByteArrayToImageConverter.Convert((s2 as MLearning.Core.ViewModels.LOViewModel.page_wrapper).cover_bytes);//s_list[j].PagesList[k].cover_bytes);
								}; ****************/
								stack.Pages.Add(page);
							}
							newchapter.Sections.Add(stack);
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
									page.Name = s_list[j].PagesList[k].page.title;
									page.Description = s_list[j].PagesList[k].page.description;
									page.BorderColor = MLConstants.getUIColor(k%6,255);
									/*******************
									if (s_list[j].PagesList[k].cover_bytes != null)
										page.ImageContent = Constants.ByteArrayToImageConverter.Convert(s_list[j].PagesList[k].cover_bytes);
									s_list[j].PagesList[k].PropertyChanged += (s2, e2) =>
									{
										if (e2.PropertyName == "cover_bytes")
											page.ImageContent = Constants.ByteArrayToImageConverter.Convert(s_list[j].PagesList[k].cover_bytes);
									};*********************/
									stack.Pages.Add(page);
								}
								newchapter.Sections.Add(stack);
							}
						};
					}
					booksource.Chapters.Add(newchapter);
				}
				//menu.SelectElement(vm.LOCurrentIndex);
				booksource.TemporalColor = MLConstants.getUIColor (vm.LOCurrentIndex,255);
				readerView.booksource = booksource;
				readerView.InitIndexReader ();
				//_backgroundscroll.Source = booksource;
				//_menucontroller.SEtColor(booksource.Chapters[vm.LOCurrentIndex].ChapterColor);
			}
		}


		List<List<LOPageSource>> pagelistsource = new List<List<LOPageSource>>();

		void LoadPagesDataSource()
		{
			var vm = ViewModel as LOViewModel;
			//var styles = new StyleConstants();
			if (vm.LOsInCircle == null)
				return;
			
			for (int i = 0; i < vm.LOsInCircle.Count; i++)
			{
				var s_list = vm.LOsInCircle[i].stack.StacksList;
				for (int j = 0; j < s_list.Count; j++)
				{
					List<LOPageSource> list_lo = new List<LOPageSource> ();
					for (int k = 0; k < s_list[j].PagesList.Count; k++)
					{
						LOPageSource page = new LOPageSource();
						var content = s_list[j].PagesList[k].content;

						page.Cover = MLConstants.BytesToUIImage (s_list[j].PagesList[k].cover_bytes);
						page.PageIndex = k;
						page.StackIndex = j;
						page.LOIndex = i;
						var slides = s_list[j].PagesList[k].content.lopage.loslide;
						page.Slides = new List<LOSlideSource>();
						for (int m = 0; m < slides.Count; m++)
						{
							LOSlideSource slidesource = new LOSlideSource();
							slidesource.Style = new LOSlideStyle (){ TitleColor = MLConstants.getUIColor (slides [m].lotype, 255) };//[slides[m].lotype];
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
						//pages
						list_lo.Add(page);

					}
					pagelistsource.Add(list_lo);
				}


			}

			//readerView.InitContentReader (0);

			//add pages
			//_readerview.Source = pagelistsource;
			//Canvas.SetZIndex(_readerview, 10);
		}


	}
}