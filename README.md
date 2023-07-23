# Devameet C#

## Description
Este projeto foi desenvolvido em C#, baseado numa fus�o do Meet da Google com o Gather, o Devameet.
Os prot�tipos e regras do neg�cio est�o neste link do figma:
https://www.figma.com/file/mIXcu8SJWqi0ylVHtZn89a/Devameet-(Projeto-2023)

## Tecnologias

- C#
- ASP.NET Core
- Entity Framework Core
- SQL Server

### Depend�ncias
- Microsoft.EntityFrameworkCore (7.0.4)
- Microsoft.EntityFrameworkCore.Design (7.0.4)
- Microsoft.EntityFrameworkCore.SqlServer (7.0.4)
- WebSocket (para comunica��o em tempo real)
- Microsoft.EntityFrameworkCore.Tools (7.0.4)


## Features
- Cadastro de usu�rios
- Autentica��o de usu�rios (login/logout)
- Cria��o, edi��o e exclus�o de salas de reuni�o
  
## Instalation
1. Clone o reposit�rio: `git clone git@github.com:Elianehenri/Devameet-CSharp.git`
1. Entre na pasta do projeto: `cd Devameet-CSharp`
1. Instale as depend�ncias: `dotnet restore`
1. Execute o projeto: `dotnet run`
1. Acesse o Swagger em: `http://localhost:porta/swagger`				
1. Configurar no arquivo appsettings.json a string de conexao com o banco de dados.
1. Executar o comando `dotnet ef database update` para criar o banco de dados.

	
#
### Autor
* **Eliane Henriqueta**

