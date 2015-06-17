using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LloydSurvey.Classes
{
    /// <summary>
    /// The Model for Page One Questions. Extends Question
    /// </summary>
    public class QuestionPageOneModel : Question
    {
        /// <summary>
        /// List of checkbox options
        /// </summary>
        public List<OptionCheckBox> Items { get; set; }

        /// <summary>
        /// The maximum number of selections allowed from the list.
        /// Example: If MaxSelections is 3, the user will be limited
        /// to selecting a maximum of 3 responses out of the total number
        /// in the list.
        /// </summary>
        public int MaxSelections { get; set; }

        /// <summary>
        /// The minimum number of selections allowed from the list.
        /// </summary>
        public int MinSelections { get; set; }
        
        /// <summary>
        /// The value of the Other text box
        /// </summary>
        public string OtherText { get; set; }

        public QuestionPageOneModel() : base()
        {
            this.Items = new List<OptionCheckBox>();
        }
    }
}