using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace YourSPBall.Models
{
    public class UserSettings : BaseModel
    {
        [PrimaryKey,AutoIncrement]
        public int ID { get; set; }

        private bool _MuteSound = false;
        public bool MuteSound
        {
            get { return _MuteSound; }
            set { _MuteSound = value; OnPropertyChanged(nameof(MuteSound)); }
        }

        private string _SelectedLanguageCode = "en";
        public string SelectedLanguageCode
        {
            get { return _SelectedLanguageCode; }
            set { _SelectedLanguageCode = value; OnPropertyChanged(nameof(_SelectedLanguageCode)); }
        }

        [Ignore]
        public string MuteImageName
        {
            get
            {
                if(!MuteSound)
                    return "btn_sound.png";

                return "btn_unmute.png";
            }
        }
    }
}
