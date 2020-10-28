﻿using System.Collections.Generic;

namespace authorization_play.Persistance.Models
{
    public class Resource
    {
        public int ResourceId { get; set; }
        public string CanonicalName { get; set; }
        public int ResourceKindId { get; set; }
        public ResourceKind ResourceKind { get; set; }
        public List<ResourceAction> Actions { get; set; }
    }
}
