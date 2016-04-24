using System; 
using System.Drawing;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;

namespace YComponents.YWidgets
{
	public class HomeworkThumb : UIView
	{
		public HomeworkThumb (nfloat px, nfloat py)
		{
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;

			x = px;
			y = py;

			Frame = new CGRect (x,y,w,h);
			initView ();
		}


		nfloat x, y, w = 234, h = 150 ;

		UIView downView ;
		UILabel descriptionLabel , questLabel , nameLabel ;
		UIImageView askImage , selectImage ;
		void initView ()
		{
			BackgroundColor = UIColor.FromRGBA (240,240,240,240);

			descriptionLabel = new UILabel (new CGRect(22, 16, 186, 68))
			{
				TextColor = UIColor.Black,
				Text = "Descripcion del primer texto en este learning object",
				Font = UIFont.FromName ("HelveticaNeue-Light",13),
				Lines = 5,
				TextAlignment = UITextAlignment.Left
			} ;
			Add (descriptionLabel);

			questLabel = new UILabel (new CGRect(160, 86, 64, 16))
			{
				TextColor = UIColor.Gray,
				Text = "10 Preguntas",
				Font = UIFont.FromName ("HelveticaNeue-Light",10),
				TextAlignment = UITextAlignment.Right
			} ;
			Add (questLabel);

			askImage = new UIImageView (new CGRect(144,86,14,14));
			askImage.Image = UIImage.FromFile ("efiles/muro/preguntastareas.png");
			Add (askImage);

			///down view

			downView = new UIView (new CGRect(0,h-46  , w,46)) {BackgroundColor = WidgetsUtil.themes[0]};
			Add (downView);

			nameLabel = new UILabel (new CGRect(22, 16, 154, 18))
			{
				TextColor = UIColor.White,
				Text = "Cuestionario 1",
				Font = UIFont.FromName ("HelveticaNeue",17),
				TextAlignment = UITextAlignment.Left
			} ;
			downView.Add (nameLabel);


			selectImage = new UIImageView (new CGRect(195,8,28,28));
			selectImage.Image = UIImage.FromFile ("efiles/muro/tareacompleta.png");

			downView.Add (selectImage);

		}


		void select()
		{
			//set the image  
			if (isSelected) {
				selectImage.Image = UIImage.FromFile ("efiles/muro/tareacompleta.png");

			} else {
				selectImage.Image = UIImage.FromFile ("efiles/muro/tareaincompleta.png");

			}
		}

		bool isSelected = false ;
		public bool IsSelected
		{
			get { return isSelected;}
			set { isSelected = value;}
		}


		public void SetValues(string title, string description, int number, bool state)
		{
			nameLabel.Text = title;
			questLabel.Text = number + " preguntas";
			descriptionLabel.Text = description;
			isSelected = state;
			select ();
		}


		public UIColor ThemeColor
		{
			set
			{
				downView.BackgroundColor = value;
			}
		}



	}
}

