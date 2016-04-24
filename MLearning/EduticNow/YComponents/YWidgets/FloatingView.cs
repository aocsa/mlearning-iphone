using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;

namespace YComponents.YWidgets
{
	public delegate void Navigate2ReaderEventHandler(object sender);
	public delegate void Navigate2ReaderStartedEventHandler(object sender);
	public class FloatingView : UIView
	{

		public event Navigate2ReaderEventHandler Navigate2Reader;
		public event Navigate2ReaderStartedEventHandler Navigate2ReaderStarted;
		public FloatingView ()//:base()
		{
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
			Frame = new CGRect (0,0,1024,768);

			initView ();
			BackgroundColor = UIColor.Clear;

		}

		#region view properties

		string filepath = "efiles/muro/" ;

		nfloat h= 750, w= 888 , x= 82, y =20;
		nfloat x_max = 290 ;	
		UIView content ;
		bool isOpen = false ;
		nfloat swipe_delta =0  ;


		UIColor themeColor = WidgetsUtil.themes [0]; //UIColor.FromRGBA(255,210,0,240);

		#endregion
	

		#region private functions


		void initView()
		{
			content = new UIView (new CGRect (x, y, w, h));//{ BackgroundColor = UIColor.White};
			content.Layer.CornerRadius = 8;
			Add (content);
 
			initLeftMenuView ();
			initTopContent ();
			initCenterContent ();
			initDownContent ();

			var panGesture = new UIPanGestureRecognizer (panHandle);
			panGesture.MaximumNumberOfTouches = 1; 
			AddGestureRecognizer (panGesture);


			var bookImage = new UIImageView (new CGRect(w+90 , h/2 - 20, 38,38));
			bookImage.Image = UIImage.FromFile (filepath + "contenido.png");
			Add (bookImage);

			//addScrollView ();
		}


		UIScrollView scroll ;
		void addScrollView()
		{
			scroll = new UIScrollView (new CGRect ( 50,50, 200, 300));
			content.Add (scroll);

			scroll.ContentSize = new CGSize (200, 600);

			scroll.Add (new UIView(new CGRect(30,20, 50,80)){BackgroundColor= UIColor.Red});
			scroll.Add (new UIView(new CGRect(30,20, 50,180)){BackgroundColor= UIColor.Red});
		}
	 

		void animateOffset(UIView v , nfloat xoffset, nfloat yoffset, double dur)
		{
			UIView.Animate( dur, 0, UIViewAnimationOptions.CurveEaseIn ,
				() => { v.Center =  new PointF ((float)xoffset,
					 (float)yoffset);}, null ); 
		}

		#endregion




		#region Top Content

		public string UserName {
			get { return "";}
			set { usernameLabel.Text = value; }
		}

		public UIView topContent ;
		LoadingView loadView ;
		UILabel usernameLabel ;
		UIImageView userImageView ;
		UIView commentBT ;
		NotificationCounter whiteNotView ;

		void initTopContent()
		{
			topContent = new UIView(new CGRect( 0,0,w,58)){BackgroundColor =  themeColor};
			content.Layer.MasksToBounds = true;
			content.Add (topContent);

			//add logo
			var logoimage = new UIImageView(new CGRect(16,16,102,20)){ContentMode = UIViewContentMode.ScaleAspectFit};
			logoimage.Image = UIImage.FromFile (filepath+"logo.png");
			topContent.Add (logoimage);

			//avance tareas text
			var avancelabel = new UILabel(new CGRect(218,20,125,16));
			avancelabel.Text = "Avance de tareas";
			avancelabel.Font = UIFont.FromName ("HelveticaNeue", 14);
			topContent.Add (avancelabel);

			//loadview
			loadView = new LoadingView(340, 22, 188,22 , filepath + "loadtareas.png") ;
			loadView.LoadPercent = 60;
			topContent.Add (loadView);

			//nombre del usuario
			usernameLabel = new UILabel(new CGRect(602,28, 80,16)){ Text = "Steven"};
			usernameLabel.Font = UIFont.FromName ("HelveticaNeue", 14);
			topContent.Add (usernameLabel);

			//imagen del usuario
			userImageView = new UIImageView(new CGRect(690, 10, 42,42))  ;
			userImageView.Image = UIImage.FromFile ("MyImage.png");
			userImageView.Layer.MasksToBounds = true;
			userImageView.Layer.CornerRadius = 4;

			//userImageView = WidgetsUtil.getImageFromBytes(690,10,42,42, 4 , 0
			topContent.Add (userImageView);

			//comment bt
			commentBT = new UIView(new CGRect(770, 20, 26,22));
			commentBT.Add (new UIImageView(new CGRect(0,0,26,22)){Image = UIImage.FromFile(filepath + "chatwhite.png") });
			topContent.Add (commentBT);

			//cnotification
			var campimage = new UIImageView(new CGRect(822,17,20,24)){Image = UIImage.FromFile(filepath + "notificacioneswhite.png")};
			topContent.Add (campimage);
			whiteNotView = new NotificationCounter(838, 8, 12) ;
			topContent.Add (whiteNotView);

		}


