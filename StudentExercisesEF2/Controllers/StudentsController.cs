using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentExercisesEF.Models;
using StudentExercisesEF.Data;
using StudentExercisesEF.Models.ViewModels;

namespace StudentExercisesEF.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string searchQuery)
        {
            List<Student> students = await _context.Student
              .Include(s => s.Cohort)
              .OrderBy(s => s.LastName)
              .ToListAsync();

            if (searchQuery != null)
            {
                string normalizedQuery = searchQuery.ToUpper();

                students = students
                    .Where(s =>
                    s.FirstName.ToUpper()
                    .Contains(normalizedQuery) ||
                    s.LastName.ToUpper()
                    .Contains(normalizedQuery))
                    .ToList();

            }


            return View(students);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Cohort)
                .Include(s => s.StudentExercises)
                    .ThenInclude(se => se.Exercise)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            ViewData["CohortId"] = new SelectList(_context.Cohort, "Id", "Name");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,FirstName,LastName,SlackHandle,CohortId")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CohortId"] = new SelectList(_context.Cohort, "Id", "Name", student.CohortId);
            return View(student);
        }

        private async Task<StudentEditViewModel> buildStudentEditViewModel(int? id)
        {
            var student = await _context.Student
                .Include(s => s.Cohort)
                .Include(s => s.StudentExercises)
                .ThenInclude(e => e.Exercise)
                .FirstOrDefaultAsync(s => s.Id == id);

            StudentEditViewModel viewModel = new StudentEditViewModel(_context, student);

            return viewModel;

        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            StudentEditViewModel viewModel = await buildStudentEditViewModel(id);
            if (viewModel.student == null)
            {
                return NotFound();
            }


            return View(viewModel);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudentEditViewModel viewModel)
        {


            if (ModelState.IsValid)
            {
                try
                {


                    // Get all the exercises that WERE assigned to the student before we edited
                    List<StudentExercise> previouslyAssignedExercises = await _context.StudentExercise.Include(se => se.Exercise).Where(se => se.StudentId == id).ToListAsync();


                    // Loop through the exercises that we just assigned 
                    viewModel.SelectedExercises.ForEach(exerciseId =>
                    {
                        // Was the exercise already assigned? 
                        // If so, do nothing-- we want to leave it alone so we can hold onto its completion status
                        // If not
                        if (!previouslyAssignedExercises.Any(studentExercise => studentExercise.ExerciseId == exerciseId))
                        {
                            StudentExercise newAssignment = new StudentExercise()
                            {
                                StudentId = id,
                                ExerciseId = exerciseId,
                                isComplete = false
                            };

                            // Add the newly assigned exercise to the student's list of assigned exercises
                            _context.StudentExercise.Add(newAssignment);

                        }
                    });

                    // Loop through previously assigned exercises and check if they're still assigned. If not, delete them from the student's list of assigned exercises.
                    previouslyAssignedExercises.ForEach(studentExercise =>
                    {
                        if (!viewModel.SelectedExercises.Any(exerciseId => exerciseId == studentExercise.ExerciseId))
                        {

                            // remove from student exercises list
                            _context.StudentExercise.Remove(studentExercise);

                        }

                    });

                    // Update the information about the student ( this includes cohortId property)
                    _context.Update(viewModel.student);

                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    if (!StudentExists(viewModel.student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // If the model state isn't valid, send us back the form with the same information
            StudentEditViewModel newViewModel = await buildStudentEditViewModel(id);

            return View(newViewModel);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Cohort)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Student.FindAsync(id);
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.Id == id);
        }
    }
}
