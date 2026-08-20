using System.ComponentModel.DataAnnotations;

namespace StudentRegistry.Models;

public class Student
{
    [Key]
    [Display(Name = "Id")]
    public int Id { get; set; }


    [Required(ErrorMessage = "Student_NameRequired")]
    [StringLength(80, ErrorMessage = "Student_NameMaxLength")]
    [MinLength(5, ErrorMessage = "Student_NameMinLength")]
    [Display(Name = "Student_Name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Student_EmailRequired")]
    [EmailAddress(ErrorMessage = "Student_EmailInvalid")]
    [Display(Name = "Student_Email")]
    public string Email { get; set; } = string.Empty;

    public List<Premium> Premiums { get; set; } = new();
}