		#endregion


		#region Center Content

		UIView centerContent ;
		public UIImageView  backCenterImage, iconCenterImage ;
		public UILabel centerTitle, centerUnits , centerStudents;

		void initCenterContent()
		{
			centerContent = new UIView (new CGRect (0,58,w,208)){BackgroundColor =  UIColor.Clear};
			content.Add (centerContent);

			//backimage
			backCenterImage = new UIImageView(new CGRect(0,0,w,208)) {ContentMode = UIViewContentMode.ScaleAspectFill};
			backCenterImage.Layer.MasksToBounds = true;
			backCenterImage.Image = UIImage.FromFile ("MyImage.png");
			centerContent.Add (backCenterImage);

			iconCenterImage = new UIImageView (new CGRect(34,56, 90,90)){ContentMode = UIViewContentMode.ScaleAspectFit};
			iconCenterImage.Image = UIImage.FromFile (filepath + "colegioheader.png");
			centerContent.Add (iconCenterImage);

			centerTitle = new UILabel (new CGRect (148, 60, 600, 38)) {
				Font = UIFont.FromName ("HelveticaNeue",36) ,
				TextColor =  UIColor.White,
				Text = "Edutic Now"// "Colegio San Jose" 
			};
			centerContent.Add (centerTitle);


			centerUnits = new UILabel (new CGRect (148, 100, 300, 26)) {
				Font = UIFont.FromName ("HelveticaNeue",20) ,
				TextColor =  UIColor.White,
				Text = "15 cursos" 
			};
			centerContent.Add (centerUnits);

			centerStudents = new UILabel (new CGRect (148, 124, 300, 26)) {
				Font = UIFont.FromName ("HelveticaNeue",20) ,
				TextColor =  UIColor.White,
				Text = "30 estudiantes" 
			};
			centerContent.Add (centerStudents);


		}


	



		#endregion



		#region Down Content

		UIView downContent ;
		UIView firstView, secondView ;//first for presentation - second content menu

		void initDownContent()
		{
			downContent = new UIView (new CGRect (0,266,w,484)){BackgroundColor =  UIColor.White};
			content.Add (downContent);

			//initFirstView ();
			initSecondView ();
			initLeftView ();
		}


		//UILabel
		void initFirstView()
		{
			firstView = new UIView (new CGRect(0,0,660,484)){BackgroundColor=UIColor.Blue};
			downContent.Add (firstView);
		}



		#endregion

		#region Second Down View

		void initSecondView()
		{
			secondView = new UIView (new CGRect(0,0,660,484));
			//secondView.Hidden = true;
			downContent.Add (secondView);

			initPostView ();
			initBottonMenu ();
			initLOsView ();
			initWorkView ();
		}


		UIView postView;
		public UIView commentsView ; //post for all - comment into post  
		public UIScrollView postScroll ;

