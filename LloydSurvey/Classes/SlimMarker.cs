using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LloydSurvey.Classes
{
    [Serializable]
    public class SlimMarker
    {
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Type { get; set; }
        public string Icon { get; set; }
        public string Id { get; set; }
        public string Uuid { get; set; }
        public string Comment { get; set; }
    }
}