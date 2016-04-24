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

namespace MLearning.iPhone
{
	[Register("LoginView")]
	public class LoginView : MvxViewController
	{
		/// <summary>
		/// states for the views
		/// </summary>

		int social_state = 1 , login_state = 1 , sign_state =1, slogin_state =1 ; 
		 
		float DeviceHeight = 568.0f , DeviceWidth = 320.0f;
		UIView LogoView ;
		UIView FacebookView, TwitterView, LinkedView , DownView;
		UIButton FacebookButton, TwitterButton, LinkedButton, LoginButton, SignupButton ;
		UILabel ConectarField ;

		LoadingOverlay LoadingView  ;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			View.BackgroundColor = UIColor.White;

			// ios7 layout
			if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
				EdgesForExtendedLayout = UIRectEdge.None;



				
			var backimage = new UIImageView (new CGRect (0, 0, 320, 568));
			backimage.ContentMode = UIViewContentMode.ScaleToFill;	
			backimage.Image = UIImage.FromFile ("MLResources/Icons/background.png");
			View.Add (backimage);

			//initial animation of the logo
			initLogoView ();
			logo_animation (); 

		 	//first view
			initSocialButtons();
			//inicio view
			initInicioSesion ();

			initRegister ();
			initCameraMenu ();

			//R_UserText , R_PassText,R_CorreoText , R_NombreText, R_ApellidoText ;
			//UserLoginTF, PassLoginTF ;
			var tap = new UITapGestureRecognizer (() => 
				{
					R_UserText.ResignFirstResponder();
					R_PassText.ResignFirstResponder();
					R_CorreoText.ResignFirstResponder();
					R_NombreText.ResignFirstResponder();
					R_ApellidoText.ResignFirstResponder();
					UserLoginTF.ResignFirstResponder();
					PassLoginTF.ResignFirstResponder();
				} );
			View.AddGestureRecognizer (tap); 

			//binding UserLoginTF, PassLoginTF ;
			var set = this.CreateBindingSet<LoginView, LoginViewModel>(); 
			set.Bind(UserLoginTF).To(vm => vm.Username);   
			set.Bind (PassLoginTF).To (vm=>vm.Password);
			set.Apply ();
		}
			
		#region Animations

		private void logo_animation()
		{ 
			UIView.Animate (0.6, 0, UIViewAnimationOptions.CurveEaseIn, () => {
				LogoView.Transform = CGAffineTransform.MakeScale (0.53f, 0.53f);
			}, null);

			var t_animation = CABasicAnimation.FromKeyPath ("position");
			t_animation.TimingFunction = CAMediaTimingFunction.FromName (CAMediaTimingFunction.EaseIn); 
			t_animation.From = NSValue.FromPointF (new PointF ((float)LogoView.Center.X,(float)LogoView.Center.Y));
			t_animation.To = NSValue.FromPointF (new PointF ((float)LogoView.Center.X,160.0f));
			t_animation.Duration = 0.6;
			t_animation.RemovedOnCompletion = true;
			t_animation.AutoReverses = false;
			t_animation.AnimationStopped += (s, e) => {
				//LogoView.Layer.Position = new PointF ((float)LogoView.Center.X,160.0f);
				//input of first view
				socialanimate(-1.0f*DeviceWidth , true , true) ;
				social_state =0 ;
			};
			LogoView.Layer.AddAnimation (t_animation, "position"); 
			LogoView.Layer.Position = new PointF ((float)LogoView.Center.X,160.0f);
		}


		 


