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
using YComponents;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Core.Repositories;
using Cirrious.CrossCore;

namespace MLearning.UnifiedTouch.Views
{
	[Register("LoginView")]
	public class LoginView : MvxViewController
	{

		string filepath = "efiles/login/" ; 
		UIView mainView, photoView, loginView, regView; 
		int activeView = 0 ;//0:mainview 1:loginview 2:photoview 3:regview 



		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			View.BackgroundColor = UIColor.White;

			// ios7 layout
			if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
				EdgesForExtendedLayout = UIRectEdge.None;

			setOrientation ();

			setBackground ();
			initBlurEffect ();
			initMainView ();
			initPhotoView();
			initLoginView();
			initRegisterView ();

			//bind
			doBindings();

			setTopView (mainView);
			setAnimations ();

			//for input
			var tap = new UITapGestureRecognizer (() => 
				{
					loginUserInput.ResignFirstResponder();
					loginPassInput.ResignFirstResponder();
					regNameInput.ResignFirstResponder();
					regUserInput.ResignFirstResponder();
					regPassInput.ResignFirstResponder();
					regEmailInput.ResignFirstResponder(); 
				} );
			View.AddGestureRecognizer (tap); 

			//var vm1 = ViewModel as MainViewModel ;
			var vm = ViewModel as LoginViewModel ;
			vm.PropertyChanged+= (sender, e) => {

				if(e.PropertyName =="UserImageBytes")
				{
					//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>Cambio imagen
					//regUserImage = getimagefrombytes(457,164,110,110, (sender as LoginViewModel).UserImageBytes );
					//regView.Add(regUserImage);
					regView.SetNeedsDisplay();
					if(isNewPhotoReady)
					{
						photo2register();
						activeView =3 ;
					}
					else isNewPhotoReady = true ;
				}
			};
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

		void doBindings()
		{
			var set = this.CreateBindingSet<LoginView, LoginViewModel>(); 
			set.Bind(loginUserInput).To(vm => vm.Username);   
			set.Bind (loginPassInput).To (vm=>vm.Password);

			set.Bind (regNameInput).To (vm=>vm.RegUsername );
			set.Bind (regNameInput).To (vm=>vm.Name);
			set.Bind (regPassInput).To (vm=>vm.RegPassword	);
			set.Bind (regEmailView).To (vm=>vm.Email);
			//login facebook
			set.Bind(cameraChoose).To(vm => vm.TakePictureCommand);
			set.Bind(galleryChoose).To(vm => vm.ChoosePictureFromLibraryCommand); 

			set.Bind (doRegButton).To (vm=>vm.RegisterCommand);

			set.Apply ();
		}

	  
		/// <summary>
		/// The back image.
		/// </summary>
		UIImageView backImage ;
		void setBackground()
		{
			backImage = new UIImageView (new CGRect(0,0,EConstants.DeviceWidth,EConstants.DeviceHeight));
			backImage.Image = UIImage.FromFile ("efiles/fondologin.jpg");//filepath + "fondoa.jpg");
			View.Add (backImage);
		}


		void setTopView(UIView v )
		{
			//mainView.Layer.ZPosition = 0;
			mainView.Alpha = 0;
			//photoView.Layer.ZPosition = 0;
			photoView.Alpha = 0;
			//loginView.Layer.ZPosition = 0;
			loginView.Alpha = 0;
			//regView.Layer.ZPosition = 0;
			regView.Alpha = 0;
			//v.Layer.ZPosition = 10;
			mainView.Alpha = 1;

			blurMask.Alpha = 0;
			 
		}


		#region Fabebook Login

		MobileServiceUser user ;

		private async Task Authenticate(MobileServiceAuthenticationProvider provider)
		{
			while (user == null)
			{ 
				try
				{
					WAMSRepositoryService service = Mvx.Resolve<IRepositoryService>() as WAMSRepositoryService;
					//user = await service.MobileService.LoginAsync(this, provider); 
					user =  await service.MobileService.LoginAsync(this, provider ); 

					//await service.MobileService.Logout();
					Console.WriteLine ("Facebook : " + user.UserId + "  " + user.MobileServiceAuthenticationToken ) ;
				}
				catch (InvalidOperationException e)
				{
					
				} 
			} 
			var vm = ViewModel as LoginViewModel;
			vm.CreateUserCommand.Execute(user);
		}
 

