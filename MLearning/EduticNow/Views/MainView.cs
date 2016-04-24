using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using ObjCRuntime;
using UIKit;
using Foundation;
using CoreAnimation;
using CoreGraphics;
using CoreImage;
using MLearning.Core.ViewModels;
using Cirrious.MvvmCross.Binding.Touch.Views;
using System;
using System.Collections.Generic;
using MLearning.UnifiedTouch.CustomComponents;
using YComponents.YWidgets;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using Core.Repositories;
using Cirrious.CrossCore;

namespace MLearning.UnifiedTouch.Views
{
	[Register("MainView")]
	public class MainView : MvxViewController
	{

		FloatingView floatView ;
		MainViewModel vm ;
		int SelectedLOIndex ;
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			vm = ViewModel as MainViewModel;

			NavigationController.NavigationBarHidden = true; 

			DoLogoutFacebook ();
			setOrientation ();

			initBackgroundImage ();
			initComponent ();
			bindData ();

		}

 		  




		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (false);
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


		void setOrientation()
		{

			var toInterfaceOrientation = InterfaceOrientation;
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



		void initComponent()
		{
			floatView = new FloatingView ();
			View.Add (floatView);
			floatView.Navigate2Reader+= FloatView_Navigate2Reader;
			floatView.Navigate2ReaderStarted += (object sender) => {
				UIView.Animate(0.35,()=>{
					backgroundImage.Alpha = 0 ;
					waitImage.Alpha =1 ;
				});
			};
 
			floatView.backCenterImage.Image = UIImage.FromFile ("efiles/fondologin.jpg");

			UITapGestureRecognizer dologout = new UITapGestureRecognizer (() => {
				vm.LogoutCommand.Execute(null);
			});
			floatView.doAskButton.AddGestureRecognizer( dologout);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			backgroundImage.Alpha = 1 ;
			waitImage.Alpha =1 ;
			floatView.leftMenuView.Alpha = 1;

			floatView.CloseContent ();

			if(navigateLoad!=null)
			navigateLoad.Hide ();
		}

		void bindData()
		{
			setUserName ();

			if (vm.CirclesList != null)
				linkCourses ();

			if (vm.LearningOjectsList != null)
				linkLOList ();

			if (vm.PostsList != null)
				linkPostList ();

			vm.PropertyChanged+= (sender, e) => {
				string property = e.PropertyName;
				switch (property)
				{
				case "UserFirstName": 
					setUserName();
					break;
				case "UserLastName":
					setUserName();
					break;

				case "UserImage":
					setUserImage();
					break;
				case "UsersList":
					linkUserList();
					break;

				case "CirclesList":
					linkCourses();
					break;
				case "PostsList":
					linkPostList();
					break;
				case "LearningOjectsList":
					linkLOList();
					break;
				case "PendingQuizzesList":
					linkPendingQuiz();
					break;
				case "ThemeID":
					fixTheme();
					break;
				case "CompletedQuizzesList":
					//resetCompleteQuizzes();
					break; 
				}
			};


			//floatView.inputPost.

			var set = this.CreateBindingSet<MainView, MainViewModel>(); 
			set.Bind(floatView.inputPost.InputTextView.Text).To(vm => vm.NewPost);
			set.Apply ();

			floatView.inputPost.DoSendButton.TouchUpInside+= (sender, e) => {
				//vm.PostCommand.Execute(null);
				floatView.inputPost.InputTextView.Text = "" ;
			};
		}


		YComponents.LoadingView navigateLoad ;
		void FloatView_Navigate2Reader (object sender)
		{
			
			if( vm.LearningOjectsList != null)
			vm.OpenLOCommand.Execute(vm.LearningOjectsList[0]);

			navigateLoad = new YComponents.LoadingView (View.Bounds);
			View.Add (navigateLoad);
		}
 

		void fixTheme()
		{
			
			for (int i = 0; i < lobycourseList.Count; i++) 
				lobycourseList [i].ThemeColor = WidgetsUtil.themes[0];

			if(pendingQuizList!=null)
			for (int i = 0; i < pendingQuizList.Count; i++) 
				pendingQuizList [i].ThemeColor = WidgetsUtil.themes[0];

			if (usersList != null)
				for (int i = 0; i < usersList.Count; i++)
					usersList [i].ThemeColor = WidgetsUtil.themes [0];//[vm.ThemeID %6];

			floatView.postButton.BackgroundColor = WidgetsUtil.themes [0];//[vm.ThemeID % 6];
			floatView.unitsButton.BackgroundColor = WidgetsUtil.themes[0];// [vm.ThemeID % 6];
			floatView.workButton.BackgroundColor = WidgetsUtil.themes [0];//[vm.ThemeID % 6];

			floatView.topContent.BackgroundColor = WidgetsUtil.themes[0];// [vm.ThemeID % 6];


			//UIImage bimg =   UIImageEffects.ApplyBlur (backgroundImage.Image, 90, 
			//	WidgetsUtil.bthemes[vm.ThemeID%6], 0.6f, null);

			//backgroundImage.Image = bimg;
		}


		#region Fabebook Login

		//MobileServiceUser user ;

		private async Task DoLogoutFacebook()
		{
			//while (user == null)
			//{ 
				try
				{
					WAMSRepositoryService service = Mvx.Resolve<IRepositoryService>() as WAMSRepositoryService;
					//user = await service.MobileService.LoginAsync(this, provider); 

					 service.MobileService.Logout();
					//Console.WriteLine ("Facebook : " + user.UserId + "  " + user.MobileServiceAuthenticationToken ) ;
				}
				catch (InvalidOperationException e)
				{} 
			//} 

		}


		#endregion



		#region User


		void setUserName()
		{
			string name = "";
			if (vm.UserFirstName != null)
				name += (vm.UserFirstName + " ");
			if (vm.UserLastName != null)
				name += vm.UserLastName;
			floatView.mainUserName.Text = name;
		}


		void setUserImage()
		{
			if (vm.UserImage != null)
				floatView.mainUserImage.Image = WidgetsUtil.ToUIImage (vm.UserImage);
		}

		#endregion



		#region Courses

		List<LeftCircleItemView> coursesList  ;
		void linkCourses()
		{
			foreach(UIView sub in floatView.courseScroll.Subviews)
				sub.RemoveFromSuperview();
			
			coursesList = new List<LeftCircleItemView> ();
			populateCircleScroll(0);
			(ViewModel as MainViewModel).CirclesList.CollectionChanged+= (sender, e) => {
				populateCircleScroll(e.NewStartingIndex);
			};
		}

		void populateCircleScroll(int idx)
		{
			nfloat itemh = 58;
			if (vm.CirclesList != null) {
				if (vm.CirclesList.Count > 0)
					resetCourse (0);
				for (int i = idx; i < vm.CirclesList.Count; i++) {
					LeftCircleItemView item = new LeftCircleItemView (0, i * itemh);
					item.Title = vm.CirclesList [i].name;
					item.Number = vm.CirclesList [i].type +  "";
					item.Index = i;//vm.CirclesList [i].id;
					floatView.courseScroll.Add (item);
					coursesList.Add (item);
					floatView.courseScroll.ContentSize = new CGSize(300,(i+1)*itemh) ;


					item.LeftCircleItemViewTapped+= (object sender) => {
						resetCourse((sender as LeftCircleItemView).Index);
					};

				}
			}
		}


		void resetCourse(int c_id)
		{
			vm.SelectCircleCommand.Execute(vm.CirclesList[c_id]);

			foreach(UIView sub in floatView.commentsView.Subviews)
					sub.RemoveFromSuperview();
			
			foreach(UIView sub in floatView.loScroll.Subviews)
				sub.RemoveFromSuperview();
			
			commentPos = 0;
		}



		#endregion


		#region post link

		List<OutputPostView> postList ;
		void linkPostList()
		{
			postList = new List<OutputPostView> (); 
			//foreach(UIView sub in floatView.commentsView.Subviews)
			//	sub.RemoveFromSuperview();
			loadPostList (0);
			vm.PostsList.CollectionChanged+= (sender, e) => {
				loadPostList(e.NewStartingIndex);
			};
		}

		nfloat commentPos = 0;
		void loadPostList(int idx)
		{
			 
			for (int i = idx; i < vm.PostsList.Count; i++) {
				OutputPostView post = new OutputPostView (0, commentPos, 660 );
				post.SetText (vm.PostsList [i].post.name + " " + vm.PostsList [i].post.lastname,
					vm.PostsList [i].post.text,
					vm.PostsList [i].post.created_at.ToString ());

				postList.Add (post);
				vm.PostsList[i].PropertyChanged+= (sender, e) => {
					post.UserImage = WidgetsUtil.ToUIImage((sender as MainViewModel.post_with_username_wrapper).userImage); 
				};
				floatView.commentsView.Add (post);
				//floatView.commentsView.BackgroundColor = UIColor.Red;
				commentPos += post.GetHeight ();
			}

			floatView.commentsView.Frame = new CGRect (0, floatView.inputPost.GetHeight () + 24, 430, commentPos);
			floatView.postScroll.ContentSize = new CGSize (430, floatView.inputPost.GetHeight () + floatView.commentsView.Frame.Height + 24);
		}


		#endregion


		#region LOs and Homework

		List<LOThumbView> lobycourseList ;

		void linkLOList()
		{
			lobycourseList = new List<LOThumbView> ();
			loadLOs (0);
			vm.LearningOjectsList.CollectionChanged+= (sender, e) => {
				loadLOs(e.NewStartingIndex);
			};
		}
		public UIImage ToUIImage (byte[] bytes)
		{
			if (bytes == null)
				return null;

			using (var data = NSData.FromArray (bytes)) {
				return UIImage.LoadFromData (data);
			}
		}


		nfloat lo_yposition = 0 ;
		int loIdSelected = 0 ;
		void loadLOs(int index)
		{
			Random r = new Random ();
			nfloat v_width = 660, v_height = 430;
			nfloat tx , tdelta =30 , th =164 ;
			for (int i = index; i < vm.LearningOjectsList.Count; i++) {
				if (i >= 0) {
 
					lo_yposition = tdelta * (i / 2 + 1) + th * (nfloat)Math.Floor ((double)i / 2);
					if (i % 2 == 0)
						tx = 80;
					else
						tx = 354;
					LOThumbView lo = new LOThumbView (tx, lo_yposition);
					lo.SetValues (vm.LearningOjectsList [i].lo.title,
						r.Next (0, 100), r.Next (1, 20), r.Next (1, 10));
					lo.LOColorID = vm.LearningOjectsList [i].lo.color_id;
					if (vm.LearningOjectsList [i].cover_bytes != null)
						lo.LOImage = ToUIImage (vm.LearningOjectsList [i].cover_bytes );

					vm.LearningOjectsList [i].PropertyChanged += (sender, e) => {
						if(e.PropertyName == "cover_bytes")
							lo.LOImage = ToUIImage ((sender as MainViewModel.lo_by_circle_wrapper).cover_bytes);
						if(e.PropertyName == "background_bytes")
						{ 
							resetCenterView();
						}
					};

					lo.LOThumbSelected += (object sender, int color) => {
						//vm.ThemeID =  color ;

					};

					lobycourseList.Add (lo);
					floatView.loScroll.Add (lo); 
					floatView.loScroll.ContentSize = new CGSize (v_width, lo_yposition + th + tdelta);
				}
			}

			//resetCenterView ();
		}





		void resetCenterView()
		{
			byte[] bytes = vm.LearningOjectsList [loIdSelected].background_bytes;
			if (bytes != null) {
				floatView.backCenterImage.Image = WidgetsUtil.ToUIImage (bytes);
				//UIImage bimg =   UIImageEffects.ApplyBlur (WidgetsUtil.ToUIImage(bytes), 90, 
				//	WidgetsUtil.bthemes[vm.ThemeID%6], 0.6f, null);

				//backgroundImage.Image = bimg;
			}
		}


		List<HomeworkThumb> pendingQuizList ;
		void linkPendingQuiz()
		{
			pendingQuizList = new List<HomeworkThumb> ();
			resetPendingQuiz (0);
			vm.PendingQuizzesList.CollectionChanged+= (sender, e) => {
				resetPendingQuiz(e.NewStartingIndex);
			};
		}

		void resetPendingQuiz(int idx)
		{
			nfloat v_width = 660;
			nfloat tx , ty = 0 , tdelta =48 , th =150 ;
			for (int i = idx; i < vm.PendingQuizzesList.Count; i++) {
				if (i >= 0) {
					ty = tdelta * (i / 2 + 1) + th * (nfloat)Math.Floor ((double)i / 2);
					if (i % 2 == 0)
						tx = 80;
					else
						tx = 358;
					HomeworkThumb lo = new HomeworkThumb (tx, ty);
					lo.SetValues (vm.PendingQuizzesList [i].title,
						vm.PendingQuizzesList [i].content,
						15, true);

					pendingQuizList.Add (lo);
					floatView.workScroll.Add (lo); 
					floatView.workScroll.ContentSize = new CGSize (v_width, ty + th + tdelta);
				}
			}
		}

		#endregion


		List<UserElementView> usersList;
		void linkUserList()
		{
			usersList = new List<UserElementView>();
			foreach(UIView sub in floatView.PeopleScroll.Subviews)
				sub.RemoveFromSuperview();
		
			var itemH = 38;
			loadUserList (0, itemH);
			vm.UsersList.CollectionChanged+= (sender, e) => {
				loadUserList(e.NewStartingIndex,itemH);
			};
		}


		void loadUserList(int idx, nfloat h)
		{
			for (int i = idx ; i < vm.UsersList.Count; i++) {
				UserElementView elem = new UserElementView(0,i*h);
				if (vm.UsersList [i].user.is_online > 0)
					elem.IsOnline = true;
				else
					elem.IsOnline = false;
				elem.Name = vm.UsersList[i].user.name +  " " + vm.UsersList[i].user.lastname ;
				elem.ThemeColor = WidgetsUtil.themes [0];
				usersList.Add(elem);
				floatView.PeopleScroll.Add(elem);

				floatView.PeopleScroll.ContentSize=  new CGSize (228, h * (i+1));

				vm.UsersList[i].PropertyChanged+= (s1, e1) => {
					elem.UserImage = WidgetsUtil.ToUIImage((s1 as MainViewModel.user_by_circle_wrapper).userImage);
				};

			}
		}



		#region BackImage

		UIImageView backgroundImage , waitImage ;

		void initBackgroundImage()
		{
			waitImage = new UIImageView (new CGRect(0,0,1024,768));
			waitImage.Layer.ZPosition = -2;
			UIImage bimg = UIImage.FromFile ("efiles/backgroundmuro.jpg");
			//UIImageEffects.ApplyBlur (, 90, 
			//			WidgetsUtil.bthemes[vm.ThemeID%6], 0.6f, null);

			waitImage.Image = bimg;
			View.Add (waitImage);

			backgroundImage = new UIImageView (new CGRect(0,0,1024,768));
			//backgroundImage.Layer.ZPosition = -1;

			//View.Add (backgroundImage);


		}

		#endregion



	}
}