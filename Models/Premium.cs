using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace testeleo.Models
{
    public class Premium : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório")]
        [StringLength(80, ErrorMessage = "O título deve ter no máximo 80 caracteres")]
        public string Title { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "O estudante é obrigatório")]
        public int StudentId { get; set; }

        public Student? Student { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndDate < StartDate)
            {
                yield return new ValidationResult("A data de término não pode ser menor que a data de início.", new[] { nameof(EndDate) });
            }
        }
    }
}
