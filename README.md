# Devameet C#

## Description
Este projeto foi desenvolvido em C#, baseado numa fusão do Meet da Google com o Gather, o Devameet.
Os protótipos e regras do negócio estão neste link do figma:
https://www.figma.com/file/mIXcu8SJWqi0ylVHtZn89a/Devameet-(Projeto-2023)

## Tecnologias

- C#
- ASP.NET Core
- Entity Framework Core
- SQL Server

### Dependências
- Microsoft.EntityFrameworkCore (7.0.4)
- Microsoft.EntityFrameworkCore.Design (7.0.4)
- Microsoft.EntityFrameworkCore.SqlServer (7.0.4)
- WebSocket (para comunicação em tempo real)
- Microsoft.EntityFrameworkCore.Tools (7.0.4)


## Features
- Cadastro de usuários
- Autenticação de usuários (login/logout)
- Criação, edição e exclusão de salas de reunião
  
## Instalation
1. Clone o repositório: `git clone git@github.com:Elianehenri/Devameet-CSharp.git`
1. Entre na pasta do projeto: `cd Devameet-CSharp`
1. Instale as dependências: `dotnet restore`
1. Execute o projeto: `dotnet run`
1. Acesse o Swagger em: `http://localhost:porta/swagger`				
1. Configurar no arquivo appsettings.json a string de conexao com o banco de dados.
1. Executar o comando `dotnet ef database update` para criar o banco de dados.

	
#
### Autor
* **Eliane Henriqueta**

