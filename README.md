SPRINT3 - APOSTADORES (WinForms + SQLite + Import/Export)
=========================================================

Projeto: Sprint3WinForms Framework: .NET 8.0 (Windows)
UI: Windows Forms (WinForms) Banco de dados: SQLite (via EF Core)
Plataforma/IDE: Visual Studio 2022+ ou .NET SDK 8.0 (CLI)

\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\--
SUMÁRIO
\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-- 1.
Visão geral 2. Recursos 3. Pré‑requisitos 4. Como abrir e executar
(Visual Studio e CLI) 5. Primeira execução (criando o banco) 6. Importar
dados (JSON) 7. Como usar o aplicativo (CRUD, filtro, avaliação,
exportações) 8. Estrutura do projeto 9. Esquema da base (EF Core) 10.
Solução de problemas (FAQ) 11. Roadmap (ideias futuras) 12. Licença /
Créditos

1\) VISÃO GERAL
\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\--
Este aplicativo WinForms demonstra um fluxo completo para registrar e
acompanhar apostadores com base em sinais de alerta e perdas
financeiras.

Ele usa: - EF Core + SQLite (arquivo local app.db) para persistência -
UI WinForms com foco em produtividade (grid rápida, filtros, chips de
risco) - Importação e exportação em JSON/TXT/CSV - Log de auditoria
simples (data/audit.log)

Este pacote é a variante SEM gráficos --- ideal para ambientes onde o
assembly DataVisualization não está disponível.

2\) RECURSOS
\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\--
• CRUD de apostadores • Cálculo automático do nível de risco
(Baixo/Médio/Alto) • Recomendações automáticas com base no risco •
Filtro instantâneo por texto + filtro por nível (topbar) • Cartões de
KPI: Total \| Alto \| Médio \| Baixo • Importar JSON (em massa) •
Exportar JSON, TXT ou CSV • Log de auditoria de operações
(import/export/salvar/excluir) • Banco local SQLite criado
automaticamente (sem migrations obrigatórias)

3\) PRÉ‑REQUISITOS
\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\--
Opção A -- Visual Studio 2022+ (recomendado) - Workload ".NET Desktop
Development" - .NET 8.0 SDK

Opção B -- Linha de comando (CLI) - .NET 8.0 SDK
(https://dotnet.microsoft.com/)

Sistema Operacional - Windows 10/11

4\) COMO ABRIR E EXECUTAR
\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\--
A) Visual Studio  1. Extraia o .zip. 2. Abra a pasta "Sprint3WinForms"
como solução/projeto (File → Open → Folder). 3. Restaure os pacotes
automaticamente (VS faz isso ao abrir). 4. Compile (Build → Build
Solution). 5. Execute (F5).

B\) Linha de comando (CLI)  1. Abra um terminal na pasta do projeto: cd
Sprint3WinForms 2. Restaure pacotes: dotnet restore 3. Execute: dotnet
run

Observações - Na primeira execução, o arquivo "app.db" será criado
automaticamente no diretório de saída (bin\\Debug\\net8.0-windows\\ ou
bin\\Release\\net8.0-windows\\).

5\) PRIMEIRA EXECUÇÃO
\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\--
Ao abrir o app pela primeira vez: - A base SQLite (app.db) é criada se
ainda não existir. - A tela inicial apresenta a grid vazia e a topbar
com busca e filtro. - Use "Arquivo → Importar JSON" para carregar uma
amostra rapidamente.

Arquivo exemplo incluído no pacote: - data\\sample_apostadores.json -
(Se você solicitou o arquivo maior: apostadores_100.json)

6\) IMPORTAR DADOS (JSON)
\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\--
Menu: Arquivo → Importar JSON

