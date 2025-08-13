# Sistema de Gerenciamento de Propostas de Seguro

Este projeto implementa um sistema de gerenciamento de propostas de seguro, dividido em dois microserviços principais: `PropostaService` e `ContratacaoService`. O sistema segue a arquitetura hexagonal (Ports & Adapters) e princípios de Clean Architecture, DDD e SOLID.

## Arquitetura

O sistema é composto por dois microserviços independentes que se comunicam via HTTP REST:

### PropostaService
Responsável por:
- Criar proposta de seguro
- Listar propostas
- Alterar status da proposta (Em Análise, Aprovada, Rejeitada)
- Expor API REST

### ContratacaoService
Responsável por:
- Contratar uma proposta (somente se Aprovada)
- Armazenar informações da contratação (ID da proposta, data de contratação)
- Comunicar-se com o PropostaService para verificar status da proposta
- Expor API REST

## Tecnologias Utilizadas

- **Linguagem**: C# com .NET 8
- **Banco de Dados**: PostgreSQL
- **ORM**: Entity Framework Core
- **Comunicação**: HTTP REST
- **Testes**: xUnit, Moq, FluentAssertions
- **Containerização**: Docker e Docker Compose
- **Arquitetura**: Hexagonal (Ports & Adapters)

## Estrutura do Projeto

```
seguro_propostas/
├── PropostaService/                 # Microserviço de Propostas
│   ├── Domain/                      # Camada de Domínio
│   │   ├── Entities/               # Entidades de negócio
│   │   ├── Enums/                  # Enumerações
│   │   └── Interfaces/             # Interfaces (Ports)
│   ├── Application/                 # Camada de Aplicação
│   │   ├── Services/               # Serviços de aplicação
│   │   ├── Commands/               # Commands
│   │   └── DTOs/                   # Data Transfer Objects
│   ├── Infrastructure/              # Camada de Infraestrutura
│   │   ├── Data/                   # Contexto do banco
│   │   └── Repositories/           # Implementações (Adapters)
│   └── Controllers/                 # Controllers da API
├── ContratacaoService/              # Microserviço de Contratação
│   ├── Domain/                      # Camada de Domínio
│   ├── Application/                 # Camada de Aplicação
│   ├── Infrastructure/              # Camada de Infraestrutura
│   │   ├── Data/                   # Contexto do banco
│   │   ├── Repositories/           # Repositórios
│   │   └── Gateways/               # Gateways para comunicação
│   └── Controllers/                 # Controllers da API
├── PropostaService.Tests/           # Testes do PropostaService
├── ContratacaoService.Tests/        # Testes do ContratacaoService
├── docs/                           # Documentação
├── scripts/                        # Scripts de banco de dados
└── docker-compose.yml              # Configuração Docker
```

## Pré-requisitos

- .NET 8 SDK
- Docker e Docker Compose
- SQLServer (se executar localmente)

## Como Executar

### Opção 1: Com Docker Compose (Recomendado)

1. Clone o repositório:
```bash
git clone <url-do-repositorio>
cd seguro_propostas
```

2. Execute com Docker Compose:
```bash
docker-compose up --build
```

3. Os serviços estarão disponíveis em:
   - PropostaService: http://localhost:5000
   - ContratacaoService: http://localhost:5001   

### Opção 2: Execução Local

1. Instale o SQLServer e configure as bases de dados:
```sql
CREATE DATABASE "PropostaServiceDb";
CREATE DATABASE "ContratacaoServiceDb";
```

2. Execute as migrations:
```bash
cd PropostaService
dotnet ef database update

cd ../ContratacaoService
dotnet ef database update
```

3. Execute os serviços:
```bash
# Terminal 1 - PropostaService
cd PropostaService
dotnet run

# Terminal 2 - ContratacaoService
cd ContratacaoService
dotnet run
```

## Executar Testes

```bash
# Testes do PropostaService
cd PropostaService.Tests
dotnet test

# Testes do ContratacaoService
cd ContratacaoService.Tests
dotnet test

# Executar todos os testes
dotnet test
```

## API Endpoints

### PropostaService (http://localhost:5000)

- `POST /api/propostas` - Criar nova proposta
- `GET /api/propostas` - Listar todas as propostas
- `GET /api/propostas/{id}` - Obter proposta por ID
- `PUT /api/propostas/{id}/status` - Atualizar status da proposta

### ContratacaoService (http://localhost:5001)

- `POST /api/contratacoes` - Contratar uma proposta

## Exemplos de Uso

### Criar uma Proposta
```bash
curl -X POST http://localhost:5000/api/propostas \
  -H "Content-Type: application/json" \
  -d '{
    "clienteId": "123e4567-e89b-12d3-a456-426614174000",
    "valorProposta": 1500.00
  }'
```

### Aprovar uma Proposta
```bash
curl -X PUT http://localhost:5000/api/propostas/{id}/status \
  -H "Content-Type: application/json" \
  -d '{
    "novoStatus": 2
  }'
```

### Contratar uma Proposta
```bash
curl -X POST http://localhost:5001/api/contratacoes \
  -H "Content-Type: application/json" \
  -d '{
    "propostaId": "123e4567-e89b-12d3-a456-426614174000"
  }'
```

## Princípios Aplicados

### Arquitetura Hexagonal
- **Ports**: Interfaces que definem contratos (IPropostaRepository, IPropostaService, etc.)
- **Adapters**: Implementações concretas (PropostaRepository, PropostaController, etc.)
- **Domain**: Lógica de negócio isolada de dependências externas

### DDD (Domain-Driven Design)
- Entidades ricas com comportamentos
- Agregados bem definidos
- Separação clara entre domínio e infraestrutura

### SOLID
- **S**ingle Responsibility: Cada classe tem uma única responsabilidade
- **O**pen/Closed: Extensível via interfaces, fechado para modificação
- **L**iskov Substitution: Implementações podem ser substituídas
- **I**nterface Segregation: Interfaces específicas e coesas
- **D**ependency Inversion: Dependência de abstrações, não implementações

### Clean Code
- Nomes expressivos
- Métodos pequenos e focados
- Comentários apenas quando necessário
- Tratamento adequado de exceções

## Testes

O projeto inclui testes unitários abrangentes:
- **PropostaService**: 14 testes passando
- **ContratacaoService**: 7 testes passando

Os testes cobrem:
- Entidades de domínio
- Serviços de aplicação
- Repositórios
- Cenários de sucesso e falha

## Contribuição

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## Licença

Este projeto está sob a licença MIT. Veja o arquivo `LICENSE` para mais detalhes.

