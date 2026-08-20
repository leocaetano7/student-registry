using System;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StudentRegistry.Data;
using StudentRegistry.Models;
using Xunit;

public class EmailUnicoTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly ApplicationDbContext _context;

    public EmailUnicoTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new ApplicationDbContext(options);
        _context.Database.EnsureCreated();
    }

    [Fact]
    public async Task Student_ComEmailDuplicado_LancaExcecaoAoSalvar()
    {
        _context.Students.Add(new Student { Name = "Ana", Email = "ana@teste.com" });
        await _context.SaveChangesAsync();

        _context.Students.Add(new Student { Name = "Ana 2", Email = "ana@teste.com" });

        await Assert.ThrowsAsync<DbUpdateException>(() => _context.SaveChangesAsync());
    }

    public void Dispose()
    {
        _connection.Close();
        _context.Dispose();
    }
}