		#endregion


		#region Animations

		void setAnimations()
		{
			mainAnimation ();
		}

		/**
		UIView.Animate(0.35 ,0, UIViewAnimationOptions.CurveEaseIn  , () =>
				{ logoImage.Alpha = 0; }, null);
		**/

		void mainAnimation()
		{
			animateOffset(logoImage,0,-46,0.35);
 
			UIView.Animate( 0.35, 0, UIViewAnimationOptions.CurveEaseIn ,
				() => { descriptionView.Center =  new PointF ((float)descriptionView.Center.X  ,
					(float)descriptionView.Center.Y - 46);}, ()=>{
					animateAlpha(checkDescription,0.3,1.0f);
					animateAlpha(gotoSignView,0.3,1.0f);
					animateAlpha(gotoLoginBT,0.3,1.0f);
					animateAlpha(signFaceView,0.3,1.0f);
				} ); 
		}


	 

		void main2login()
		{ 
			animateOffset (mainView, -1024, 0, 0.4);
			
			UIView.Animate(0.25 ,0, UIViewAnimationOptions.CurveEaseIn  , () =>
				{ mainView.Alpha = 0; }, ()=>{
					animateAlpha(loginView,0.25,1.0f); 
					animateOffset(loginView,-1024,0,0.4) ;
				} );
			
	 
		}

		void login2main()
		{
			animateOffset (loginView, 1024, 0, 0.4);

			UIView.Animate(0.25 ,0, UIViewAnimationOptions.CurveEaseIn  , () =>
				{ loginView.Alpha = 0; }, ()=>{
					animateAlpha(mainView,0.25,1.0f); 
					animateOffset(mainView,1024,0,0.4) ;
				} );
		}


		void main2photo()
		{ 
			animateOffset (mainView, -1024, 0, 0.4);
			animateAlpha (blurMask, 0.35, 1.0f);
			UIView.Animate(0.25 ,0, UIViewAnimationOptions.CurveEaseIn  , () =>
				{ mainView.Alpha = 0; }, ()=>{
					animateAlpha(photoView,0.25,1.0f); 
					animateOffset(photoView,-1024,0,0.4) ;
				} );


		}
	 
		void photo2main()
		{ 
			animateOffset (photoView, 1024, 0, 0.4);
			animateAlpha (blurMask, 0.35, 0.0f);
			UIView.Animate(0.25 ,0, UIViewAnimationOptions.CurveEaseIn  , () =>
				{ photoView.Alpha = 0; }, ()=>{
					animateAlpha(mainView,0.25,1.0f); 
					animateOffset(mainView,1024,0,0.4) ;
				} );


		}


		void photo2register ()
		{
			animateOffset (photoView, -1024, 0, 0.4);
			animateAlpha (blurMask, 0.35, 0.0f);
			UIView.Animate(0.25 ,0, UIViewAnimationOptions.CurveEaseIn  , () =>
				{ photoView.Alpha = 0; }, ()=>{
					animateAlpha(regView,0.25,1.0f); 
					animateOffset(regView,-1024,0,0.4) ;
					photoLoading.Hide ();
				});
		}


		//new
		void main2register()
		{
			animateOffset (mainView, -1024, 0, 0.4);
			animateAlpha (blurMask, 0.35, 1.0f);
			UIView.Animate(0.25 ,0, UIViewAnimationOptions.CurveEaseIn  , () =>
				{ photoView.Alpha = 0; }, ()=>{
					animateAlpha(regView,0.25,1.0f); 
					animateOffset(regView,-1024,0,0.4) ;
					//photoLoading.Hide ();
				});
		}


