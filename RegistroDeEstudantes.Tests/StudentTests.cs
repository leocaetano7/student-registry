using System.ComponentModel.DataAnnotations;
using RegistroDeEstudantes.Models;
using Xunit;

namespace RegistroDeEstudantes.Tests;

public class StudentTests
{
    private static IList<ValidationResult> Validate(object instance)
    {
        var context = new ValidationContext(instance);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(instance, context, results, validateAllProperties: true);
        return results;
    }

    [Fact]
    public void Student_WithValidData_PassesValidation()
    {
        var student = new Student { Name = "Maria Silva", Email = "maria@example.com" };

        var results = Validate(student);

        Assert.Empty(results);
    }

    [Fact]
    public void Student_WithEmptyName_FailsValidation()
    {
        var student = new Student { Name = "", Email = "maria@example.com" };

        var results = Validate(student);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(Student.Name)));
    }

    [Fact]
    public void Student_WithNameShorterThanMinimum_FailsValidation()
    {
        var student = new Student { Name = "Zoe", Email = "zoe@example.com" };

        var results = Validate(student);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(Student.Name)));
    }

    [Fact]
    public void Student_WithNameLongerThanMaximum_FailsValidation()
    {
        var student = new Student { Name = new string('A', 81), Email = "maria@example.com" };

        var results = Validate(student);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(Student.Name)));
    }

    [Fact]
    public void Student_WithInvalidEmail_FailsValidation()
    {
        var student = new Student { Name = "Maria Silva", Email = "not-an-email" };

        var results = Validate(student);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(Student.Email)));
    }

    [Fact]
    public void Student_WithEmptyEmail_FailsValidation()
    {
        var student = new Student { Name = "Maria Silva", Email = "" };

        var results = Validate(student);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(Student.Email)));
    }
}

public class PremiumTests
{
    private static IList<ValidationResult> Validate(object instance)
    {
        var context = new ValidationContext(instance);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(instance, context, results, validateAllProperties: true);
        return results;
    }

    [Fact]
    public void Premium_WithValidDatesAndStudent_PassesValidation()
    {
        var premium = new Premium
        {
            Title = "Plano Anual",
            StartDate = new DateTime(2026, 1, 1),
            EndDate = new DateTime(2026, 12, 31),
            StudentId = 1
        };

        var results = Validate(premium);

        Assert.Empty(results);
    }

    [Fact]
    public void Premium_WithEndDateBeforeStartDate_FailsValidation()
    {
        var premium = new Premium
        {
            Title = "Plano Anual",
            StartDate = new DateTime(2026, 6, 1),
            EndDate = new DateTime(2026, 1, 1),
            StudentId = 1
        };

        var results = Validate(premium);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(Premium.EndDate)));
    }

    [Fact]
    public void Premium_WithMissingStartDate_FailsValidation()
    {
        var premium = new Premium
        {
            Title = "Plano Anual",
            StartDate = default,
            EndDate = new DateTime(2026, 12, 31),
            StudentId = 1
        };

        var results = Validate(premium);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(Premium.StartDate)));
    }

    [Fact]
    public void Premium_WithMissingEndDate_FailsValidation()
    {
        var premium = new Premium
        {
            Title = "Plano Anual",
            StartDate = new DateTime(2026, 1, 1),
            EndDate = default,
            StudentId = 1
        };

        var results = Validate(premium);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(Premium.EndDate)));
    }

    [Fact]
    public void Premium_WithEmptyTitle_FailsValidation()
    {
        var premium = new Premium
        {
            Title = "",
            StartDate = new DateTime(2026, 1, 1),
            EndDate = new DateTime(2026, 12, 31),
            StudentId = 1
        };

        var results = Validate(premium);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(Premium.Title)));
    }
}
