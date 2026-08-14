# Student Registry 🚀

Sistema de gerenciamento de estudantes e planos Premium desenvolvido em ASP.NET Core Razor Pages com banco de dados SQLite.

## 🛠️ Tecnologias Utilizadas
- .NET 10 / ASP.NET Core Razor Pages
- Entity Framework Core
- SQLite (Banco de dados local)
- Bootstrap (Estilização Cyberpunk/Dark personalizada)

## 🏃‍♂️ Como Rodar o Projeto Localmente

1. Certifique-se de ter o **SDK do .NET 10** instalado.
2. Clone o repositório:
   ```bash
   git clone https://github.com/leocaetano7/student-registry
   ```
3. Acesse a pasta do projeto:
   ```bash
   cd student-registry
   ```
4. Instale a ferramenta `dotnet-ef` (necessária para aplicar as migrations; só precisa ser feito uma vez por máquina):
   ```bash
   dotnet tool install --global dotnet-ef
   ```
5. Atualize o banco de dados:
   ```bash
   dotnet ef database update
   ```
6. Execute a aplicação:
   ```bash
   dotnet run
   ```
