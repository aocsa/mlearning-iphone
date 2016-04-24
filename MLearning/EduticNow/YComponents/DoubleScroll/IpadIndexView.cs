using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;
using System.Collections.ObjectModel;
using MLearning.Core.ViewModels;
using YComponents.YWidgets;
using System.Text.RegularExpressions;

namespace YComponents
{ 

	public delegate void PageIndexSelectedEventHandler(object sender, int section, int PageIndexView);

	/// <summary>
	/// Ipad index view.
	/// Vista para el indice del ModuloLector
	/// </summary>
	public class IpadIndexView : UIView
	{
		GTParallaxScroll pscroll  ;
		public event PageIndexSelectedEventHandler PageIndexSelected;
		public IpadIndexView ()
		{
			Frame = new CGRect (0, 0, 1024, 768);

			//initMainView ();
			//initBackView ();
			pscroll = new GTParallaxScroll (0, 0, 1024, 768, GTScrollOrientation.Horizontal);
			//pscroll.SetContent (mainView, mainWidth);
			//pscroll.SetBackContent (backView, backWidth);
			Add (pscroll);
		}
 

		ObservableCollection<LOViewModel.lo_by_circle_wrapper> _losInCircle;
		public ObservableCollection<LOViewModel.lo_by_circle_wrapper> LOsInCircle
		{
			get { return _losInCircle; }
			set 
			{
				_losInCircle = value; 
				if (_losInCircle != null) {
					loadLOsInCircle (0);
					_losInCircle.CollectionChanged+= _losInCircle_CollectionChanged;
				}
			}
		}

		void _losInCircle_CollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			loadLOsInCircle (e.NewStartingIndex);	
		}


		UIView mainView, backView  ; 
		nfloat mainWidth, backWidth ;

		List<PageIndexView> indexList = new List<PageIndexView>();

		void loadLOsInCircle(int index)
		{
			int numberofPages = 0, numberofLOs = 0, counter = 1;
			nfloat lowidth =  660 , iwidth = 390 , pos = 0;
			mainView = new UIView (new CGRect (0, 0, lowidth * 3 +iwidth * 4*3, 768));

			nfloat lowidth2 =  660/2 , iwidth2 = 390/2 , pos2 = 0;
			backView = new UIView (new CGRect(0,0,(lowidth2 + 4* iwidth2) *3,768));

			numberofLOs = LOsInCircle.Count;
			for (int i = index; i < numberofLOs; i++) {

				var loNameLabel = new UILabel (new CGRect(pos + 90 , 170, 420 , 180));
				loNameLabel.Lines = 2;
				loNameLabel.LineBreakMode = UILineBreakMode.WordWrap;
				loNameLabel.TextColor = UIColor.White;
				loNameLabel.Font = UIFont.FromName ("HelveticaNeue", 46);

				loNameLabel.Text = LOsInCircle [i].lo.title;
				mainView.Add (loNameLabel);

				pos += lowidth; 

				UIImageView image = new UIImageView (new CGRect (pos2,0,1366,768));
				//image.Image = UIImage.FromFile ("efiles/fondologin.png"); 

				if(LOsInCircle[i].background_bytes !=null)
					image.Image = WidgetsUtil.ToUIImage( LOsInCircle[i] .background_bytes);
				image.ContentMode = UIViewContentMode.ScaleAspectFill;
				backView.Add (image);
				pos2 += lowidth2;
					
				LOsInCircle[i].PropertyChanged+= (sender, e) => {
					if(e.PropertyName=="background_bytes")
					{
						image.Image = WidgetsUtil.ToUIImage((sender as LOViewModel.lo_by_circle_wrapper).background_bytes);
					}
				};

				UIColor locolor = WidgetsUtil.themes[ LOsInCircle [i].lo.color_id % 6];


				for (int j = 0; j < LOsInCircle[i].stack.StacksList.Count; j++) {
					var stack = LOsInCircle [i].stack.StacksList [j];
					numberofPages += stack.PagesList.Count; 

 

					for (int k = 0; k < stack.PagesList.Count; k++) {
						var page_source = stack.PagesList [k];
						var page = new PageIndexView (pos, 0);
						page.Number = j;
						page.Section = i;
						page.Number = indexList.Count;
						page.DescriptionText = GetPlainTextFromHtml(page_source.page.description);
						page.setName (page_source.page.title, WidgetsUtil.themes[j%6]);
						page.setNumber (counter+"", WidgetsUtil.themes[j%6]);
						page.PageIndexTapped += Page_PageIndexTapped;

						indexList.Add (page);
						mainView.Add (page);
						pos += iwidth; 
						pos2 += iwidth2;
						counter++;
					} 
				}
				 
				counter = 1;
			}

			backWidth = pos2 + 100;
			mainWidth = lowidth *  numberofLOs +iwidth * numberofPages  + 200;
			//mainView.Frame = new CGRect (0,0, mainWidth , 768);
			pscroll.SetContent (mainView, mainWidth);
			pscroll.SetBackContent (backView,backWidth);
		}

