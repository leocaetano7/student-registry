# Registro de Estudantes 🚀

Sistema de gerenciamento de estudantes e planos Premium desenvolvido em ASP.NET Core Razor Pages, containerizado e com banco de dados PostgreSQL.

## 🛠️ Tecnologias Utilizadas
- .NET 10 / ASP.NET Core Razor Pages
- Entity Framework Core
- PostgreSQL (banco de dados, rodando em container)
- Docker / Docker Compose
- Serilog (logs estruturados) + Health Checks
- OpenTelemetry / .NET Aspire (observabilidade)
- GitHub Actions (CI/CD, publicação de imagem no GHCR)
- Bootstrap (Estilização Cyberpunk/Dark personalizada)

## 🐳 Infraestrutura e DevOps

O projeto evoluiu de um CRUD simples para uma aplicação containerizada, com banco real, CI/CD e observabilidade:

- **Docker**: `Dockerfile` multi-stage (build + runtime enxuto) e `docker-compose.yml` orquestrando app + banco
- **CI/CD**: GitHub Actions builda, testa e publica a imagem no GitHub Container Registry (ghcr.io) a cada push
- **Observabilidade**: logs estruturados com Serilog, endpoint `/health`, e traces/métricas via OpenTelemetry + .NET Aspire
- **Deploy**: aplicação em produção no [Render](https://student-registry-wp6s.onrender.com), com deploy automático a cada push

## 🏃‍♂️ Como Rodar o Projeto Localmente

### Opção 1 — Docker Compose (recomendado, sem precisar instalar .NET SDK)
Pré-requisito: Docker Desktop instalado.
```bash
git clone https://github.com/leocaetano7/student-registry
cd student-registry
docker compose up
```
Isso sobe a aplicação e o banco PostgreSQL juntos. Acesse `http://localhost:8080`.

### Opção 2 — .NET Aspire (com dashboard de observabilidade)
Pré-requisito: SDK do .NET 10 e Docker Desktop (para o banco).
```bash
git clone https://github.com/leocaetano7/student-registry
cd student-registry
dotnet run --project StudentRegistry.AppHost
```
Isso sobe a aplicação e um dashboard local com logs, traces e métricas em tempo real.

### Opção 3 — Manual (sem Docker)
1. Certifique-se de ter o **SDK do .NET 10** e um **PostgreSQL** disponível.
2. Clone o repositório e acesse a pasta do projeto.
3. Instale a ferramenta `dotnet-ef` (só precisa ser feito uma vez por máquina):
```bash
   dotnet tool install --global dotnet-ef
```
4. Ajuste a connection string em `appsettings.json` para apontar ao seu PostgreSQL.
5. Atualize o banco de dados:
```bash
   dotnet ef database update
```
6. Execute a aplicação:
```bash
   dotnet run
```