		//new
		void register2main()
		{
			animateOffset (regView, 1024, 0, 0.4);
			animateAlpha (blurMask, 0.35, 1.0f);
			UIView.Animate(0.25 ,0, UIViewAnimationOptions.CurveEaseIn  , () =>
				{ regView.Alpha = 0; }, ()=>{
					animateAlpha(mainView,0.25,1.0f); 
					animateOffset(mainView,1024,0,0.4) ;
				} );
		}


		void register2photo()
		{

			animateOffset (regView, 1024, 0, 0.4);
			animateAlpha (blurMask, 0.35, 1.0f);
			UIView.Animate(0.25 ,0, UIViewAnimationOptions.CurveEaseIn  , () =>
				{ regView.Alpha = 0; }, ()=>{
					animateAlpha(photoView,0.25,1.0f); 
					animateOffset(photoView,1024,0,0.4) ;
				} );
		}




		#endregion


		#region Main View
 
		void initMainView()
		{
			mainView = new UIView (new CGRect (0,0,1024,768));
			View.Add (mainView);

			setLogo ();
			addDescriptionText ();

			addCheckBox ();
			initGotoSign ();
			initSignFacebook ();
			initGotoLogin ();

			//animations
			//animateOffset(logoImage,0,-46,0.35);
			//animateOffset (descriptionView, 0, -46, 0.35);

			checkDescription.Alpha = 0;
			gotoSignView.Alpha = 0;
			gotoLoginBT.Alpha = 0;
			signFaceView.Alpha = 0;
		}

		UIImageView logoImage ;
		void setLogo()
		{
			logoImage = new UIImageView (new CGRect(370,276,282,54));
			logoImage.Image = UIImage.FromFile (filepath + "logo.png");
			mainView.Add (logoImage);
		}

		UIView descriptionView ; 
		void addDescriptionText()
		{
			descriptionView = new UIView (new CGRect(338, 374, 348, 110));
			mainView.Add (descriptionView);

			string s1 = "Connect with your classmates";
			string s2 = "Get help on homework";
			string s3 = "Get better graces for real";

			descriptionView.Add (getTextField(s1, 8, 28 , 0,0,348, 30 , 24f));
			descriptionView.Add (getTextField(s2, 8, 20 , 0,44,348, 30, 24f));
			descriptionView.Add (getTextField(s3, 17, 26 , 0,88,348, 30, 24f));
		}



		YCheckBox checkBox ;
		UIView checkDescription ;
		void addCheckBox()
		{
			checkBox = new YCheckBox (334, 490, 34, 34);
			mainView.Add (checkBox);

			checkDescription = new UIView (new CGRect(384, 500, 297,16));
			mainView.Add (checkDescription);
			string str = "TO REGISTER ACCEPT THE TERMS OF USE ";
			checkDescription.Add (getTextField(str,22,35,0,0,297,16, 14));

			checkDescription.Alpha = 0;
		}


		//signUp button
		UIView gotoSignView ;
		void initGotoSign()
		{
			gotoSignView = new UIView (new CGRect(332, 542, 360, 56));
			mainView.Add (gotoSignView);

			gotoSignView.Add (new UIImageView(new CGRect(0, 0, 360, 56)){
				Image = UIImage.FromFile(filepath + "signup.png"),
				ContentMode = UIViewContentMode.ScaleAspectFit
			});

			//Go to Register
			Action action = ()=> {
				//main2photo(); 
				//activeView = 2 ;

				main2register();
				activeView =  3 ;

			};
			var tapgest = new UITapGestureRecognizer(action){NumberOfTapsRequired = 1};
			gotoSignView.AddGestureRecognizer (tapgest);
		}

		//Go to login 
		UIButton gotoLoginBT ;
		void initGotoLogin()
		{
			gotoLoginBT = new UIButton (UIButtonType.Custom);
			gotoLoginBT.Frame = new CGRect (556, 32, 138, 38);
			gotoLoginBT.SetImage (UIImage.FromFile(filepath+"login.png"), UIControlState.Normal);
			mainView.Add (gotoLoginBT);

			gotoLoginBT.TouchUpInside += (sender, e) => {
				main2login();
				activeView = 1 ;
			};
		}


