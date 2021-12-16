
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace NeuronalNetwork
{   [DataContract]
    public class Net
    
    {   [DataMember]
        public List<Connection> Connections = new List<Connection>();
        [DataMember]
        public List<Node> Nodes = new List<Node>();
        public Net(){}
        
}}
