using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesEF.Models.ViewModels.ReportItems
{
    public class StudentReportItem
    {
        public Student student { get; set; }
        public int numberOfExercises { get; set; }
    }
}