		//facebook login
		UIView signFaceView ;
		void initSignFacebook()
		{
			signFaceView = new UIView (new CGRect(332, 628, 360,56));
			signFaceView.Alpha = 0;
			//mainView.Add (signFaceView);

			signFaceView.Add (new UIImageView(new CGRect(0, 0, 360, 56)){
				//Image = UIImage.FromFile(filepath + "signupface.png"),
				ContentMode = UIViewContentMode.ScaleAspectFit
			});

			//Go to Facebook
			 
			Action action = async ()=> {
				//await Authenticate(MobileServiceAuthenticationProvider.Facebook);
			};
			var tapgest = new UITapGestureRecognizer(action){NumberOfTapsRequired = 1};
			signFaceView.AddGestureRecognizer (tapgest);
		}

		#endregion


		#region Photo View


		YBlurMask blurMask ;
		LoadingView photoLoading ;

		void initPhotoView()
		{
			photoView = new UIView (new CGRect(1024,0,1024,768));
			//photoView.BackgroundColor = UIColor.Red;
			View.Add (photoView);
 
			initBackButton ();
			initDophotoButton ();
			initPhotoUserView ();
			initPhotoSelectView ();
			initColorView ();
		}

		void initBlurEffect()
		{
			blurMask = new YBlurMask ("efiles/fondologin.jpg", 0, 0, YConstants.DeviceWidht, YConstants.DeviceHeight);
			View.Add (blurMask);	
		}

		UIButton photoBackBT ;
		void initBackButton()
		{
			var img = new UIImageView (new CGRect(106,60,18,26));
			img.Image = UIImage.FromFile (filepath +  "atras.png");
			photoView.Add (img);

			photoBackBT = new UIButton (UIButtonType.Custom){Frame = new CGRect(96,50, 38,46)}; 
			photoView.Add (photoBackBT);
			photoBackBT.Layer.ZPosition = 100;
			photoBackBT.TouchUpInside += (sender, e) => {
				photo2main();
				activeView = 0 ;
			};
		}

		//photo selected correctly
		bool isNewPhotoReady = false ;
		UIButton photoDoBT ;
		void initDophotoButton()
		{
			var img = new UIImageView (new CGRect(900,60,18,26));
			img.Image = UIImage.FromFile (filepath +  "check.png");
			photoView.Add (img);

			photoDoBT = new UIButton (UIButtonType.Custom){Frame = new CGRect(890,50, 38,46)}; 
			photoView.Add (photoDoBT);
			photoDoBT.TouchUpInside += (sender, e) => {
				photoLoading = new LoadingView(View.Bounds);
				View.Add(photoLoading);

				if(isNewPhotoReady)
				{
					photo2register();
					activeView =3 ;
				}
				else isNewPhotoReady = true ;

			};
		}


		UIView photoUserView ;
		UIImageView photoUserImage ;
		void initPhotoUserView()
		{
			photoUserView = new UIView (new CGRect (412,162,200,180)){BackgroundColor = UIColor.Clear};
			photoView.Add (photoUserView);

			photoUserImage = new UIImageView (new CGRect(44,0, 112,112)){Image = UIImage.FromFile(filepath+"fotoperfil.png")};
			photoUserView.Add (photoUserImage);

			var textLabel = new UILabel (new CGRect (10, 126, 180,48)){TextAlignment = UITextAlignment.Center};
			textLabel.Text = "CHOOSE A PICTURE AND SELECT A COLOUR"; 
			textLabel.TextColor = UIColor.White;
			textLabel.Lines = 2;
			textLabel.Font = UIFont.FromName ("HelveticaNeue",14);
			photoUserView.Add (textLabel);
		}