		public InputPostView inputPost ;
		nfloat v_width = 660, v_height =  430 , v_pos = 54; // vpos = de las vistas 
		void initPostView()
		{
			postView = new UIView(new CGRect(0,v_pos,v_width ,v_height)) {BackgroundColor = UIColor.Clear};
			secondView.Add (postView);
			postScroll = new UIScrollView (new CGRect (0,0,v_width,v_height)){BackgroundColor = UIColor.Clear}; 
			postView.Add (postScroll); 

			inputPost = new InputPostView (16, 12, 614, 42);
			postScroll.Add (inputPost);
			postScroll.ContentSize = new CGSize (v_width, inputPost.GetHeight () +24);

			commentsView = new UIView(new CGRect(0,inputPost.GetHeight() + 24, v_width, 100)){BackgroundColor = UIColor.Clear};
			postScroll.Add (commentsView);
			postScroll.ContentSize = new CGSize (v_width, inputPost.GetHeight () + 24);
			//postScroll.AlwaysBounceVertical = true;


			inputPost.IPostSizeChanged+= (object sender) => {
				postScroll.ContentSize = new CGSize (v_width, inputPost.GetHeight () + commentsView.Frame.Height + 24);
				UIView.Animate(0.2 ,0, UIViewAnimationOptions.CurveEaseIn  , () =>
					{
						commentsView.Frame = new CGRect(0,inputPost.GetHeight() + 24, v_width, commentsView.Frame.Height) ;
					}, null );
			};

			inputPost.IPostDoComment+= (object sender) => {
				postScroll.ContentSize = new CGSize (v_width, inputPost.GetHeight () + commentsView.Frame.Height+ 24);
				UIView.Animate(0.2 ,0, UIViewAnimationOptions.CurveEaseIn  , () =>
					{
						commentsView.Frame = new CGRect(0,inputPost.GetHeight() + 24, v_width, commentsView.Frame.Height) ;
					}, null );
			};

			postView.Layer.ZPosition = 2;
			postView.Alpha = 1;

			//tmp
			//addComments ();
		}


		/// <summary>
		/// Set is animated
		/// </summary>
		void addComments()
		{
			nfloat pos = 0;
			for (int i = 0; i < 4; i++) {
				string text = "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo";
				OutputPostView post = new OutputPostView (0, pos, 660 );
				post.SetText ("Alexander Ocsa", text, "1.24 pm");

				commentsView.Add (post);
				pos += post.GetHeight ();
			}
			commentsView.Frame = new CGRect (0, inputPost.GetHeight () + 24, v_width, pos);
			postScroll.ContentSize = new CGSize (v_width, inputPost.GetHeight () + commentsView.Frame.Height + 24);
		}


		UIView losView ; //View for Los
		public UIScrollView loScroll ;  

		void initLOsView()
		{
			losView = new UIView (new CGRect(0,v_pos, v_width , v_height )){BackgroundColor = UIColor.White};
			secondView.Add (losView);

			loScroll = new UIScrollView (new CGRect(0,0, v_width , v_height )){BackgroundColor = UIColor.Clear}; 
			//loScroll.ShowsVerticalScrollIndicator = false;
			losView.Add (loScroll);

			losView.Layer.ZPosition = 1;
			losView.Alpha = 0;

			/**tmp
			nfloat tx , ty = 0 , tdelta =30 , th =164 ;
			for (int i = 0; i < 8; i++) {
				ty = tdelta * (i/2 + 1) + th* (nfloat)Math.Floor((double)i/2);
				if (i % 2 == 0)
					tx = 80;
				else
					tx = 354;
				LOThumbView lo = new LOThumbView (tx, ty);
				loScroll.Add (lo);

				loScroll.ContentSize = new CGSize (v_width , ty + th + tdelta);
			}*/

		}


		UIView workView ; //View for Los
		public UIScrollView workScroll ;  

		void initWorkView()
		{
			workView = new UIView (new CGRect(0,v_pos, v_width , v_height )){BackgroundColor = UIColor.White};
			secondView.Add (workView);

			workScroll = new UIScrollView (new CGRect(0,0, v_width , v_height )){BackgroundColor = UIColor.Clear}; 
			//workScroll.ShowsVerticalScrollIndicator = false;
			workView.Add (workScroll);

			workView.Layer.ZPosition = 1;
			workView.Alpha = 0;

			//tmp
			nfloat tx , ty = 0 , tdelta =48 , th =150 ;
			for (int i = 0; i < 8; i++) {
				ty = tdelta * (i/2 + 1) + th* (nfloat)Math.Floor((double)i/2);
				if (i % 2 == 0)
					tx = 80;
				else
					tx = 358;
				HomeworkThumb lo = new HomeworkThumb (tx, ty);
				workScroll.Add (lo);

				workScroll.ContentSize = new CGSize (v_width , ty + th + tdelta);
			}

		}


