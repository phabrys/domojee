﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Domojee.Models
{
    [DataContract]
    class ResponseScene : Response
    {
        [DataMember]
        public Scene result;
    }
}