		UIButton cameraChoose, galleryChoose ;
		void initPhotoSelectView()
		{
			cameraChoose = new UIButton (UIButtonType.Custom);
			cameraChoose.Frame  = new CGRect (86, 574, 60, 47);
			cameraChoose.SetImage(UIImage.FromFile (filepath+"camara.png"), UIControlState.Normal);
			cameraChoose.TouchUpInside+= (sender, e) => {
				photoLoading = new LoadingView(View.Bounds);
				View.Add(photoLoading);
			};
			photoView.Add (cameraChoose);
			 

			galleryChoose = new UIButton (UIButtonType.Custom);
			galleryChoose.Frame = new CGRect(881,574, 57, 52);
			galleryChoose.SetImage ( UIImage.FromFile (filepath+"biblioteca.png"), UIControlState.Normal);
			photoView.Add (galleryChoose);
			galleryChoose.TouchUpInside+= (sender, e) => {
				photoLoading = new LoadingView(View.Bounds);
				View.Add(photoLoading);
			};

			//choose camera  
 
		}


		UIView colorView ; 
		List<UIView> colorListView ;
		int IndexColor =0 ;
		void initColorView()
		{
			colorView = new UIView (new CGRect(86, 694, 852, 20)){BackgroundColor = UIColor.Clear};
			photoView.Add (colorView); 

			colorListView = new List<UIView> ();
			for (int i = 0; i < 6; i++) {
				var cview = new UIView (new CGRect (i*142,8,142,4));
				cview.BackgroundColor = YConstants.ColorList [i%6];
				colorListView.Add (cview);
				colorView.Add (cview);
			}

			resetColors (IndexColor);

			WireUpDragGestureRecognizer (); 
		}


		protected void WireUpDragGestureRecognizer ()
		{
			// create a new tap gesture
			UIPanGestureRecognizer gesture = new UIPanGestureRecognizer ();
			// wire up the event handler (have to use a selector)
			gesture.AddTarget ( () => { HandleDrag (gesture); });
			// add the gesture recognizer to the view
			colorView.AddGestureRecognizer (gesture);
		}

		protected void HandleDrag (UIPanGestureRecognizer recognizer)
		{  
			if (recognizer.State != (UIGestureRecognizerState.Cancelled | UIGestureRecognizerState.Failed
				| UIGestureRecognizerState.Possible)) { 
				//cololabel.Text = "" + recognizer.LocationInView (colorView).X; 
				int idx = (int)(recognizer.LocationInView (colorView).X /142) ;
				if (idx >= 0 && idx < 6)
					IndexColor = idx;
				resetColors (IndexColor);
			}
		}


		void resetColors(int index)
		{
			for (int i = 0; i < 6; i++) {
				colorListView [i].Frame = new CGRect (i * 142, 8, 142, 4);
				colorListView [i].Layer.CornerRadius = 1;
			}
			colorListView [index%6].Frame = new CGRect (index * 142, 5, 142, 10);
			colorListView [index%6].Layer.CornerRadius = 4;
		}

		#endregion


		#region Login View

		void initLoginView()
		{
			loginView = new UIView (new CGRect(1024,0,1024,768));
			View.Add (loginView);

			initStaticViews ();
			initInputLoginText ();
			initLogBackButton ();
		}

		UIButton loginBackBT ;
		void initLogBackButton()
		{
			var img = new UIImageView (new CGRect(106,60,18,26));
			img.Image = UIImage.FromFile (filepath +  "atras.png");
			loginView.Add (img);

			loginBackBT = new UIButton (UIButtonType.Custom){Frame = new CGRect(96,50, 38,46)};
			loginView.Add (loginBackBT);
			loginBackBT.TouchUpInside += (sender, e) => {
				login2main();
				activeView =0 ;
			};
		}


