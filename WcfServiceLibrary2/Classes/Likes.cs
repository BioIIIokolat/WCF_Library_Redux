using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceLibrary2.Classes
{
    public class Likes
    {
        [Key]
        public int LikeID { get; set; }

        public int User_Liked_ID { get; set; }

        public int User_Who_Liked_ID { get; set; }

        public DateTime Date_Like { get; set; }
    }
}
