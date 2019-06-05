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

        public List<SelectListItem> ExerciseOptions { get; set; }

        public List<int> SelectedExercises { get; set; }

        private readonly ApplicationDbContext _context;
        public StudentEditViewModel()
        {
            // Empty constructor, will run if we pass nothing in (i.e. when we POST from the student edit form)
        }

        public StudentEditViewModel(ApplicationDbContext context, Student studentParam)
        {
            _context = context;

            student = studentParam;

            // Get a list of all the cohorts
            // convert to a list of select list items
            // Select the student's current cohort (i.e. the one that matches their cohortId property)
            CohortOptions = _context.Cohort
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = c.Id == student.CohortId
                }).ToList();


            // Get a list of JUST the id's of the exercises, we're going to use this with our asp-for helper in the view
            SelectedExercises = _context.StudentExercise
                .Include(se => se.Exercise) // Get all the student exercises and join in the Exercise table so we can read the exercise name
                .Where(se => se.StudentId == studentParam.Id) // Filter them-- we only want ones assigned to THIS student
                .Select(se => se.ExerciseId) // map over the student exercises and return their exercise Id's 
                .ToList();



            ExerciseOptions = _context.Exercise // Get ALL the exercises and conver them to a list of select list items
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.Name
                }).ToList();


        }


    }
}
