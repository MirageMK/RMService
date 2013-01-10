using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RMService
{
    [DataContract]
    public class Item
    {
        [DataMember]
        public int ID;
        [DataMember]
        public Group group;
        [DataMember]
        public string title;
        [DataMember]
        public string subtitle;
        [DataMember]
        public string description;
        [DataMember]
        public string content;
        [DataMember]
        public string backgroundImage;
    }
}