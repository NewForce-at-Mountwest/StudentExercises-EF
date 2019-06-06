using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesEF.Models.ViewModels.ReportItems
{
    public class StudentReportItem
    {
        [Display(Name="Student")]
        public Student student { get; set; }

        [Display(Name ="Number of Exercises")]
        public int numberOfExercises { get; set; }
    }
}
