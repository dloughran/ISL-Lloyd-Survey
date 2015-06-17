using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LloydSurvey.Classes
{
    /// <summary>
    /// Holds map marker info
    /// </summary>
    public class MapModel
    {
        /// <summary>
        /// List of JSON serialized map marker and comment information.
        /// </summary>
        public List<string> Markers { get; set; }
        
        /// <summary>
        /// View to redirect to based on navigation direction (back or next)
        /// </summary>
        public string RedirectTo { get; set; }
    }
}