using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LloydSurvey.Classes
{
    /// <summary>
    /// A checkbox question option. Extends Option.
    /// This type of option is rendered as a checkbox with a label.
    /// </summary>
    public class OptionCheckBox : Option
    {
        /// <summary>
        /// The option label.
        /// </summary>
        public string Label { get; set; }
 
        /// <summary>
        /// True or False, depending upon whether or not the option has been selected.
        /// </summary>
        public bool Selected { get; set; }
    }
}