		UIView bottonMenu ;
		public UIButton postButton, unitsButton, workButton ;
		void initBottonMenu()
		{
			bottonMenu = new UIView (new CGRect(0,0,w,54)){BackgroundColor = UIColor.FromRGBA (240,240,240,240)};
			//bottonMenu.Hidden = true;
			downContent.Add (bottonMenu);

			postButton = WidgetsUtil.getDownMenuButton (84, 12, WidgetsUtil.themes [0], "All post");
			bottonMenu.Add (postButton); 
			unitsButton = WidgetsUtil.getDownMenuButton (84 + 140, 12, WidgetsUtil.themes [0], "Unidades"); 
			bottonMenu.Add (unitsButton); 
			workButton = WidgetsUtil.getDownMenuButton (84 + 280 , 12 , WidgetsUtil.themes [0], "Tareas");
			bottonMenu.Add(workButton); 

			handleEventsForMenu ();
		}


		//Handle buttons events and do Animation for subViews
		void handleEventsForMenu()
		{
			postButton.TouchUpInside += (sender, e) => {
				UIView.Animate(0.25,()=>{
					postView.Alpha =  1 ;
					losView.Alpha = 0 ;
					workView.Alpha = 0 ;
				}, ()=>{
					postView.Layer.ZPosition = 2 ;
					losView.Layer.ZPosition =1 ;
					workView.Layer.ZPosition = 1 ;
				}) ;	
			};

			unitsButton.TouchUpInside += (sender, e) => {
				UIView.Animate(0.25,()=>{
					postView.Alpha =  0 ;
					losView.Alpha = 1 ;
					workView.Alpha = 0 ;
				}, ()=>{
					postView.Layer.ZPosition = 1 ;
					losView.Layer.ZPosition =2 ;
					workView.Layer.ZPosition = 1 ;
				}) ;
			};

			workButton.TouchUpInside += (sender, e) => {
				UIView.Animate(0.25,()=>{
					postView.Alpha =  0 ;
					losView.Alpha = 0 ;
					workView.Alpha = 1 ;
				}, ()=>{
					postView.Layer.ZPosition = 1 ;
					losView.Layer.ZPosition =1 ;
					workView.Layer.ZPosition = 2 ;
				}) ;
			};
		}


		#endregion


		#region Right SubView

		UIView rightDownView ;
		UILabel ptodayLabel, pweekLabel;
		public UIScrollView PeopleScroll ;
		void initLeftView()
		{
			rightDownView = new UIView (new CGRect( 660, 0, 228, 484)){BackgroundColor = UIColor.Clear};
			downContent.Add (rightDownView);

			//Activity
			var labelActivity = new UILabel(new CGRect(14, 20, 170,18))
			{
				TextColor = UIColor.Black,
				Font = UIFont.FromName("HelveticaNeue-Bold",12),
				Text = "ACTIVITY"
			};
			rightDownView.Add (labelActivity);
			ptodayLabel = new UILabel(new CGRect(14, 48, 170,18))
			{
				TextColor = UIColor.Black,
				Font = UIFont.FromName("HelveticaNeue",12),
				Text = "15 people online today"
			};
			rightDownView.Add (ptodayLabel);
			pweekLabel = new UILabel(new CGRect(14, 68, 170,18))
			{
				TextColor = UIColor.Black,
				Font = UIFont.FromName("HelveticaNeue",12),
				Text = "15 people online this week"
			};
			rightDownView.Add (pweekLabel);

			//Persons
			var labelPersons = new UILabel(new CGRect(14, 120, 170,18))
			{
				TextColor = UIColor.Black,
				Font = UIFont.FromName("HelveticaNeue-Bold",12),
				Text = "PERSONAS"
			};
			rightDownView.Add (labelPersons); 

			PeopleScroll = new UIScrollView (new CGRect(0, 150, 228, 36 * 8 ));
			rightDownView.Add (PeopleScroll);

			/**tmp init scroll
			for (int i = 0; i < 10; i++) {
				var item = new UserElementView (0, 38 * i) {
					UserImage = UIImage.FromFile ("MyImage.png"),
					IsOnline = true,
					ThemeColor = WidgetsUtil.themes [0],
					Name = "Jose Herrera"
				};
				PeopleScroll.Add (item);
			}
			PeopleScroll.ContentSize = new CGSize (228, 38 * 10);*/
		}

