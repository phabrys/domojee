﻿using System.Runtime.Serialization;

namespace Jeedom.Model
{
    [DataContract]
    internal class Parameters
    {
        [DataMember]
        public string apikey;

        [DataMember(IsRequired = false)]
        public string eqLogic_id;

        [DataMember]
        public string id;

        [DataMember]
        public string name;

        [DataMember(IsRequired = false)]
        public string state;

        [DataMember(IsRequired = false)]
        public string object_id;
    }
}