
# Diagramas de Arquitetura

## C4 (Container) – Visão Geral
```mermaid
flowchart LR
    user([Usuário]) --> web[ASP.NET Core Web (API)]
    web --> app[Application (Serviços)]
    app --> domain[Domain (Entidades)]
    app --> infra[Infrastructure (EF Core + Repositórios + FileService)]
    infra --> db[(SQLite DB)]
    infra --> files[[Arquivos JSON/TXT + audit.log]]
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
      +Update(title, author, price, publishedOn)
    }

    class IBookRepository {
      +GetByIdAsync(id, ct)
      +GetAllAsync(ct)
      +AddAsync(book, ct)
      +UpdateAsync(book, ct)
      +DeleteAsync(id, ct)
    }

    class BookService {
      -IBookRepository repo
      +CreateAsync(...)
      +GetAllAsync(ct)
      +GetAsync(id, ct)
      +UpdateAsync(id, ...)
      +DeleteAsync(id, ct)
    }

    BookService --> IBookRepository
```

> Os diagramas acima podem ser visualizados diretamente no GitHub/VS Code com preview de Mermaid.