		private void socialanimate(float offset , bool down_on , bool logo_on)
		{
			if (!down_on) {
				UIView.Animate (0.25, 0, UIViewAnimationOptions.CurveEaseIn ,
					() => { DownView.Center =  new PointF ((float)DownView.Center.X ,
						(float)DownView.Center.Y+ 100);}, null ); 
			}

			if(!logo_on)
				UIView.Animate(0.25,0, UIViewAnimationOptions.CurveEaseIn  , () =>
					{ LogoView.Alpha = 0; }, null);
			
			UIView.Animate (0.36, 0, UIViewAnimationOptions.CurveEaseIn ,
				() => { ConectarField.Center =  new PointF ((float)ConectarField.Center.X + offset,
					(float)ConectarField.Center.Y);}, null );
			
			UIView.Animate (0.44, 0, UIViewAnimationOptions.CurveEaseIn ,
				() => { FacebookView.Center =  new PointF ((float)FacebookView.Center.X + offset,
					(float)FacebookView.Center.Y);}, null );

			UIView.Animate (0.52, 0, UIViewAnimationOptions.CurveEaseIn ,
				() => { TwitterView.Center =  new PointF ((float)TwitterView.Center.X + offset,
					(float)TwitterView.Center.Y);}, null );

			var l_animation = CABasicAnimation.FromKeyPath ("position");
			l_animation.TimingFunction = CAMediaTimingFunction.FromName (CAMediaTimingFunction.EaseIn); 
			l_animation.From = NSValue.FromPointF (new PointF ((float)LinkedView.Center.X ,(float)LinkedView.Center.Y));
			l_animation.To = NSValue.FromPointF (new PointF ((float)LinkedView.Center.X + offset,(float)LinkedView.Center.Y));
			l_animation.Duration = 0.6;
			l_animation.RemovedOnCompletion = false;
		
			l_animation.AnimationStopped += (s, e) => {
				//LinkedView.Layer.Position = new PointF ((float)LinkedView.Center.X + offset,(float)LinkedView.Center.Y) ;
				//input botton buttons
				if(down_on)
				UIView.Animate (0.25, 0, UIViewAnimationOptions.CurveEaseIn ,
					() => { DownView.Center =  new PointF ((float)DownView.Center.X ,
						(float)DownView.Center.Y- 100);}, null );

				if(logo_on)
					UIView.Animate(0.2,0, UIViewAnimationOptions.CurveEaseIn  , () =>
						{ LogoView.Alpha = 1; }, null);
			};
			LinkedView.Layer.AddAnimation (l_animation, "position");
			LinkedView.Layer.Position = new PointF ((float)LinkedView.Center.X + offset,(float)LinkedView.Center.Y) ;
		}

		//inicio sesion 

		private void  loginanimate(float offset , bool logo_on)
		{ 
			if(!logo_on)
				UIView.Animate(0.2,0, UIViewAnimationOptions.CurveEaseIn  , () =>
					{ LogoView.Alpha = 0; }, null);
			//Animations
			UIView.Animate (0.36, 0, UIViewAnimationOptions.CurveEaseIn ,
				() => { IniciarField.Center =  new PointF ((float)IniciarField.Center.X + offset,
					(float)IniciarField.Center.Y);}, null );

			UIView.Animate (0.44, 0, UIViewAnimationOptions.CurveEaseIn ,
				() => { LoginUserView.Center =  new PointF ((float)LoginUserView.Center.X + offset,
					(float)LoginUserView.Center.Y);}, null );

			UIView.Animate (0.52, 0, UIViewAnimationOptions.CurveEaseIn ,
				() => { LoginPassView.Center =  new PointF ((float)LoginPassView.Center.X + offset,
					(float)LoginPassView.Center.Y);}, null );

			UIView.Animate (0.6, 0, UIViewAnimationOptions.CurveEaseIn ,
				() => { BackLoginBt.Center =  new PointF ((float)BackLoginBt.Center.X + offset,
					(float)BackLoginBt.Center.Y);}, null );

			var l_animation = CABasicAnimation.FromKeyPath ("position");
			l_animation.TimingFunction = CAMediaTimingFunction.FromName (CAMediaTimingFunction.EaseIn); 
			l_animation.From = NSValue.FromPointF (new PointF ((float)GoToRegisterBt.Center.X ,(float)GoToRegisterBt.Center.Y));
			l_animation.To = NSValue.FromPointF (new PointF ((float)GoToRegisterBt.Center.X + offset,(float)GoToRegisterBt.Center.Y));
			l_animation.Duration = 0.6;
			l_animation.RemovedOnCompletion = false;

			l_animation.AnimationStopped += (s, e) => {
				//GoToRegisterBt.Layer.Position = new PointF ((float)GoToRegisterBt.Center.X + offset,(float)GoToRegisterBt.Center.Y) ;
				//logo
				if(logo_on)
					UIView.Animate(0.2,0, UIViewAnimationOptions.CurveEaseIn  , () =>
						{ LogoView.Alpha = 1; }, null);
			};
			GoToRegisterBt.Layer.AddAnimation (l_animation, "position");
			GoToRegisterBt.Layer.Position = new PointF ((float)GoToRegisterBt.Center.X + offset,(float)GoToRegisterBt.Center.Y) ;
		}

		//register
		//UIView RegisterPhoto , R_UserView , R_PassView,R_CorreoView , R_NombreView, R_ApellidoView, R_ProfView ;

