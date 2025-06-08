using System.ComponentModel.DataAnnotations;

namespace Clinic.Models
{
    /// <summary>
    /// Represents a patient of the clinic.
    /// Ideally, this class would be containt additional properties for a patient's details,
    /// </summary>
    public class Patient
    {
        [Key]
        public Guid ApplicationUserId { get; set; }
        public string Code { get; set; } = string.Empty; // Unique code for the patient, e.g., "P1234"

        // Navigation property: A patient can have many appointments.
        public virtual ICollection<Appointment> Appointments { get; set; } = [];
    }

}
