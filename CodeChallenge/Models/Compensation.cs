using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeChallenge.Models
{
    public class Compensation
    {
        [Key]
        public int CompensationId { get; set; }

        [Required]
        public decimal Salary { get; set; }

        [Required]
        public DateTime EffectiveDate { get; set; }

        [ForeignKey("Employee")]
        public string EmployeeId { get; set; } // Foreign key to Employee

        public Employee Employee { get; set; } // Navigation property
    }
}
