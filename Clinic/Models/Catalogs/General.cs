using System.ComponentModel.DataAnnotations;

namespace Clinic.Models.Catalogs
{
    /// <summary>
    /// Enum class for common status on db
    /// </summary>
    public enum StatusCore
    {
        [Display(Name = "Activo")]
        Active,
        [Display(Name = "Inactivo")]
        Inactive,
        [Display(Name = "Eliminado")]
        Deleted
    }

    /// <summary>
    /// Enum class for UI catalogs
    /// </summary>
    public enum StatusSimpleCore
    {
        [Display(Name = "Activo")]
        Active,
        [Display(Name = "Inactivo")]
        Inactive,
    }
}
