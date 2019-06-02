using System.ComponentModel.DataAnnotations;

namespace StudentExercisesEF.Models
{
    public class StudentExercise
    {
        public int Id { get; set; }
        [Required]
        public int StudentId { get; set; }
        [Required]
        public int ExerciseId { get; set; }
        public Student Student { get; set; }
        public Exercise Exercise { get; set; }

        public bool isComplete { get; set; }
    }
}