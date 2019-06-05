using Microsoft.AspNetCore.Mvc.Rendering;
using StudentExercisesEF.Data;
using StudentExercisesEF.Models.ViewModels.ReportItems;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesEF.Models.ViewModels
{
    public class CohortReportViewModel
    {
       
        [Display(Name = "Choose a cohort:")]
        public List<SelectListItem> allCohorts = new List<SelectListItem>();

        public int currentCohortId { get; set; }

        [Display(Name = "Top Three Assigned but Incomplete Exercises")]
        public List<ExerciseReportItem> mostWorkedOnExercises { get; set; } = new List<ExerciseReportItem>();

        [Display(Name = "Top Three Busiest Students")]
        public List<StudentReportItem> topThreeBusiestStudents { get; set; } = new List<StudentReportItem>();

        [Display(Name = "Top Three Laziest Students")]
        public List<StudentReportItem> topThreeLaziestStudents { get; set; } = new List<StudentReportItem>();

        private ApplicationDbContext _context { get; set; }
        public CohortReportViewModel() { }

        // This will be called the first time we render the form
        public CohortReportViewModel(ApplicationDbContext context)
        {
            _context = context;

            allCohorts = _context.Cohort.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

        }

       


    }
}