		UILabel loginTitle ;
		UIButton doLoginButton ;
		UIView forgotPassView ;
		LoadingView loginLoad ;
		void initStaticViews()
		{
			loginTitle = new UILabel (new CGRect(332,412,140,24)){TextColor = UIColor.White};
			loginTitle.Text = "Iniciar Sesion";
			loginTitle.Font = UIFont.FromName ("HelveticaNeue",21);
			loginView.Add (loginTitle);

			doLoginButton = new UIButton (UIButtonType.Custom);
			doLoginButton.SetImage(UIImage.FromFile(filepath+"logincolor.png"), UIControlState.Normal );
			doLoginButton.Frame = new CGRect (334, 624, 172,58);
			loginView.Add (doLoginButton);
			doLoginButton.TouchUpInside += (sender, e) => {
				var vm = ViewModel as LoginViewModel ;
				vm.LoginCommand.Execute(null);
				//View.Alpha = 0;

				loginLoad =  new LoadingView(View.Bounds) ;
				View.Add(loginLoad);
			};

			forgotPassView = new UIView (new CGRect(540,624,172,58));
			var text1 = new UILabel (new CGRect(0,0, 172, 22)){ 
				Font = UIFont.FromName("HelveticaNeue" , 16),
				Text = "FORGOT PASSWORD?",
				TextAlignment = UITextAlignment.Center,
				TextColor = UIColor.White
			};
			forgotPassView.Add (text1); 

			var text2 = new UILabel (new CGRect(0,34, 172, 22)){ 
				Font = UIFont.FromName("HelveticaNeue" , 16),
				Text = "CHANGE",
				TextAlignment = UITextAlignment.Center,
				TextColor = YConstants.ColorList[0]
			};
			forgotPassView.Add (text2);
			loginView.Add (forgotPassView);
		}

		UIView loginUserView, loginPassView ;
		UITextField loginUserInput, loginPassInput ;
		void initInputLoginText()
		{
			loginUserView = getRoundBoardView (334, 450, 358, 58);
			loginUserInput = getInputText (32, 20, 290, 22, "Usuario", "HelveticaNeue");
			loginUserInput.AutocapitalizationType = UITextAutocapitalizationType.None;
			loginUserView.Add (loginUserInput);
			loginView.Add (loginUserView);

			loginUserInput.Started+= loginInput_Started;
			loginUserInput.Ended+= loginInput_Ended;

			loginPassView = getRoundBoardView (334, 517, 358, 58);
			loginPassInput = getInputText (32, 20, 290, 22, "Contrasena", "HelveticaNeue");
			loginPassInput.AutocapitalizationType = UITextAutocapitalizationType.None;
			loginPassView.Add (loginPassInput);
			loginView.Add (loginPassView);

			loginPassInput.Started+= loginInput_Started;
			loginPassInput.Ended+= loginInput_Ended;
		}


		void loginInput_Ended (object sender, EventArgs e)
		{
			animateOffset (loginView, 0, 200, 0.25f);
		}

		void loginInput_Started (object sender, EventArgs e)
		{
			animateOffset (loginView, 0, -200, 0.25f);
		}

		#endregion

		#region Register View

		void initRegisterView()
		{
			regView = new UIView (new CGRect(1024,0,1024,768));
			View.Add(regView) ;

			initInputRegText ();
			initStaticViews2 ();
			initRegUserImage ();
			initRegBackButton ();
		}


		UIButton regBackBT ;
		void initRegBackButton()
		{
			var img = new UIImageView (new CGRect(106,60,18,26));
			img.Image = UIImage.FromFile (filepath +  "atras.png");
			regView.Add (img);

			regBackBT = new UIButton (UIButtonType.Custom){Frame = new CGRect(96,50, 38,46)};
			regView.Add (regBackBT);
			regBackBT.TouchUpInside += (sender, e) => {
				//register2photo();
				//activeView = 2 ;

				register2main();
				activeView =1 ;
			};
		}


		UILabel regTitle ;
		UIButton doRegButton ; 
		void initStaticViews2()
		{
			regTitle = new UILabel (new CGRect(332,299,140,24)){TextColor = UIColor.White};
			regTitle.Text = "Registro";
			regTitle.Font = UIFont.FromName ("HelveticaNeue",21);
			regView.Add (regTitle);

			doRegButton = new UIButton (UIButtonType.Custom);
			doRegButton.SetImage(UIImage.FromFile(filepath+"crearcuenta.png"), UIControlState.Normal );
			doRegButton.Frame = new CGRect (334, 625, 358, 58);
			regView.Add (doRegButton);
			doRegButton.TouchUpInside += (sender, e) => {
				photoLoading = new LoadingView(View.Bounds);
				View.Add(photoLoading);
			};
		}