		private string GetPlainTextFromHtml(string htmlString)
		{
			string htmlTagPattern = "<.*?>";
			var regexCss = new Regex("(\\<script(.+?)\\</script\\>)|(\\<style(.+?)\\</style\\>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
			htmlString = regexCss.Replace(htmlString, string.Empty);
			htmlString = Regex.Replace(htmlString, htmlTagPattern, string.Empty);
			htmlString = Regex.Replace(htmlString, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
			htmlString = htmlString.Replace("&nbsp;", string.Empty);

			return htmlString;
		}

		//HTML
		public string RemoveHTMLTags(string HTMLCode)
		{
			var fp = System.Text.RegularExpressions.Regex.Replace(HTMLCode, "<[^>]*>", "");
			return System.Text.RegularExpressions.Regex.Replace(fp, "&", " ");
		}
		 

		void Page_PageIndexTapped (object sender, int idx)
		{
			PageIndexView piv = sender as PageIndexView;

			if (PageIndexSelected != null)
				PageIndexSelected (this, piv.Section, piv.Index);
		}


 
		void initMainView()
		{
			//static 
			nfloat lowidth =  660 , iwidth = 390 , pos = 0;
			mainView = new UIView (new CGRect (0, 0, lowidth * 3 +iwidth * 4*3, 768));

			for (int i = 0; i < 3; i++) { 



				pos += lowidth;
				for (int j = 0; j < 4; j++) { 
					var page = new PageIndexView (pos, 0);
					page.Index = j;
					page.Section = i;
					page.PageIndexTapped += delegate(object sender, int idx) {
						int b = (sender as PageIndexView).Section ;
					}; 
					//page.Alpha = (nfloat)0.5;
					for (int k = 0; k < 7; k++)
						page.addSlideButton (new UISlideButton (40 * k, 0, 36, 36){ ImageUrl = "assets/icons/contenido" + (k + 1) + ".png" }); 
					mainView.Add (page);
					pos += iwidth;
				} 
			}
  
			mainWidth = lowidth * 3 +iwidth * 4*3;


		}



 

		void initBackView()
		{
			nfloat lowidth =  660/2 , iwidth = 390/2 , pos = 0/2;
			backView = new UIView (new CGRect(0,0,(lowidth + 4* iwidth) *3,768));


			for (int i = 0; i < 3; i++) { 
				UIImageView image = new UIImageView (new CGRect (pos,0,1366,768));
				image.Image = UIImage.FromFile ("assets/00"+(i+1)+"fondo.png");
				image.ContentMode = UIViewContentMode.ScaleAspectFit;
				backView.Add (image);
				pos += (lowidth + 4* iwidth);
			}

			//backView.Frame = new CGRect (0,0,pos,768);
			backWidth = pos;
		}
	}
}