		private void registeranimate(float offset, bool photo_on)
		{
			if (!photo_on) {
				UIView.Animate (0.25, 0, UIViewAnimationOptions.CurveEaseIn ,
					() => { RegisterPhoto.Center =  new PointF ((float)RegisterPhoto.Center.X ,
						(float)RegisterPhoto.Center.Y- 150);}, null );
			}
			//
			UIView.Animate (0.36, 0, UIViewAnimationOptions.CurveEaseIn ,
				() => { R_UserView.Center =  new PointF ((float)R_UserView.Center.X + offset,
					(float)R_UserView.Center.Y);}, null );

			UIView.Animate (0.42, 0, UIViewAnimationOptions.CurveEaseIn ,
				() => { R_PassView.Center =  new PointF ((float)R_PassView.Center.X + offset,
					(float)R_PassView.Center.Y);}, null );
 
			UIView.Animate (0.48, 0, UIViewAnimationOptions.CurveEaseIn ,
				() => { R_CorreoView.Center =  new PointF ((float)R_CorreoView.Center.X + offset,
					(float)R_CorreoView.Center.Y);}, null );

			UIView.Animate (0.54, 0, UIViewAnimationOptions.CurveEaseIn ,
				() => { R_NombreView.Center =  new PointF ((float)R_NombreView.Center.X + offset,
					(float)R_NombreView.Center.Y);}, null );

			UIView.Animate (0.60, 0, UIViewAnimationOptions.CurveEaseIn ,
				() => { R_ApellidoView.Center =  new PointF ((float)R_ApellidoView.Center.X + offset,
					(float)R_ApellidoView.Center.Y);}, null );
			UIView.Animate (0.64, 0, UIViewAnimationOptions.CurveEaseIn ,
				() => { R_ProfView.Center =  new PointF ((float)R_ProfView.Center.X + offset,
					(float)R_ProfView.Center.Y);}, null );

			var l_animation = CABasicAnimation.FromKeyPath ("position");
			l_animation.TimingFunction = CAMediaTimingFunction.FromName (CAMediaTimingFunction.EaseIn); 
			l_animation.From = NSValue.FromPointF (new PointF ((float)RegisterBackBt.Center.X ,(float)RegisterBackBt.Center.Y));
			l_animation.To = NSValue.FromPointF (new PointF ((float)RegisterBackBt.Center.X + offset,(float)RegisterBackBt.Center.Y));
			l_animation.Duration = 0.70;
			l_animation.RemovedOnCompletion = false;

			l_animation.AnimationStopped += (s, e) => {
				//RegisterBackBt.Layer.Position = new PointF ((float)RegisterBackBt.Center.X + offset,(float)RegisterBackBt.Center.Y) ;
				//input botton buttons
				if(photo_on)
					UIView.Animate (0.25, 0, UIViewAnimationOptions.CurveEaseIn ,
						() => { RegisterPhoto.Center =  new PointF ((float)RegisterPhoto.Center.X ,
							(float)RegisterPhoto.Center.Y + 150);}, null );
			};
			RegisterBackBt.Layer.AddAnimation (l_animation, "position");
			RegisterBackBt.Layer.Position = new PointF ((float)RegisterBackBt.Center.X + offset,(float)RegisterBackBt.Center.Y) ;
		}


	 
		#endregion


		#region Functions

		private void initLogoView()
		{
			LogoView = new UIView (new CGRect (75, 200, 172, 172));
			var img_logo = new UIImageView (new RectangleF(0,0,172,172)); 
			img_logo.ContentMode = UIViewContentMode.ScaleToFill;
			LogoView.ContentMode = UIViewContentMode.ScaleToFill;
			img_logo.Image = UIImage.FromFile ("MLResources/Icons/logo.png");
			LogoView.Add (img_logo); 

			View.Add(LogoView);
		}


