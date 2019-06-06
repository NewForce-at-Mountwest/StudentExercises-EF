using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesEF.Models.ViewModels.ReportItems
{
    public class ExerciseReportItem 
    {
        [Display(Name="Exercise")]
        public Exercise exercise { get; set; }

        [Display(Name ="Number of Students")]
        public int numberOfStudents { get; set; }

    }
}
