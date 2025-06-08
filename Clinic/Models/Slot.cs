using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic.Models
{
    public class DoctorSlot
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid DoctorId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; } 

        public bool IsBooked { get; set; } = false;

        // Navigation properties
        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; } = null!;
    }
}
