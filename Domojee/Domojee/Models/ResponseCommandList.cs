﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Domojee.Models
{
    [DataContract]
    class ResponseCommandList
    {
        [DataMember]
        public string id;

        [DataMember]
        public ObservableCollection<Command> result;

        [DataMember]
        public string jsonrpc;

        [DataMember]
        public Error error;
    }
}
