using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LloydSurvey.Classes
{
    /// <summary>
    /// A text response option. Extends Option.
    /// Can be redered as either a textbox or textarea
    /// </summary>
    public class OptionText
    {
        /// <summary>
        /// Label for the field. Example: "Enter your name."
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Maximum number of characters to allow for entry.
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// The value of the text entry
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// If true, the option will render as a TextArea control.
        /// Default is false, which renderes the option as a TextBox control.
        /// </summary>
        public bool RenderAsTextArea { get; set; }
    }
}