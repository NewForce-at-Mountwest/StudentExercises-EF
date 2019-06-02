using Microsoft.AspNetCore.Mvc.Rendering;
using StudentExercisesEF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesEF.Models.ViewModels
{
    public class StudentEditViewModel
    {
        public Student Student { get; set; }
        public List<SelectListItem> CohortOptions { get; set; }
    }
}