		private void initSocialButtons()
		{
			ConectarField = new UILabel (new CGRect (100 +  DeviceWidth,244, 120, 20));
			ConectarField.Text = "Conectar con";
			ConectarField.TextAlignment = UITextAlignment.Center;
			ConectarField.TextColor = UIColor.Yellow;
			ConectarField.Font = UIFont.FromName ("HelveticaNeue", 18);
			View.Add (ConectarField);

			//social views
			FacebookView = getRoundImageView( 30 + DeviceWidth , 285, "MLResources/Icons/icon_face.png" , "Facebook") ;
			View.Add (FacebookView);
			TwitterView = getRoundImageView( 30 + DeviceWidth , 345, "MLResources/Icons/icon_twitter.png" , "Twitter") ;
			View.Add (TwitterView);
			LinkedView = getRoundImageView( 30 + DeviceWidth , 402, "MLResources/Icons/icon_in.png", "LinkedIn") ;
			View.Add (LinkedView);

			FacebookButton = new UIButton (new CGRect (0,0,260,55));
			FacebookView.Add (FacebookButton);
			FacebookButton.TouchUpInside += delegate {
			};

			TwitterButton = new UIButton (new CGRect (0,0,260,55));
			TwitterView.Add (FacebookButton);
			TwitterButton.TouchUpInside += delegate {
			};

			LinkedButton = new UIButton (new CGRect (0,0,260,55));
			LinkedView.Add (FacebookButton);
			LinkedButton.TouchUpInside += delegate {
			};

			//botton buttons
			DownView = new UIView(new CGRect(0,488 + 100 , 320, 80)) ;
			View.Add (DownView);

			var leftline = new UIView(new CGRect( 22,10,120,1)){BackgroundColor = UIColor.Red} ;
			DownView.Add (leftline);
			var rightline = new UIView(new CGRect( 178,10,120,1)){BackgroundColor = UIColor.Red} ;
			DownView.Add (rightline);

			LoginButton = new UIButton (new CGRect(40, 35, 100, 30));
			LoginButton.SetTitle ("Login", UIControlState.Normal); 
			LoginButton.SetTitleColor(UIColor.Yellow, UIControlState.Normal) ;
			DownView.Add (LoginButton);
			LoginButton.TouchUpInside += delegate {
				loginanimate(-1.0f*DeviceWidth,true) ;
				socialanimate(-1.0f*DeviceWidth,false, true) ;
				social_state =-1;
				login_state =0 ;
			};


			SignupButton = new UIButton (new CGRect(180, 35, 100, 30));
			SignupButton.SetTitle ("Sign Up", UIControlState.Normal);
			SignupButton.SetTitleColor(UIColor.Yellow, UIControlState.Normal) ;
			DownView.Add (SignupButton);
			SignupButton.TouchUpInside += delegate {
				registeranimate(-1.0f*DeviceWidth, true) ;
				socialanimate(-1.0f*DeviceWidth,false, false) ;
				social_state =-1;
				sign_state = 0 ;
			};
		}


		//inicio sesion
		UITextField UserLoginTF, PassLoginTF ;
		UILabel IniciarField ;
		UIButton DoLoginBt , GoToRegisterBt , BackLoginBt ;
		UIView LoginUserView, LoginPassView ;


		private void initInicioSesion()
		{
			IniciarField = new UILabel (new CGRect (100 +  DeviceWidth ,244, 120, 20));
			IniciarField.Text = "Iniciar Sesion";
			IniciarField.TextAlignment = UITextAlignment.Center;
			IniciarField.TextColor = UIColor.Yellow;
			IniciarField.Font = UIFont.FromName ("HelveticaNeue", 18);
			View.Add (IniciarField);

			//login views 

			LoginUserView = getRoundImageView (30 + DeviceWidth, 300, null, null);
			View.Add (LoginUserView);
			LoginPassView = getRoundImageView (30 + DeviceWidth, 360, "MLResources/Icons/icon_enter.png", null);
			View.Add (LoginPassView);
			//login text
			UserLoginTF = getInputText("Usuario");
			LoginUserView.Add (UserLoginTF);
			PassLoginTF = getInputText ("Contrasena");
			LoginPassView.Add (PassLoginTF);

			DoLoginBt = new UIButton (new CGRect (205,0,55,55));
			LoginPassView.Add (DoLoginBt);
			DoLoginBt.TouchUpInside += delegate {
				//navigate to mainview ?correct
				var command = ((LoginViewModel)this.ViewModel).LoginCommand;
				command.Execute(null); 
				LoadingView = new LoadingOverlay(new CGRect(0,0,320,568));
				View.Add(LoadingView);
			};



			//registrarse button
			GoToRegisterBt = new UIButton (new CGRect(200 + DeviceWidth, 426, 100, 22));
			GoToRegisterBt.SetTitle ("Register", UIControlState.Normal); 
			GoToRegisterBt.SetTitleColor(UIColor.Yellow, UIControlState.Normal) ;
			View.Add (GoToRegisterBt);
			GoToRegisterBt.TouchUpInside += delegate {
				//go to register/ sign up
				registeranimate(-1.0f*DeviceWidth, true) ;
				loginanimate(-1.0f*DeviceWidth,false) ;
				login_state =-1;
				sign_state = 0 ;
			};

			//registrarse button Back
			BackLoginBt = new UIButton (new CGRect(40 + DeviceWidth, 426, 100, 22));
			BackLoginBt.SetTitle ("Back", UIControlState.Normal); 
			BackLoginBt.SetTitleColor(UIColor.Yellow, UIControlState.Normal) ;
			View.Add (BackLoginBt);
			BackLoginBt.TouchUpInside += delegate {
				//go to back view
				 
				socialanimate(DeviceWidth,true, true) ;
				social_state =0 ; 
				loginanimate(DeviceWidth,true) ;
				login_state = 1 ;
			};
		}
	 

