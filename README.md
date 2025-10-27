# pb-credit-system
Desafio

🏦 PB Credit System

Sistema de análise e concessão de crédito baseado em arquitetura de microsserviços desenvolvido em .NET 8.

📋 Descrição do Projeto

Sistema completo para cadastro de clientes, análise de propostas de crédito e emissão de cartões, seguindo as regras de negócio estabelecidas pelo PB.

🎯 Funcionalidades

Cadastro de Clientes via API REST

Análise Automática de Crédito baseada em score

Emissão de Cartões com limites variáveis

Comunicação Assíncrona entre microsserviços via RabbitMQ

Resiliência a Falhas com retry patterns e dead letter queues

🏗️ Arquitetura

Microsserviços
CustomerService - Cadastro e gestão de clientes
CreditProposalService - Análise e scoring de propostas
CreditCardService - Emissão e gestão de cartões
Tecnologias
.NET 8.0 - Framework principal
Entity Framework Core - ORM e acesso a dados
RabbitMQ - Mensageria assíncrona
MassTransit - Biblioteca de mensageria
SQL Server - Banco de dados
xUnit - Testes unitários
FluentValidation - Validações
Docker - Containerização

🚀 Como Executar
Pré-requisitos
.NET 8.0 SDK

Docker e Docker Compose

SQL Server (pode ser via Docker)

1. Clone o repositório
bash
git clone https://github.com/seu-usuario/pb-credit-system.git
cd pb-credit-system

3. Execute a infraestrutura com Docker
bash
docker-compose up -d

4. Execute os microsserviços
bash
### Customer Service
dotnet run --project src/Services/CustomerService/PB.CustomerService.API

### Credit Proposal Service  
dotnet run --project src/Services/CreditProposalService/PB.CreditProposalService.API

### Credit Card Service
dotnet run --project src/Services/CreditCardService/PB.CreditCardService.API
4. Acesse as APIs
Customer Service: http://localhost:5000

Credit Proposal Service: http://localhost:5001

Credit Card Service: http://localhost:5002

📡 Endpoints da API
Customer Service
http
POST /api/customers
Content-Type: application/json

json: {
  "name": "João Silva",
  "cpf": "123.456.789-00",
  "email": "joao@email.com",
  "phone": "(11) 99999-9999",
  "birthDate": "1990-01-01",
  "monthlyIncome": 5000.00,
  "address": {
    "street": "Rua Exemplo",
    "number": "123",
    "complement": "Apto 45",
    "neighborhood": "Centro",
    "city": "São Paulo",
    "state": "SP",
    "zipCode": "01234-567"
  }
}

http
GET /api/customers/{id}
GET /api/customers/cpf/{cpf}

🧪 Testes
Executar testes unitários
bash
### Todos os testes
dotnet test

### Testes específicos
dotnet test tests/PB.CustomerService.Tests
dotnet test tests/PB.CreditProposalService.Tests  
dotnet test tests/PB.CreditCardService.Tests
Cobertura de testes
CustomerService: Testes de domínio e validações

CreditProposalService: Testes do ScoreCalculator

CreditCardService: Testes de emissão de cartões
