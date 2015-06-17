using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LloydSurvey.Classes
{
    /// <summary>
    /// Collection of questions
    /// </summary>
    public class QuestionSet
    {
        /// <summary>
        /// A list of Text Entry questions that are to be rendered together.
        /// </summary>
        public List<QuestionTextEntry> Items { get; set; }

        public QuestionSet()
        {
            this.Items = new List<QuestionTextEntry>();
        }
    }
}