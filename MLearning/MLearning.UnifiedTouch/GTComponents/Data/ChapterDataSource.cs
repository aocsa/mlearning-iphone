using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit; 

namespace DataSource
{
    public class ChapterDataSource : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Title"));
            }
        }


        private string _author;

        public string Author
        {
            get { return _author; }
            set
            {
                _author = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Author"));
            }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Description"));
            }
        }

        private ObservableCollection<SectionDataSource> _sections = new ObservableCollection<SectionDataSource>();

        public ObservableCollection<SectionDataSource> Sections
        {
            get { return _sections; }
            set
            {
                _sections = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Sections"));
            }
        }


		private UIColor _chaptercolor;

		public UIColor ChapterColor
        {
            get { return _chaptercolor; }
            set
            {
                _chaptercolor = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ChapterColor"));
            }
        }


		private UIColor _temporalcolor;

		public UIColor TemporalColor
        {
            get
            {
                return _temporalcolor;
            }
            set
            {
                _temporalcolor = value;
                foreach (var item in _sections)
                    item.TemporalColor = _temporalcolor;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("TemporalColor"));
            }
        }


		private UIImage _backgroundimage;

		public UIImage BackgroundImage
        {
            get { return _backgroundimage; }
            set
            {
                _backgroundimage = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("BackgroundImage"));
            }
        }


    }
}
