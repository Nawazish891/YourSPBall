using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace YourSPBall.Models
{
    public class SPBall : BaseModel
    {
        [PrimaryKey,AutoIncrement]
        public int ID { get; set; }

        private string _ImageURL;
        public string ImageURL
        {
            get { return _ImageURL; }
            set { _ImageURL = value; OnPropertyChanged(nameof(ImageURL)); }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; OnPropertyChanged(nameof(Name)); }
        }

        private string _SportType;

        public string SportType
        {
            get { return _SportType; }
            set { _SportType = value; OnPropertyChanged(nameof(SportType)); }
        }

        
    }
}
