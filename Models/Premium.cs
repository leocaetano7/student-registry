using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace testeleo.Models;

public class Premium
{
    [Key]
    [DisplayName("Id")]
    public int Id { get; set; } // Corrigido: adicionado o nome da propriedade 'Id' e chaves {}

    [Required(ErrorMessage = "Informe o título do Premium")] // Corrigido: ErrorMessage
    [StringLength(80, ErrorMessage = "O título deve conter até 80 caracteres")] // Corrigido: tamanho 80 e texto
    [MinLength(5, ErrorMessage = "O título deve conter pelo menos 5 caracteres")] // Corrigido: adicionado número 5
    [DisplayName("Título")]
    public string Title { get; set; } = string.Empty; // Corrigido: 'Title'

    [DataType(DataType.DateTime)]
    // [GreaterThanToday] // Nota: Requer uma classe de validação customizada para funcionar
    [DisplayName("Início")]
    public DateTime StartDate { get; set; } // Corrigido: PascalCase 'StartDate'

    [DataType(DataType.DateTime)]
    [DisplayName("Término")] // Corrigido: caracteres cirílicos substituídos
    public DateTime EndDate { get; set; }

    [DisplayName("Aluno")]
    [Required(ErrorMessage = "Aluno Inválido")] // Corrigido: ErrorMessage
    public int StudentId { get; set; } // Corrigido: adicionado '{' e corrigido PascalCase

    public Student? Student { get; set; } // Corrigido: 'public'
}