		UIView regNameView, regUserView, regEmailView, regPassView ;
		UITextField regNameInput, regUserInput, regEmailInput, regPassInput ;
		void initInputRegText()
		{
			//nombre
			regNameView = getRoundBoardView (334, 337, 358, 58);
			regNameInput = getInputText (32, 20, 290, 22, "Nombre", "HelveticaNeue");
			regNameInput.AutocapitalizationType = UITextAutocapitalizationType.None;
			regNameView.Add (regNameInput);
			regView.Add (regNameView);
			regNameInput.Started+= RegInput_Started;
			regNameInput.Ended+= RegInput_Ended;
 
			//user
			regUserView = getRoundBoardView (334, 404, 358, 58);
			regUserInput = getInputText (32, 20, 290, 22, "Usuario", "HelveticaNeue");
			regUserView.Add (regUserInput);
			regUserInput.AutocapitalizationType = UITextAutocapitalizationType.None;
			regView.Add (regUserView);
			regUserInput.Started+= RegInput_Started;
			regUserInput.Ended+= RegInput_Ended;

			//email
			regEmailView = getRoundBoardView (334, 472, 358, 58);
			regEmailInput = getInputText (32, 20, 290, 22, "Direccion de correo", "HelveticaNeue");
			regEmailView.Add (regEmailInput);
			regView.Add (regEmailView);
			regEmailInput.Started+= RegInput_Started;
			regEmailInput.Ended+= RegInput_Ended;

			//password
			regPassView = getRoundBoardView (334, 540, 358, 58);
			regPassInput = getInputText (32, 20, 290, 22, "Contrasena", "HelveticaNeue");
			regPassInput.AutocapitalizationType = UITextAutocapitalizationType.None;
			regPassView.Add (regPassInput);
			regView.Add (regPassView);
			regPassInput.Started+= RegInput_Started;
			regPassInput.Ended+= RegInput_Ended;
		}

		void RegInput_Ended (object sender, EventArgs e)
		{
			animateOffset (regView, 0, 224, 0.25f);
		}

		void RegInput_Started (object sender, EventArgs e)
		{
			animateOffset (regView, 0, -224, 0.25f);
		}


		UIImageView regUserImage ;
		void initRegUserImage()
		{
			//regUserImage = getimage (457,164,110,110,filepath+"introduceyourself.png");
			//regView.Add (regUserImage);
		}


		#endregion

		#region auxiliar functions 


		CABasicAnimation animateFromTo(UIView v, float dur, string path , float fromx, float fromy , float tox, float toy)
		{ 
			var l_animation = CABasicAnimation.FromKeyPath (path);//position
			l_animation.TimingFunction = CAMediaTimingFunction.FromName (CAMediaTimingFunction.EaseIn); 
			l_animation.From = NSValue.FromPointF (new PointF (fromx, fromy));
			l_animation.To = NSValue.FromPointF (new PointF (tox,toy));
			l_animation.Duration = 0.6;
			l_animation.RemovedOnCompletion = false;

			//l_animation.AnimationStopped += faction;
			v.Layer.AddAnimation (l_animation, "position");
			v.Layer.Position = new PointF (tox,toy) ;

			return l_animation;
		}


		UIImageView getimage(float x , float y, float width , float height, string source )
		{
			var image = new UIImageView (new RectangleF(x,y,width,height)); 
			image.Layer.CornerRadius = image.Frame.Size.Width / 2;
			image.Layer.BorderWidth = 6.0f ;
			image.Layer.BorderColor = UIColor.White.CGColor;
			image.ContentMode = UIViewContentMode.ScaleToFill;
			image.Layer.MasksToBounds = true;
			image.Image = UIImage.FromFile (source);
			image.SetNeedsDisplay ();  
			return image;
		}

