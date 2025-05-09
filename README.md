# 🔐 UserAuthAPI

Uma API de autenticação desenvolvida com foco em segurança, boas práticas e escalabilidade. Utiliza JWT (JSON Web Token) para autenticação, DTOs para tráfego seguro de dados e Entity Framework Core com SQL Server para persistência.

---

## 🚀 Funcionalidades implementadas

- ✅ Cadastro e login de usuários
- ✅ Geração e validação de tokens JWT
- ✅ Proteção de rotas com `[Authorize]`
- ✅ Estrutura com camadas organizadas (Controller, Service, DTO, Model)
- ✅ Swagger para teste e documentação dos endpoints

---

## 💻 Tecnologias utilizadas

- **ASP.NET Core** – Backend da aplicação
- **Entity Framework Core + SQL Server** – Persistência de dados
- **JWT** – Autenticação segura
- **AutoMapper** – Mapeamento entre modelos e DTOs
- **Swagger** – Documentação e testes via interface

---

## 🧠 Arquitetura e conceitos aplicados

- Separação de responsabilidades entre camadas
- Uso de **DTOs** para segurança no tráfego de dados
- Configuração centralizada do JWT
- Mapeamento com **AutoMapper**
- Testes de endpoints com **Swagger** e **Insomnia**

---

## 📂 Estrutura do projeto

```bash
UserAuthAPI/
├── Controllers/
│   └── UserController.cs
├── DTOs/
│   ├── UserDTO.cs
│   ├── UserLoginDTO.cs
│   └── UserRegisterDTO.cs
├── Data/
│   └── AppDbContext.cs
├── Mappings/
│   └── UserDTOMappingProfile.cs
├── Models/
│   └── User.cs
├── Configurations/
│   └── JwtSettings.cs
├── Services/
│   └── TokenService.cs
├── Properties/
│   └── launchSettings.json
├── Program.cs
├── appsettings.json
```
---

## ▶️ Como executar o projeto

1. Clone o repositório:

```bash
git clone https://github.com/fernandoportodev/UserAuthAPI.git
```

2. Configure o `appsettings.json` com a sua `ConnectionString` e chave do JWT:

```json
"JwtSettings": {
  "SecretKey": "sua-chave-secreta-aqui",
  "Issuer": "sua-api",
  "Audience": "seus-usuarios"
},
"ConnectionStrings": {
  "DefaultConnection": "Server=SEU_SERVIDOR;Database=UserAuthDb;Trusted_Connection=True;"
}
```

3. Execute as migrações (caso esteja usando EF Core):

```bash
dotnet ef database update
```

4. Inicie a aplicação:

```bash
dotnet run
```