Formato esperado (lista de objetos): \[ { \"Id\": 0, \"Nome\": \"Ana
Souza\", \"Idade\": 29, \"FrequenciaSemanal\": 5,
\"TempoMedioSessaoMin\": 130, \"PerdasUltimoMes\": 1200,
\"SinaisAlerta\": \"Joga de madrugada; irritação após perdas\",
\"NivelRisco\": \"\", \"Recomendacao\": \"\", \"DataUltimaAvaliacao\":
\"2025-09-21\" }, \... \]

Notas: - O campo Id é ignorado no import (zerado para forçar
inserção). - "NivelRisco" e "Recomendacao" são recalculados no import. -
Datas devem estar em "yyyy-MM-dd" (ou ISO 8601 compatível).

7\) COMO USAR O APLICATIVO
\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\--
Topbar (logo acima da grid) - Buscar: filtra por Nome, Sinais de Alerta
ou Recomendação. - Nível: filtra por Todos \| Alto \| Médio \| Baixo. -
Cards de KPI: mostram contagens atuais do resultado filtrado.

Grid principal - Exibe os campos principais, com destaque visual (chip)
para o nível. - Selecionar uma linha preenche os campos do formulário
(abaixo da grid).

Formulário (abaixo da grid) - Id (read-only), Nome, Sinais de Alerta,
Perda (mês), Publicado em. - Botões: • Novo: limpa o formulário para
inserir um novo registro. • Salvar: insere ou atualiza (conforme Id=0 ou
Id\>0). Ao salvar, o risco e a recomendação são recalculados. • Excluir:
remove o selecionado (confirmação antes). • Atualizar: recarrega a grid
a partir do banco.

Exportações - Arquivo → Exportar JSON - Arquivo → Exportar TXT -
(Opcional na classe FileService: Exportar CSV) - Após exportar,
verifique também o log em data/audit.log

Cálculo de risco (lógica atual) - +2 pontos se FrequenciaSemanal ≥ 4 -
+2 pontos se TempoMedioSessaoMin ≥ 120 - +2 pontos se PerdasUltimoMes ≥
1000 - +2 pontos se SinaisAlerta não estiver vazio - Regra final: Pontos
≥ 6 → Alto \| Pontos ≥ 3 → Médio \| Caso contrário → Baixo

Recomendação (de acordo com o risco) - Alto: Encaminhar para apoio
especializado; limites rígidos; contato semanal. - Médio: Definir
limites; alerta financeiro; revisão quinzenal. - Baixo: Educação
financeira básica; revisão mensal.

8\) ESTRUTURA DO PROJETO
\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\--
Sprint3WinForms/ ├─ data/ │ ├─ sample_apostadores.json (amostra de
importação) │ └─ audit.log (gerado em tempo de execução) ├─ Apostador.cs
(modelo de domínio) ├─ AppDbContext.cs (EF Core + configuração SQLite)
├─ FileService.cs (import/export + auditoria) ├─ FancyUi.cs (topbar de
busca/filtro + KPIs + chip de risco) ├─ MainForm.cs (lógica do
formulário) ├─ MainForm.Designer.cs (layout do formulário) ├─ Program.cs
(bootstrap WinForms) └─ Sprint3WinForms.csproj (projeto .NET 8;
WinForms)

9\) ESQUEMA DA BASE (EF CORE)
\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\--
Tabela: Apostadores - Id (INTEGER, PK) - Nome (TEXT, Required, 120) -
Idade (INTEGER) - FrequenciaSemanal (INTEGER) - TempoMedioSessaoMin
(INTEGER) - PerdasUltimoMes (INTEGER) - SinaisAlerta (TEXT, 1000) -
NivelRisco (TEXT, 20) - Recomendacao (TEXT, 500) - DataUltimaAvaliacao
(TEXT -- ISO Date)

Observação: O EF Core cria o schema na primeira execução via
EnsureCreated().

10\) SOLUÇÃO DE PROBLEMAS (FAQ)
\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\--
• "O projeto não compila / falta System.Data.SQLite?" → Não usamos
System.Data.SQLite direto; a dependência é
"Microsoft.EntityFrameworkCore.Sqlite" e vem especificada no .csproj.
Execute "dotnet restore" ou deixe o VS restaurar.

• "O app roda, mas a grid está vazia." → Importe o JSON (Arquivo →
Importar JSON) ou cadastre manualmente usando "Novo" + "Salvar".

• "Quero redefinir o banco do zero." → Feche o app e delete o arquivo
"app.db" da pasta de saída (bin\\...\\net8.0-windows\\). Ao reabrir, ele
será recriado.

• "Exportar CSV não aparece no menu." → A função existe em FileService,
mas o menu principal inclui JSON e TXT por padrão. Se quiser o CSV no
menu, chame ExportCsvAsync num handler similar.

11\) ROADMAP (IDEIAS FUTURAS)
\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\--
• Gráficos (pizza / barras) -- requer referência ao
System.Windows.Forms.DataVisualization • Tema escuro (toggle) •
Dashboard de tendências por período • Undo/Redo e histórico por registro
• Validações avançadas e máscaras • Autenticação básica e perfis de
usuário • Distribuição (MSIX) e atualização automática

\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\--

COMANDOS ÚTEIS (CLI)
\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\--
dotnet restore dotnet build -c Release dotnet run

Para publicar (x64): dotnet publish -c Release -r win-x64
\--self-contained false

\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\--
Dúvidas / melhorias? Abra uma issue ou me envie uma mensagem.
