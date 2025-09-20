
# Sprint 3 — WinForms (Projeto Único) — v2 (EnsureCreated)

- CRUD completo (SQLite/EF Core)
- Import/Export JSON + Export TXT + audit.log
- UI WinForms com DataGridView
- Projeto único (.sln incluso) — F5 abre a janela

## Como rodar
1) Abra `Sprint3WinForms.sln` no Visual Studio (workload Desktop .NET + .NET 8).  
2) Pressione **F5**. O banco `app.db` é criado automaticamente com **EnsureCreated** (não precisa migrations).  
3) Use **Arquivo → Importar/Exportar** ou os botões para CRUD.


## Escopo (Case 1 — Apostas Compulsivas)
App WinForms para rastrear **Apostadores** em risco de compulsão. CRUD completo + avaliação de risco (botão "Avaliar risco") e plano de ação em `Recomendacao`.
Permite Importar/Exportar JSON e exportar TXT.

**Campos**: Nome, Idade, FrequenciaSemanal, TempoMedioSessaoMin, PerdasUltimoMes, SinaisAlerta, NivelRisco, Recomendacao, DataUltimaAvaliacao.