		#endregion
 

		#region Left Menu

		nfloat leftMenuWidth = 300 ;
		public UIView leftMenuView ;

		public UIImageView mainUserImage ;
		public UILabel mainUserName;
		UILabel userTypeLabel ;

		void initLeftMenuView()
		{

			leftMenuView = new UIView (new CGRect(0,0,leftMenuWidth, 768)){BackgroundColor= UIColor.Clear};
			leftMenuView.Layer.ZPosition = -1;
			Add (leftMenuView);

			//images Left - User
			var commImage = new UIImageView(new CGRect(28,80,20,18)){Image = UIImage.FromFile(filepath+ "chaticon.png")};
			leftMenuView.Add (commImage);

			var notifImage = new UIImageView(new CGRect(30,124,16,20)){Image = UIImage.FromFile(filepath+ "notificaciones.png")};
			leftMenuView.Add (notifImage);

			mainUserImage = new UIImageView(new CGRect(100,70,90,90)){Image = UIImage.FromFile( "MyImage.png")};
			mainUserImage.Layer.CornerRadius = 6;
			mainUserImage.Layer.MasksToBounds = true;
			mainUserImage.ContentMode = UIViewContentMode.ScaleAspectFill;
			mainUserImage.Alpha = 0;
			leftMenuView.Add (mainUserImage);


			mainUserName = new UILabel (new CGRect( 60, 184,155, 22)){
				TextColor = UIColor.White,
				Text ="Alexander Ocsa",
				TextAlignment = UITextAlignment.Center,
				Font = UIFont.FromName("HelveticaNeue",18),
				Alpha = 0 
			};
			leftMenuView.Add (mainUserName);

			userTypeLabel = new UILabel (new CGRect(118,210,54,18)){
				TextAlignment = UITextAlignment.Center ,
				Text = "Alumno",
				TextColor = UIColor.Gray,
				Font = UIFont.FromName("HelveticaNeue",15),
				Alpha =0
			};
			leftMenuView.Add (userTypeLabel);

			//load view



			initLeftDownMenu ();
		}


		public UIScrollView courseScroll ;
		public LeftMenuButton schoolButton, courseButton, worksButton, seeWorkButton, doAskButton ;
		void initLeftDownMenu()
		{
			schoolButton = new LeftMenuButton (0, 314, UIColor.Clear){
				Image = UIImage.FromFile(filepath + "colegio.png"),
				Title ="Colegio Sagrado Corazon"
			};
			schoolButton.SetInvisible (false);
			leftMenuView.Add (schoolButton);

			courseButton = new LeftMenuButton (0, 360, UIColor.FromRGBA(0,0,0,120)){
				Image = UIImage.FromFile(filepath + "cursos.png"),
				Title ="CURSOS"
			};
			courseButton.SetInvisible (false);
			leftMenuView.Add (courseButton);

			worksButton = new LeftMenuButton (0, 580,UIColor.FromRGBA(0,0,0,120) ){
				Image = UIImage.FromFile(filepath + "tareas.png"),
				Title ="TAREAS"
			};
			worksButton.SetInvisible (false);
			leftMenuView.Add (worksButton);

			seeWorkButton = new LeftMenuButton (0, 626, UIColor.Clear){
				Image = UIImage.FromFile(filepath + "vertareas.png"),
				Title ="Ver Tareas"
			};
			seeWorkButton.SetInvisible (false);
			leftMenuView.Add (seeWorkButton);

			doAskButton = new LeftMenuButton (0, 672, UIColor.Clear){
				Image = UIImage.FromFile(filepath + "icon_logout.png"),
				Title ="Salir"
			};
			doAskButton.SetInvisible (false);
			leftMenuView.Add (doAskButton);


			courseScroll = new UIScrollView (new CGRect(0,406, 300,174)){Alpha=0};
			leftMenuView.Add (courseScroll);

			/**temp courses
			nfloat itemh = 58 ;
			for (int i = 0; i < 6; i++) {
				LeftCircleItemView item = new LeftCircleItemView (0, i * itemh);
				item.Title = "Camino Inca " + i;
				item.Number = "" + i;
				courseScroll.Add (item);
				courseScroll.ContentSize = new CGSize(300,(i+1)*itemh) ;
			} */
		}