		//register
		UIView RegisterPhoto , R_UserView , R_PassView,R_CorreoView , R_NombreView, R_ApellidoView, R_ProfView ;
		UITextField R_UserText , R_PassText,R_CorreoText , R_NombreText, R_ApellidoText ;
		UIButton DoRegisterBt, RegisterBackBt, RCameraBt ;
		bool _iscameraopen = false ;

		private void initRegister()
		{
			RegisterPhoto = new UIView (new CGRect(119, 56 - 150, 82,82));//150 -> to take it up
			var img_photo = new UIImageView (new RectangleF(0,0,82,82)); 
			img_photo.ContentMode = UIViewContentMode.ScaleToFill;
			RegisterPhoto.ContentMode = UIViewContentMode.ScaleToFill;
			img_photo.Image = UIImage.FromFile ("MLResources/Icons/icon_photo.png");
			RegisterPhoto.Add (img_photo); 
			View.Add (RegisterPhoto);

			//user views
			R_UserView = getRoundImageView(30 + DeviceWidth,160 , null, null ) ;
			View.Add (R_UserView);
			R_PassView = getRoundImageView(30 + DeviceWidth,220 , null, null ) ;
			View.Add (R_PassView);
			R_CorreoView = getRoundImageView(30 + DeviceWidth,280 , null, null ) ;
			View.Add (R_CorreoView);
			R_NombreView = getRoundImageView(30 + DeviceWidth,340 , null, null ) ;
			View.Add (R_NombreView);
			R_ApellidoView = getRoundImageView(30 + DeviceWidth,400 , null, null ) ;
			View.Add (R_ApellidoView);
			R_ProfView = getRoundImageView(30 + DeviceWidth,460 , "MLResources/Icons/icon_enter.png", "Soy profesor?" ) ;
			View.Add (R_ProfView);
			DoRegisterBt = new UIButton (new CGRect (205,0,55,55));
			R_ProfView.Add (DoRegisterBt);
			DoRegisterBt.TouchUpInside += delegate {
				//show r_login
			};

			R_UserText = getInputText ("Usuario");
			R_UserView.Add (R_UserText);
			R_PassText = getInputText ("Contrasena");
			R_PassView.Add (R_PassText);
			R_CorreoText = getInputText ("Correo");
			R_CorreoView.Add(R_CorreoText);
			R_NombreText = getInputText ("Nombre");
			R_NombreView.Add (R_NombreText);
			R_ApellidoText = getInputText ("Apellido");
			R_ApellidoView.Add (R_ApellidoText);

			//registrarse button Back
			RegisterBackBt = new UIButton (new CGRect(40 + DeviceWidth, 532, 100, 22));
			RegisterBackBt.SetTitle ("Back", UIControlState.Normal); 
			RegisterBackBt.SetTitleColor(UIColor.Yellow, UIControlState.Normal) ;
			View.Add (RegisterBackBt);
			RegisterBackBt.TouchUpInside += delegate {
				//go to back view
				if(login_state == -1)
				{
					loginanimate(DeviceWidth,true); 
					login_state =0 ;
				}
				else
				{
					socialanimate(DeviceWidth,true, true) ;
					social_state =0 ;
				}
				registeranimate(DeviceWidth,false) ;
				sign_state = 1 ;
			};
			//camera
			RCameraBt = new UIButton(new CGRect(0,0,82,82));
			RegisterPhoto.Add (RCameraBt);
			RCameraBt.TouchUpInside += delegate {
				if(_iscameraopen)
				{
					UIView.Animate (0.24, 0, UIViewAnimationOptions.CurveEaseIn ,
						() => { CameraMenu.Center =  new PointF ((float)CameraMenu.Center.X ,
							(float)CameraMenu.Center.Y  + 250);}, null );
				}
				else
				{
					UIView.Animate (0.24, 0, UIViewAnimationOptions.CurveEaseIn ,
						() => { CameraMenu.Center =  new PointF ((float)CameraMenu.Center.X ,
							(float)CameraMenu.Center.Y  - 250);}, null );
				}
				_iscameraopen = !_iscameraopen ;
			};
		}

