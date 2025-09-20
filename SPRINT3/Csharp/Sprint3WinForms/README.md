
# Sprint 3 — Windows Forms (Projeto Único)

Atende aos requisitos:
- **Estruturação de classes e código limpo** (entidade `Book`, `AppDbContext`, `FileService`, `MainForm` desacoplados)
- **Manipulação de arquivos**: Importa **JSON**, exporta **JSON/TXT**, mantém **audit.log**
- **Conexão com banco (CRUD completo)**: SQLite + EF Core
- **Interface (Forms)**: CRUD visual com `DataGridView` e formulário
- **Documentação**: este README + XML docs gerados
- **Arquitetura em diagramas**: `docs/architecture.md` (Mermaid)

## Como rodar
1. Visual Studio com workload **Desktop .NET** e **.NET 8 SDK**.
2. Abra `Sprint3WinForms.csproj`.
3. **F5**. O banco `app.db` será criado.

## Endereços úteis
- Logs: `data/audit.log`
- Banco: `app.db` (na pasta do executável)
