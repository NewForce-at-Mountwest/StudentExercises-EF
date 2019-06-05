using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentExercisesEF.Data;
using StudentExercisesEF.Models;
using StudentExercisesEF.Models.ViewModels;
using StudentExercisesEF.Models.ViewModels.ReportItems;

namespace StudentExercisesEF.Controllers
{
    public class CohortsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CohortsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cohorts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cohort.ToListAsync());
        }

        // GET: Cohorts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cohort = await _context.Cohort
                .Include(c => c.Students)
                .Include(c => c.Instructors)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cohort == null)
            {
                return NotFound();
            }

            return View(cohort);
        }

        // GET: Cohorts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cohorts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Cohort cohort)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cohort);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cohort);
        }

        // GET: Cohorts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cohort = await _context.Cohort.FindAsync(id);
            if (cohort == null)
            {
                return NotFound();
            }
            return View(cohort);
        }

        // POST: Cohorts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Cohort cohort)
        {
            if (id != cohort.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cohort);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CohortExists(cohort.Id))
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
            return View(cohort);
        }

        // GET: Cohorts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cohort = await _context.Cohort
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cohort == null)
            {
                return NotFound();
            }

            return View(cohort);
        }

        // POST: Cohorts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cohort = await _context.Cohort.FindAsync(id);
            _context.Cohort.Remove(cohort);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Report(int? currentCohortId)
        {

            CohortReportViewModel vm = new CohortReportViewModel(_context);

            // If we do a GET request with the cohort Id, 
            if (currentCohortId != null)
            {
                // All student exercises for this cohort
                List<StudentExercise> studentExercises = await _context.StudentExercise
                    .Include(se => se.Exercise)
                    .Include(se => se.Student)
                    .Where(se => se.Student.CohortId == currentCohortId)
                    .ToListAsync();

                // Incomplete student exercises
                List<StudentExercise> incompleteExercises = studentExercises.Where(se => !se.isComplete).ToList();
                List<StudentExercise> completeExercies = studentExercises.Where(se => se.isComplete).ToList();


                // Top three incomplete exercsies
                // Need to attach to view model, change type in view model to report item
                vm.mostWorkedOnExercises = (from se in incompleteExercises
                                                          group se by se.ExerciseId into gr
                                                          orderby gr.Count() descending
                                                          select new ExerciseReportItem()
                                                          {
                                                              exercise = gr.ToList()[0].Exercise,
                                                              numberOfStudents = gr.ToList().Count()
                                                          }).Take(3).ToList();

                // Which students have the most completed exercise?
                vm.topThreeBusiestStudents = (from se in completeExercies
                                       group se by se.StudentId into gr
                                       orderby gr.Count() descending
                                       select new StudentReportItem()
                                       {
                                           student = gr.ToList()[0].Student,
                                           numberOfExercises = gr.ToList().Count()
                                       }).Take(3).ToList();

                // Who has the most assigned but incompleted exercises 
                vm.topThreeLaziestStudents = (from se in incompleteExercises
                                       group se by se.StudentId into gr
                                       orderby gr.Count() descending
                                       select new StudentReportItem()
                                       {
                                           student = gr.ToList()[0].Student,
                                           numberOfExercises = gr.ToList().Count()
                                       }).Take(3).ToList();
            }
            return View(vm);
        }

        private bool CohortExists(int id)
        {
            return _context.Cohort.Any(e => e.Id == id);
        }
    }

  
}
