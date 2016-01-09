﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Domojee.Models
{
    [DataContract]
    public class Message
    {
        [DataMember]
        public string id;

        [DataMember]
        public string date;

        [DataMember]
        public string logicalId;

        [DataMember]
        public string plugin;

        [DataMember]
        public string message;

        [DataMember]
        public string action;

        public string Text
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
            }
        }
    }
}
