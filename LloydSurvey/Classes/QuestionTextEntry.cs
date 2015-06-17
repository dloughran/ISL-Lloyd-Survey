using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LloydSurvey.Classes
{
    /// <summary>
    /// A text entry question type. Extends Question.
    /// Can contain one or more text entry fields.
    /// By default a text entry control will render as a
    /// textbox. A OptionText item can be rendered as a 
    /// textarea by setting its RenderAsTextArea option to true.
    /// </summary>
    public class QuestionTextEntry : Question
    {
        /// <summary>
        /// List of text entry options for the question.
        /// </summary>
        public List<OptionText> Items { get; set; }

        public QuestionTextEntry() : base()
        {
            this.Items = new List<OptionText>();
        }
    }
}