		void setSchoolButton()
		{
			UITapGestureRecognizer gesture = new UITapGestureRecognizer (() => {
				if(!isOpen)
					OpenContent();
			}) {NumberOfTapsRequired = 1};
		}


		public void setVisibleLeftMenu()
		{
			schoolButton.SetVisible (true);
			courseButton.SetVisible (true);
			worksButton.SetVisible (true);
			seeWorkButton.SetVisible (true);
			doAskButton.SetVisible (true);
			UIView.Animate (0.25, () => {
				courseScroll.Alpha =1 ;
				mainUserImage.Alpha = 1 ;
				mainUserName.Alpha =1 ;
				userTypeLabel.Alpha = 1 ;
			});
		}

		public void setInvisibleLeftMenu()
		{
			schoolButton.SetInvisible (true);
			courseButton.SetInvisible (true);
			worksButton.SetInvisible (true);
			seeWorkButton.SetInvisible (true);
			doAskButton.SetInvisible (true);
			UIView.Animate (0.25, () => {
				courseScroll.Alpha =0 ;
				mainUserImage.Alpha = 0 ;
				mainUserName.Alpha = 0 ;
				userTypeLabel.Alpha = 0 ;
			});
		}
	 

		#endregion

		#region Gestures

		void panHandle (UIPanGestureRecognizer gestureRecognizer)
		{
			//AdjustAnchorPointForGestureRecognizer (gestureRecognizer);
			var image = gestureRecognizer.View;
			if (gestureRecognizer.State == UIGestureRecognizerState.Began || gestureRecognizer.State == UIGestureRecognizerState.Changed) {
				var translation = gestureRecognizer.TranslationInView (this);
				//image.Center = new CGPoint (image.Center.X + translation.X, image.Center.Y);// + translation.Y

				nfloat delta = -1*translation.X;
				if (isOpen) {
					//left menu is active
					if (swipe_delta < 0)
						swipe_delta += (delta * 0.3f);
					else if (swipe_delta < x - x_max)
						swipe_delta += (delta * 0.3f);
					else
						swipe_delta += delta;
					content.Frame = new CGRect ( x_max - swipe_delta , y,w,h);

				} else {
					//when is in the normal state 
					if(swipe_delta >0)
						swipe_delta += (delta * 0.7f); //navigate to nextview
					else if (swipe_delta <  (x-x_max))
						swipe_delta += (delta * 0.3f); // stopping the menu
					else swipe_delta += delta;  // to the menu

					content.Frame = new CGRect (x - swipe_delta , y,w,h);
				}



				// Reset the gesture recognizer's translation to {0, 0} - the next callback will get a delta from the current position.
				gestureRecognizer.SetTranslation (CGPoint.Empty, this);
			}


			if (gestureRecognizer.State == UIGestureRecognizerState.Ended) {
				CGPoint velocity = gestureRecognizer.VelocityInView (this);
				//image.Center = new CGPoint (0,0); 
				if (content.Frame.X > (x_max + x) / 2)
					OpenContent ();
				else if (content.Frame.X < -400) {
					if (Navigate2ReaderStarted != null)
						Navigate2ReaderStarted (this);



					UIView.Animate (0.35, () => {
						content.Frame = new CGRect (-1*w, y, w, h);
						leftMenuView.Alpha = 0 ;
					}, () => {
						if (Navigate2Reader != null)
							Navigate2Reader (this);
					});


				} else {
					CloseContent ();
				}


				swipe_delta = 0;
			}
		}

		#endregion



		#region  Animations

		public void OpenContent()
		{
			UIView.Animate( 0.25, 0, UIViewAnimationOptions.CurveEaseOut ,
				() => { content.Frame = new CGRect(x_max,y,w,h);},null);

			setVisibleLeftMenu ();

			isOpen = true;
		}


		public void CloseContent()
		{
			UIView.Animate( 0.25, 0, UIViewAnimationOptions.CurveEaseOut ,
				() => {content.Frame = new CGRect(x,y,w,h);}, null ); 

			setInvisibleLeftMenu ();
			isOpen = false;
		}


		void ShowSchoolView()
		{
			
		}

		void ShowDownMenu()
		{
			
		}

		#endregion


		 
 
	}

}

