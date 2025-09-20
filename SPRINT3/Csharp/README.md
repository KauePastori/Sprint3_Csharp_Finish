
# C# Sprint 3 – Web CRUD + Files + SQLite

Projeto ASP.NET Core (NET 8) com **arquitetura por camadas** (Domain, Application, Infrastructure, Web) cobrindo:
- **Código limpo e classes bem estruturadas**
- **CRUD completo** (SQLite via EF Core) para `Book`
- **Manipulação de arquivos** JSON/TXT (import/export) e **audit log** `.txt`
- **Interface Web** com **Swagger UI**
- **Documentação** (este README + comentários XML)
- **Arquitetura em diagramas** (Mermaid em `docs/architecture.md`)

## Como rodar

Pré-requisitos: [.NET 8 SDK](https://dotnet.microsoft.com/), SQLite (opcional).

```bash
cd src/Web
dotnet restore
dotnet ef database update   # opcional, o app executa Migrate() automaticamente
dotnet run
```

Abra o navegador em: **https://localhost:7267/swagger**

### Endpoints principais

- `GET    /api/books` – lista
- `GET    /api/books/{id}` – detalhe
- `POST   /api/books` – cria
- `PUT    /api/books/{id}` – atualiza
- `DELETE /api/books/{id}` – remove
- `POST   /api/books/import/json` – envia um arquivo `.json` (form-data) no formato do `data/sample_books.json`
- `GET    /api/books/export/json` – baixa JSON
- `GET    /api/books/export/txt` – baixa TXT (tab-separated)

## Estrutura
```
src/
  Domain/            # Entidades + contratos
  Application/       # Casos de uso/serviços
  Infrastructure/    # EF Core, Repositórios, FileService
  Web/               # ASP.NET Core + Swagger + Controllers
docs/
data/
```

## Boas práticas adotadas
- Validações encapsuladas em `Book`
- Assinaturas assíncronas
- DI/IoC, separação de responsabilidades
- Comentários XML para gerar documentação do assembly
- Migrations automáticas (conveniência de dev)

## Diagrama rápido (C4 Container)

Veja `docs/architecture.md` para diagramas (C4 + classe).

## Licença
MIT
