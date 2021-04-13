using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceLibrary2.Classes
{
    [DataContract]
    public class Message
    {
        [Key]
        [DataMember]
        public int MessageId { get; set; }

        [DataMember]
        public DateTime TimeSending { get; set; }
        [DataMember]
        public bool IsRecieved { get; set; }
        [DataMember]
        public string Mes { get; set; }
        [DataMember]
        public string ImagePath { get; set; }
        [DataMember]
        public User user { get; set; }
        [DataMember]
        public ChatItem chatItem { get; set; }

    }
    [DataContract]
    public class ChatItem
    {
        [DataMember]
        [Key]
        public int ChatItemId { get; set; }

        [DataMember]
        public List<Message> Messages { get; set; }

        [DataMember]
        public string ImagePath { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public ICollection<ChatItemUsers> Participants { get; set; }

    }
    [DataContract]
    public class ChatItemUsers
    {

        [DataMember]
        [Key, Column(Order = 0)]
        public int ChatItemId { get; set; }

        [DataMember]
        [Key, Column(Order = 1)]
        public int UserId { get; set; }

        [DataMember]
        [ForeignKey("ChatItemId")]
        public virtual ChatItem ChatItem { get; set; }
        [DataMember]
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
