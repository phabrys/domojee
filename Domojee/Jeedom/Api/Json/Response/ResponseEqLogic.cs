﻿using Jeedom.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Jeedom.Api.Json.Response
{
    [DataContract]
    public class ResponseEqLogic
    {
        [DataMember]
        public EqLogic result;
    }
}