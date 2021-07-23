using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace YourSPBall.Models
{
    public class SPBall
    {
        [PrimaryKey,AutoIncrement]
        public int ID { get; set; }
        public string ImageURL { get; set; }
        public string Name { get; set; }
        public string SportType { get; set; }
    }
}