		UIImageView getimagefrombytes(nfloat x , nfloat y, nfloat width , float height, byte[] bytes )
		{ 
			var image = new UIImageView (new CGRect(x,y,width,height));
			var img = ToUIImage (bytes);
			if (img != null)
				image.Image = img;
			image.Layer.CornerRadius = image.Frame.Size.Width / 2;
			image.Layer.BorderWidth = 6.0f ;
			image.Layer.BorderColor = UIColor.White.CGColor;
			image.Layer.MasksToBounds = true;
			image.SetNeedsDisplay ();
			return image;
		}

		public UIImage ToUIImage (byte[] bytes)
		{
			if (bytes == null)
				return null;

			using (var data = NSData.FromArray (bytes)) {
				return UIImage.LoadFromData (data);
			}
		}


		private UITextField getInputText( nfloat x, nfloat y , nfloat w, nfloat h,string ph ,string font)
		{
			var textfield = new UITextField (new CGRect(x,y,w,h));
			textfield.Placeholder = ph;
			textfield.Font = UIFont.FromName (font,22);
			textfield.TextColor = UIColor.White;
			textfield.BackgroundColor = UIColor.Clear;
			return textfield;
		}

		private UIView getRoundBoardView(nfloat x, nfloat y,nfloat w, nfloat h )
		{
			var view = new UIView (new CGRect( x,y,w,h));  
			view.BackgroundColor = UIColor.FromRGBA (255,255,255,100);
			view.Layer.CornerRadius = 4;
			return  view;
		}


		void animateOffset(UIView v , float xoffset, float yoffset, double dur)
		{
			UIView.Animate( dur, 0, UIViewAnimationOptions.CurveEaseIn ,
				() => { v.Center =  new PointF ((float)v.Center.X  + xoffset,
					(float)v.Center.Y+ yoffset);}, null ); 
		}


		void animateAlpha(UIView v, double dur, nfloat alpha)
		{
			UIView.Animate(dur ,0, UIViewAnimationOptions.CurveEaseIn  , () =>
				{ v.Alpha = alpha; }, null);
		}


		//multicolor tetxfield
		UITextField getTextField(string text , int p1, int p2, nfloat x, nfloat y, nfloat w, nfloat h, nfloat tsize)
		{
			var firstAttributes = new UIStringAttributes {
				ForegroundColor = YConstants.ColorList[0] ,
				BackgroundColor = UIColor.Clear,
				Font = UIFont.FromName("HelveticaNeue", tsize)
			};

			var secondAttributes = new UIStringAttributes {
				ForegroundColor = UIColor.White,
				BackgroundColor = UIColor.Clear,
				Font = UIFont.FromName("HelveticaNeue", tsize)
			};

			// create the text field
			var  textField1 = new UITextField (new CoreGraphics.CGRect (x, y, w, h)); 
			textField1.TextAlignment = UITextAlignment.Center;
			View.Add (textField1);

			// set different ranges to different styling!
			var prettyString = new NSMutableAttributedString (text);
			prettyString.SetAttributes (firstAttributes.Dictionary, new NSRange (0, p1));
			prettyString.SetAttributes (secondAttributes.Dictionary, new NSRange (p1, p2-p1)); 

			// assign the styled text
			textField1.AttributedText = prettyString;

			return textField1;
		}



		#endregion

	 
 

		private void alertMessage (string message)
		{
			var alert = new UIAlertView ("Error", message, null, "OK", null);
			alert.Show ();
		}

		public override void ViewWillAppear (bool animated)
		{
			 
			base.ViewWillAppear (animated);
			NavigationController.SetNavigationBarHidden (true, true);

			if(loginLoad!=null)
				loginLoad.Hide ();
			if(photoLoading != null)
				photoLoading.Hide ();
			//UIView.Transition(this.View,  , 0.8f, UIViewAnimationOptions.TransitionFlipFromLeft, null);
		} 


		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);

		}
	}
}