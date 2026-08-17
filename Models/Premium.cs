using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Localization;
using RegistroDeEstudantes;

namespace RegistroDeEstudantes.Models
{
    public class Premium : IValidatableObject
    {
        [Key]
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
            var localizer = validationContext.GetService(typeof(IStringLocalizer<SharedResource>))
                as IStringLocalizer<SharedResource>;

            if (StartDate == default)
            {
                var message = localizer?["Premium_StartDateRequired"].Value
                              ?? "Informe a data de início.";

                yield return new ValidationResult(message, new[] { nameof(StartDate) });
            }

            if (EndDate == default)
            {
                var message = localizer?["Premium_EndDateRequired"].Value
                              ?? "Informe a data de término.";

                yield return new ValidationResult(message, new[] { nameof(EndDate) });
            }

            if (StartDate != default && EndDate != default && EndDate < StartDate)
            {
                var message = localizer?["Premium_EndDateBeforeStart"].Value
                              ?? "A data de término não pode ser anterior à data de início.";

                yield return new ValidationResult(message, new[] { nameof(EndDate) });
            }
        }
    }
}