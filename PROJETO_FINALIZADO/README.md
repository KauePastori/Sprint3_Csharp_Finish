# StudentsApp (WinForms + SQLite + JSON/TXT)

Entrega da **Sprint 3 – C#** com os itens do enunciado:

- **Estruturação de classes e código limpo (25%)**  
  Arquitetura em camadas: `Domain`, `Infrastructure`, `Services` e `UI`. Validação centralizada em `Student.Validate()`.  
- **Manipulação de arquivos (txt/json) (20%)**  
  `FileService` exporta/importa **JSON** e exporta **TXT**.
- **Conexão com banco de dados – CRUD completo (20%)**  
  `StudentRepository` com **Create/ReadAll/Update/Delete** usando **SQLite** (pacote `Microsoft.Data.Sqlite`).  
- **Interface (Windows Forms) (15%)**  
  `MainForm` com **DataGridView** + **TextBoxes** + botões **Adicionar/Atualizar/Excluir/Limpar/Recarregar/Exportar/Importar**.
- **Documentação (10%)**  
  Este README explica como rodar, a arquitetura, decisões e como avaliar.
- **Arquitetura em diagramas (10%)**  
  Veja `docs/diagram.png`.

## Como rodar

1. **Pré‑requisitos**: Visual Studio 2022+ ou `dotnet 8 SDK` no Windows.  
2. Abra a solução `StudentsApp.sln` e defina o projeto **StudentsApp** como *Startup Project*.  
3. Restaure os pacotes (VS faz automático).  
4. **F5** para executar. O SQLite será criado em `./StudentsApp/bin/.../data/app.db` ao iniciar.

> Primeira execução já cria a tabela `Students`.

## Uso rápido

1. Preencha **Nome**, **RM**, **CPF** e **Email**. Clique **Adicionar**.  
2. Selecione uma linha na grade para carregar no formulário; altere e clique **Atualizar**.  
3. Para excluir, selecione uma linha e clique **Excluir**.  
4. **Exportar JSON/TXT** e **Importar JSON** estão nos botões da tela.

## Camadas & Responsabilidades

- **Domain**: entidade `Student` + `Validate()`.
- **Infrastructure**: `Database` (criação do SQLite) e `StudentRepository` (CRUD).
- **Services**: `StudentService` (regras de negócio simples) e `FileService` (TXT/JSON).
- **UI**: `MainForm` (WinForms).

## Decisões de Projeto

- **SQLite local**: zero dependência externa e fácil avaliação.  
- **Validação**: feita antes de persistir para evitar dados inválidos.  
- **UI sem Designer**: os controles são criados programaticamente para facilitar a correção sem arquivos `.Designer.cs` grandes.

## Estrutura de Pastas

```
StudentsApp/
  Domain/Student.cs
  Infrastructure/Database.cs, StudentRepository.cs
  Services/StudentService.cs, FileService.cs
  UI/MainForm.cs
  Program.cs
  StudentsApp.csproj
docs/
  diagram.png
```

## Diagrama

Veja `docs/diagram.png` (camadas e fluxos principais).

## Avaliação sugerida (rubrica)

- Classes e código limpo: **✔**
- Arquivos TXT/JSON: **✔**
- CRUD completo com DB: **✔**
- Interface WinForms: **✔**
- Documentação: **✔**
- Diagrama: **✔**
