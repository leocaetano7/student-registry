using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Localization;
using StudentRegistry;

namespace StudentRegistry.Models
{
    public class Premium : IValidatableObject
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Premium_TitleRequired")]
        [StringLength(80, ErrorMessage = "Premium_TitleMaxLength")]
        [Display(Name = "Premium_Title")]
        public string Title { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Premium_StartDate")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Premium_EndDate")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Premium_StudentRequired")]
        [Display(Name = "Premium_Student")]
        public int StudentId { get; set; }

        public Student? Student { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndDate < StartDate)
            {
                var localizer = validationContext.GetService(typeof(IStringLocalizer<SharedResource>))
                    as IStringLocalizer<SharedResource>;
                var message = localizer?["Premium_EndDateBeforeStart"].Value
                              ?? "A data de término não pode ser anterior à data de início.";

                yield return new ValidationResult(message, new[] { nameof(EndDate) });
            }
        }
    }
}
