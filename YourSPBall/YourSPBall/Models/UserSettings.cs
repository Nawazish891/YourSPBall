using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace YourSPBall.Models
{
    public class UserSettings
    {
        [PrimaryKey,AutoIncrement]
        public int ID { get; set; }
        public bool MuteSound { get; set; } = false;
        public string SelectedLanguageCode { get; set; } = "en";

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
