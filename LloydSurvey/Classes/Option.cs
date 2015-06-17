using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LloydSurvey.Classes
{
    /// <summary>
    /// A question Option base class. Other option types extend this class.
    /// An option is a checkbox, textbox or textarea that represents an allowed response
    /// in a question. 
    /// For example, a fill-in-the-blank question would have a single textbox or
    /// textarea option. A question where the user could select one or more responses from a
    /// list of possible responses would have multiple checkbox options.
    /// </summary>
    public class Option
    {
        /// <summary>
        /// The Id of this question option. Ordinal within this question.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The ID of the question that this option belongs to
        /// </summary>
        public int QuestionId { get; set; }

        /// <summary>
        /// The type of option. Aids in choosing the input control type when rendering.
        /// Possible Values: CheckBox, CBWithTextBox, TextBox, TextArea
        /// </summary>
        public string Type { get; set; }
    }
}