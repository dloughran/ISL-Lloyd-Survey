using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LloydSurvey.Classes
{
    /// <summary>
    /// A survey Question base class.
    /// Other question types extend this base.
    /// </summary>
    public class Question
    {
        /// <summary>
        /// Id (ordinal) of this question in the survey
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// The text of the question (rendered on-page above the question options)
        /// </summary>
        public String QuestionText { get; set; }

        /// <summary>
        /// The number of text entry fields to render for the question.
        /// (If the question type is text entry).
        /// </summary>
        public int NumberOfEntries { get; set; }


    }
}