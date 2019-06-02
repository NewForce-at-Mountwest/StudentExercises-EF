using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentExercisesEF.Data;
using StudentExercisesEF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesEF.Models.ViewModels
{
    public class StudentEditViewModel
    {
        public Student student { get; set; }
        public List<SelectListItem> CohortOptions { get; set; }

        public List<SelectListItem> Exercises { get; set; }

        public List<StudentExercise> AssignedExercises { get; set; }

        public List<int> SelectedExercises { get; set; }

        private readonly ApplicationDbContext _context;
        public StudentEditViewModel()
        {
            // Empty constructor
        }

        public StudentEditViewModel(ApplicationDbContext context, Student studentParam)
        {
            _context = context;

            student = studentParam;

            // Figure out how to make this async
            var cohorts =  _context.Cohort.ToList();

            CohortOptions = cohorts.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
                Selected = c.Id == student.CohortId
            }).ToList();

            AssignedExercises = _context.StudentExercise.Include(se => se.Exercise).Where(se => se.StudentId == studentParam.Id).ToList();

            var allExercises = _context.Exercise.ToList();

            Exercises = allExercises.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Name,
                Selected=AssignedExercises.Any(se => se.ExerciseId == e.Id)
            }).ToList();




        }
    }
}
