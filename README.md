🏦 New Bank — Sistema de Gestão Bancária e API REST
<p align="center"> Sistema bancário completo desenvolvido com <strong>ASP.NET Core Web API</strong>, <strong>Entity Framework Core</strong>, <strong>MySQL</strong> e <strong>Front-end responsivo</strong> utilizando Bootstrap 5. </p> <p align="center"> <img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=.net" /> <img src="https://img.shields.io/badge/C%23-12.0-239120?style=for-the-badge&logo=c-sharp" /> <img src="https://img.shields.io/badge/MySQL-Database-4479A1?style=for-the-badge&logo=mysql" /> <img src="https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=for-the-badge&logo=bootstrap" /> <img src="https://img.shields.io/badge/JWT-Authentication-black?style=for-the-badge&logo=jsonwebtokens" /> </p>
📖 Sobre o Projeto

O New Bank foi desenvolvido como projeto final da Sprint 3 — Desenvolvimento de APIs e Serviços Web.

A aplicação consiste em:

🔹 API REST robusta construída com ASP.NET Core;
<br/>
🔹 Integração com banco relacional MySQL utilizando Entity Framework Core;
<br/>
🔹 Sistema completo de autenticação e autorização via JWT;
<br/>

🔹 Interface web dinâmica e responsiva utilizando HTML, CSS, Bootstrap e JavaScript;
<br/>

🔹 Aplicação dos principais conceitos de Programação Orientada a Objetos (POO) e arquitetura em camadas.
<br/>

🚀 Tecnologias Utilizadas
🔧 Back-end
C# 12
.NET 8
ASP.NET Core Web API
Entity Framework Core (Code First)
JWT Authentication
Swagger/OpenAPI
🗄️ Banco de Dados
MySQL
Entity Framework Migrations
🎨 Front-end
HTML5
CSS3
Bootstrap 5.3.2
JavaScript Vanilla
Fetch API
🏛️ Arquitetura do Projeto

O sistema segue uma arquitetura baseada em:

Models
Controllers
DTOs
Services
Entity Framework Context

Com separação clara de responsabilidades e aplicação de boas práticas de desenvolvimento.

🧠 Conceitos Aplicados
🔄 Herança e Polimorfismo

A entidade abstrata ContaBancaria serve como base para diferentes tipos de conta:

💳 Conta Corrente
Taxa de serviço de R$ 5,00 aplicada em:
Saques
Transferências
💰 Conta Poupança
Método exclusivo:
AplicarRendimento()
Geração automática de lucro sobre saldo.
🏢 Conta Empresarial
Possui:
Limite de empréstimo
Solicitação de crédito empresarial

O projeto utiliza estratégia TPH (Table-Per-Hierarchy) no Entity Framework Core.

🔐 Segurança e Controle de Acesso

A API utiliza autenticação baseada em JWT (JSON Web Token) com rotas protegidas através do atributo:

[Authorize]

O front-end realiza a leitura do token JWT para aplicar:

Redirecionamento por função (Role-Based Routing);
Controle de permissões;
Restrição de funcionalidades.
👨‍💼 Administrador
Acesso ao painel de gestão;
Gerenciamento completo de clientes;
Não pode receber PIX ou transferências.
👤 Cliente
Acesso ao dashboard financeiro;
Operações bancárias completas;
Consulta de extrato e transferências.
⚙️ Funcionalidades
🔐 Sistema Base
Cadastro de usuários;
Abertura automática de conta bancária;
Login com autenticação JWT;
Proteção de rotas.
👨‍💼 Painel Administrativo
Funcionalidades:
📋 Listagem de clientes;
✏️ Atualização de dados em tempo real;
❌ Exclusão definitiva de contas;
🔍 Gerenciamento completo do sistema bancário.
Operações disponíveis:
Método	Funcionalidade
GET	Listar clientes
PUT	Atualizar titular e CPF
DELETE	Encerrar conta
💳 Painel do Cliente
Operações Bancárias
Depósitos;
Saques;
Transferências;
PIX por CPF;
Consulta de saldo;
Extrato detalhado.
Recursos Especiais
Conta Poupança
Aplicação de rendimento automático.
Conta Empresarial
Solicitação de empréstimos.
🧾 Extrato Bancário

O sistema possui extrato completo com:

Histórico cronológico;
Identificação de entradas e saídas;
Valores formatados;
Destaque visual:
🟢 Entradas
🔴 Saídas
🔄 Transferência PIX

O sistema realiza transferências via CPF validando:

Existência da conta destino;
Bloqueio de auto transferência;
Restrição para contas administrativas;
Saldo disponível;
Segurança da operação.
🛠️ Como Executar o Projeto
1️⃣ Pré-requisitos

Instale:

.NET SDK 8+
MySQL Server
Visual Studio 2022 ou VS Code
2️⃣ Clonar o Repositório
git clone [LINK_DO_REPOSITORIO]
3️⃣ Configurar Banco de Dados

Abra o arquivo:

appsettings.json

Configure sua string de conexão MySQL.

4️⃣ Aplicar as Migrations

Abra o Package Manager Console e execute:

Update-Database

Ou via terminal:

dotnet ef database update
5️⃣ Executar a API
dotnet run

A API iniciará juntamente com:

Swagger/OpenAPI;
Endpoints documentados;
Ambiente local de testes.
6️⃣ Executar o Front-end

Abra:

index.html

em seu navegador.

📌 Requisitos Atendidos

✅ CRUD completo
✅ API RESTful
✅ Banco de dados relacional
✅ Entity Framework Core
✅ JWT Authentication
✅ Arquitetura em camadas
✅ Programação Orientada a Objetos
✅ Front-end integrado
✅ Tratamento de Status HTTP
✅ Segurança e controle de acesso

Desenvolvido por Gabriel Vinicius.

⭐ Considerações Finais

O New Bank foi desenvolvido com foco em:

Arquitetura limpa;
Escalabilidade;
Segurança;
Organização de código;
Experiência do usuário;
Aplicação prática de conceitos modernos de desenvolvimento Full Stack.
<p align="center"> Feito com dedicação 🚀 </p>