		UIView CameraMenu ;
		UIButton CamButton, BibButton, CancelCamBt ;

		private void initCameraMenu()
		{
			CameraMenu = new UIView (new CGRect (0, 382 + 250, 320, 186)); //250 to hide 
			CameraMenu.BackgroundColor = UIColor.FromRGBA (0.0f, 0.0f, 0.0f, 0.4f);
			View.Add (CameraMenu);

			var title = new UILabel (new CGRect(24,20,210,20));
			title.TextColor = UIColor.White;
			title.Text = "Elegir imagen de peril";
			CameraMenu.Add (title);

			var cimage = new UIImageView (new CGRect (60, 54, 60, 60));
			cimage.Image = UIImage.FromFile ("MLResources/Icons/icon_camara.png");
			cimage.ContentMode = UIViewContentMode.ScaleToFill;
			CameraMenu.Add (cimage); 

			var bimage = new UIImageView (new CGRect (200, 54, 60, 60));
			bimage.Image = UIImage.FromFile ("MLResources/Icons/icon_biblioteca.png");
			bimage.ContentMode = UIViewContentMode.ScaleToFill;
			CameraMenu.Add (bimage); 

			CamButton = new UIButton (new CGRect (60, 54, 60, 60));
			CameraMenu.Add (CamButton);
			CamButton.TouchUpInside += delegate {

			};

			BibButton = new UIButton (new CGRect (200, 54, 60, 60));
			CameraMenu.Add (BibButton);
			BibButton.TouchUpInside += delegate {

			};
			//labels
			var clabel = new UILabel(new CGRect(50,116, 80,20));
			clabel.TextColor = UIColor.White;
			clabel.Text = "Camara";
			CameraMenu.Add (clabel);

			var blabel = new UILabel(new CGRect(190,116, 80,20));
			blabel.TextColor = UIColor.White;
			blabel.Text = "Biblioteca";
			CameraMenu.Add (blabel);

			var down_square = new UIView (new CGRect(0,146, 320,40));
			down_square.BackgroundColor = UIColor.FromRGBA (0.0f, 0.0f, 0.0f, 0.6f);
			CameraMenu.Add (down_square);

			var lcancel = new UILabel (new CGRect(100,10,120,20));
			lcancel.TextColor = UIColor.White;
			lcancel.Text = "Cancelar";
			lcancel.TextAlignment = UITextAlignment.Center;
			down_square.Add (lcancel);

			CancelCamBt = new UIButton (new CGRect (0, 0, 320, 40));
			down_square.Add (CancelCamBt);
			CancelCamBt.TouchUpInside += delegate {
				if(_iscameraopen)
				{
					UIView.Animate (0.24, 0, UIViewAnimationOptions.CurveEaseIn ,
						() => { CameraMenu.Center =  new PointF ((float)CameraMenu.Center.X ,
							(float)CameraMenu.Center.Y  + 250);}, null );
					_iscameraopen=false;
				}
			};
		}


		private UITextField getInputText(string ph)
		{
			var textfield = new UITextField (new CGRect(20,18,200,20));
			textfield.Placeholder = ph;
			textfield.BackgroundColor = UIColor.Clear;
			return textfield;
		}

		private UIView getRoundImageView(float x, float y,string path, string text)
		{
			var view = new UIView (new CGRect( x,y,260,55));
			var backimage = new UIImageView (new CGRect (0, 0, 260, 55));
			backimage.Image = UIImage.FromFile ("MLResources/Icons/textback.png");
			backimage.ContentMode = UIViewContentMode.ScaleToFill;
			view.Add (backimage); 

			if (path != null) {
				var btimage = new UIImageView (new CGRect (208, 4, 47, 47));
				btimage.Image = UIImage.FromFile (path);
				btimage.ContentMode = UIViewContentMode.ScaleToFill;
				view.Add (btimage); 
			}

			if (text != null) {
				var textlabel = new UILabel (new CGRect (20,18 , 120, 20));
				textlabel.Text = text;
				view.Add (textlabel);
			}
			return  view;
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
			if(LoadingView!=null)
				LoadingView.Hide ();
		}

	}
}