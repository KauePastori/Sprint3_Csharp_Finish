
# Diagramas de Arquitetura

## Visão (C4 Container)
```mermaid
flowchart LR
    user([Usuário]) --> ui[WinForms MainForm]
    ui --> svc[FileService]
    ui --> dbctx[AppDbContext (EF Core)]
    dbctx --> db[(SQLite)]
    svc --> files[[JSON / TXT / audit.log]]
```

## Diagrama de Classes (simplificado)
```mermaid
classDiagram
    class Book {
      int Id
      string Title
      string Author
      decimal Price
      DateTime PublishedOn
    }
    class AppDbContext {
      +DbSet~Book~ Books
    }
    class FileService {
      +ImportJsonAsync(file)
      +ExportJsonAsync(file)
      +ExportTxtAsync(file)
      +AppendAuditAsync(msg)
    }
    class MainForm

    MainForm --> AppDbContext
    MainForm --> FileService
    AppDbContext --> Book
```
