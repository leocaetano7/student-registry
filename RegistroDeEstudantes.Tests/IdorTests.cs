using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StudentRegistry.Data;
using StudentRegistry.Models;
using Xunit;

public class IdorTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public IdorTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true
        });
    }

    private static string ExtractAntiForgeryToken(string html)
    {
        var match = Regex.Match(
            html,
            @"name=""__RequestVerificationToken""[^>]*value=""([^""]+)""");

        Assert.True(match.Success, "Token antiforgery não encontrado na página — o HTML do form pode ter mudado.");
        return match.Groups[1].Value;
    }

    private async Task RegistrarELogarUsuarioDeTesteAsync()
    {
        var getResponse = await _client.GetAsync("/Identity/Account/Register");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var html = await getResponse.Content.ReadAsStringAsync();
        var token = ExtractAntiForgeryToken(html);

        var email = $"idor-{Guid.NewGuid():N}@teste.com";
        var senha = "SenhaForte!123";

        var form = new Dictionary<string, string>
        {
            ["__RequestVerificationToken"] = token,
            ["Input.Email"] = email,
            ["Input.Password"] = senha,
            ["Input.ConfirmPassword"] = senha
        };

        var postResponse = await _client.PostAsync(
            "/Identity/Account/Register",
            new FormUrlEncodedContent(form));

        Assert.True(
            postResponse.IsSuccessStatusCode,
            $"Registro do usuário de teste falhou com status {postResponse.StatusCode}.");
    }

    private (int idRota, int idForjado) SeedDoisStudents()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var estudanteDaRota = new Student { Name = "Estudante Rota", Email = $"rota-{Guid.NewGuid():N}@teste.com" };
        var estudanteForjado = new Student { Name = "Estudante Forjado", Email = $"forjado-{Guid.NewGuid():N}@teste.com" };

        context.Students.AddRange(estudanteDaRota, estudanteForjado);
        context.SaveChanges();

        return (estudanteDaRota.Id, estudanteForjado.Id);
    }

    [Fact]
    public async Task Edit_ComIdForjadoNoBody_AtualizaApenasOStudentDaRota()
    {
        await RegistrarELogarUsuarioDeTesteAsync();
        var (idRota, idForjado) = SeedDoisStudents();

        var editGet = await _client.GetAsync($"/Students/Edit/{idRota}");
        Assert.Equal(HttpStatusCode.OK, editGet.StatusCode);

        var editHtml = await editGet.Content.ReadAsStringAsync();
        var editToken = ExtractAntiForgeryToken(editHtml);

        var formForjado = new Dictionary<string, string>
        {
            ["__RequestVerificationToken"] = editToken,
            ["Student.Id"] = idForjado.ToString(),
            ["Student.Name"] = "Nome Alterado Via Ataque",
            ["Student.Email"] = $"hackeado-{Guid.NewGuid():N}@teste.com"
        };

        var editPost = await _client.PostAsync(
            $"/Students/Edit/{idRota}",
            new FormUrlEncodedContent(formForjado));

        Assert.True(
            editPost.IsSuccessStatusCode,
            $"POST de edição falhou com status {editPost.StatusCode} — confira se o login/antiforgery deu certo.");

        using var verifyScope = _factory.Services.CreateScope();
        var verifyContext = verifyScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var estudanteDaRotaAtualizado = await verifyContext.Students.FindAsync(idRota);
        var estudanteForjadoIntacto = await verifyContext.Students.FindAsync(idForjado);

        Assert.Equal("Nome Alterado Via Ataque", estudanteDaRotaAtualizado!.Name);
        Assert.Equal("Estudante Forjado", estudanteForjadoIntacto!.Name);
    }
}