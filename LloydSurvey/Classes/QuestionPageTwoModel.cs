using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LloydSurvey.Classes
{
    public class QuestionPageTwoModel
    {
        /// <summary>
        /// View to redirect to based on navigation direction (back or next)
        /// </summary>
        public string RedirectTo { get; set; }

        /// <summary>
        /// Text of the second question of the survey overall (Q2)
        /// This corresponds to <questionnumber>2</questionnumber> in App_Data/Questions.xml
        /// </summary>
        public string Q2QuestionText { get; set; }
        
        /// <summary>
        /// First text entry option for Q2
        /// </summary>
        [MaxLength(250,ErrorMessage="The maximum length for this entry is 250 characters.")]
        public string Q2TextEntry1 { get; set; }

        /// <summary>
        /// Second text entry option for Q2
        /// </summary>
        [MaxLength(250, ErrorMessage = "The maximum length for this entry is 250 characters.")]
        public string Q2TextEntry2 { get; set; }

        /// <summary>
        /// Third text entry option for Q2
        /// </summary>
        [MaxLength(250)]
        public string Q2TextEntry3 { get; set; }

        /// <summary>
        /// Text of the third question of the survey overall (Q3)
        /// This corresponds to <questionnumber>3</questionnumber> in App_Data/Questions.xml
        /// </summary>
        public string Q3QuestionText { get; set; }
        
        /// <summary>
        /// Text entry option for Q3.
        /// This entry is renered as a textarea
        /// </summary>
        [MaxLength(5000)]
        public string Q3TextEntry1 { get; set; }

        /// <summary>
        /// Text of the fourth question of the survey overall (Q4)
        /// This corresponds to <questionnumber>4</questionnumber> in App_Data/Questions.xml
        /// </summary>
        public string Q4QuestionText { get; set; }
        
        [StringLength(6)]
        [RegularExpression(@"[AaBbCcEeGgHhJjKkLlMmNnPpRrSsTtVvXxYy][0-9][AaBbCcEeGgHhJjKkLlMmNnPpRrSsTtVvWwXxYyZz][0-9][AaBbCcEeGgHhJjKkLlMmNnPpRrSsTtVvWwXxYyZz][0-9]",ErrorMessage="Please enter a valid postal code.")]
        public string Q4TextEntry1 { get; set; }
    }
}