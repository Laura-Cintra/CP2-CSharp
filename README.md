# Mottu API - Gestão de Motos e Filiais

API RESTful desenvolvida em .NET 8 para gerenciamento de motos e filiais, utilizando Oracle, Entity Framework Core, Clean Architecture e princípios de DDD.

*solucação criada para o [check-point 2](https://github.com/2TDSPK-25/CP2)*
---

## 📦 Estrutura da Solução

- **Domain**: Entidades de domínio, enums e exceções.
- **Application**: DTOs, validações e mapeamentos.
- **Infrastructure**: Contexto do EF Core, configurações de banco e mapeamentos Fluent API.
- **Presentation**: Controllers, endpoints e configuração de rotas.

---

## 🚀 Funcionalidades

- Cadastro, consulta, atualização e remoção de motos.
- Cadastro, consulta, atualização e remoção de filiais.
- Relacionamento muitos-para-um entre Moto e Filial.
- Validações de domínio e unicidade de placa.
- Documentação automática via Swagger.

---

## 🔗 Endpoints Principais

### Motos

- `GET /api/motos` — Lista todas as motos.
- `GET /api/motos/{id}` — Consulta uma moto pelo ID.
- `POST /api/motos` — Cadastra uma nova moto.
- `PATCH /api/motos/{id}` — Atualiza parcialmente uma moto.
- `DELETE /api/motos/{id}` — Remove uma moto.

### Filiais

- `GET /api/filial` — Lista todas as filiais.
- `GET /api/filial/{id}` — Consulta uma filial pelo ID.
- `POST /api/filial` — Cadastra uma nova filial.
- `PATCH /api/filial/{id}` — Atualiza parcialmente uma filial.
- `DELETE /api/filial/{id}` — Remove uma filial.

---

## ⚙️ Tecnologias Utilizadas

- .NET 8 / ASP.NET Core
- Entity Framework Core 9
- Oracle (Oracle.EntityFrameworkCore)
- Swagger (Swashbuckle)
- Clean Architecture
- Domain-Driven Design (DDD)

---

## 🏗️ Como Executar

1. **Configurar a string de conexão Oracle**
   - No `appsettings.json` ou via variável de ambiente:
     ```
     "ConnectionStrings": {
       "Oracle": "Data Source=...;User ID=...;Password=..."
     }
     ```
2. **Restaurar pacotes e aplicar migrations**
   (No CMD da aplicação) - `dotnet restore dotnet ef database update`

4. **Executar a aplicação**
   (No CMD da aplicação) - `dotnet run` 

Acesse o Swagger em: `https://localhost:7018/swagger/index.html`
---

## 👥 Equipe - Prisma.Code
- Laura de Oliveira Cintra - RM 558843
- Maria Eduarda Alves da Paixão - RM 558832
- Vinícius Saes de Souza - RM 554456

> “Faça o teu melhor, na condição que você tem, enquanto você não tem condições melhores, para fazer melhor ainda.” — Mario Sergio Cortella
