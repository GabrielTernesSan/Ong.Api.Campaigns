# 📢 Ong.Api.Campaigns — Serviço de Campanhas e Doações

Microsserviço responsável pela **gestão de campanhas**, **processamento de intenções de doação** e **painel de transparência** da plataforma **Conexão Solidária**.

---

## 📐 Responsabilidades

| Recurso | Método | Acesso | Descrição |
|---|---|---|---|
| `/campaigns` | POST | JWT (GestorONG) | Cria nova campanha |
| `/campaigns/{id}` | PUT | JWT (GestorONG) | Atualiza campanha existente |
| `/campaigns/active` | GET | Público | Lista campanhas ativas com total arrecadado |
| `/donations/{campaignId}` | POST | JWT (Doador) | Registra intenção de doação |
| `/campaigns/{id}/donation-received` | PATCH | API Key | Worker notifica doação processada |
| `/campaigns/outbox` | GET | API Key | Lista Outbox pendente do Worker |
| `/campaigns/outbox/{id}/processed` | PATCH | API Key | Marca mensagem como processada |
| `/campaigns/outbox/{id}/error` | PATCH | API Key | Registra erro no processamento |
| `/campaigns/users` | POST | API Key | Worker sincroniza usuário criado no Auth |
| `/health` | GET | Público | Health check |
| `/metrics` | GET | Público | Métricas Prometheus (OpenTelemetry) |

---

## 📋 Regras de Negócio

- **Campanha:** `DataFim` não pode estar no passado; `MetaFinanceira` deve ser > 0.
- **Status válidos:** `Ativa`, `Concluida`, `Cancelada`.
- **Doação:** só pode ser feita em campanhas com status `Ativa`.
- O endpoint `POST /donations` **não** atualiza o valor arrecadado diretamente — publica um `DonationCreated` via padrão Outbox/RabbitMQ. O Worker processa e chama `PATCH /campaigns/{id}/donation-received`.

---

## 🏗️ Arquitetura Interna

```
Ong.Api.Campaigns     → Camada de entrada (Minimal API, Swagger, JWT + RBAC)
Ong.Application       → Handlers MediatR (CreateCampaign, Donation, GetActive...)
Ong.Domain            → Entidades, contratos (Campaign, Donation, ICampaignRepository)
Ong.Infra             → EF Core + PostgreSQL, Outbox, Repositories
Ong.Commom            → DTOs compartilhados (DonationCreated, PagedResponse<T>)
```

### Fluxo de Doação (Assíncrono)

```
[Doador] POST /donations/{id}
    ↓
[Api.Campaigns] Persiste Donation + OutboxMessage (transação atômica)
    ↓
[Worker - Job Minutely] GET /campaigns/outbox
    ↓
[Worker] Publica DonationCreated → RabbitMQ
    ↓
[Worker - Consumer] Consome fila → PATCH /campaigns/{id}/donation-received
    ↓
[Api.Campaigns] Incrementa ValorArrecadado da campanha
```

---

## ⚙️ Pré-requisitos

| Ferramenta | Versão mínima |
|---|---|
| .NET SDK | 10.0 (preview) |
| Docker + Docker Compose | 24+ |
| PostgreSQL | 15+ |
| RabbitMQ | 3.12+ |

---

## 🚀 Rodando localmente

### 1. Clone o repositório

```bash
git clone https://github.com/<seu-org>/Ong.Api.Campaigns.git
cd Ong.Api.Campaigns
```

### 2. Configure as variáveis de ambiente

Configure o arquivo `src/Ong.Api.Campaigns/appsettings.Development.json` (ou use variáveis de ambiente)

### 3. Suba as dependências via Docker

```bash
# PostgreSQL para Campaigns (porta diferente do Auth)
docker run -d \
  --name postgres-campaigns \
  -e POSTGRES_DB=ong_campaigns \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=postgres \
  -p 5433:5432 \
  postgres:15

# RabbitMQ
docker run -d \
  --name rabbitmq \
  -e RABBITMQ_DEFAULT_USER=guest \
  -e RABBITMQ_DEFAULT_PASS=guest \
  -p 5672:5672 \
  -p 15672:15672 \
  rabbitmq:3-management
```

### 4. Aplique as migrations

```bash
dotnet ef database update --project src/Ong.Infra/Ong.Infra.csproj \
  --startup-project src/Ong.Api.Campaigns/Ong.Api.Campaigns.csproj
```

### 5. Execute a API

```bash
dotnet run --project src/Ong.Api.Campaigns/Ong.Api.Campaigns.csproj
```

Disponível em:
- **Swagger UI:** http://localhost:5001/swagger
- **Health:** http://localhost:5001/health
- **Métricas:** http://localhost:5001/metrics
- **RabbitMQ Management:** http://localhost:15672 (guest/guest)

---

## 🐳 Rodando via Docker

```bash
docker build -f src/Ong.Api.Campaigns/Dockerfile -t ong-api-campaigns:local .

docker run -d \
  --name ong-api-campaigns \
  -p 5001:8080 \
  -e ConnectionStrings__DefaultConnection="Host=host.docker.internal;Port=5433;Database=ong_campaigns;Username=postgres;Password=postgres" \
  -e Jwt__Key="sua-chave-secreta-minimo-32-caracteres!!" \
  -e Jwt__Issuer="ong-auth" \
  -e Jwt__Audience="ong-platform" \
  -e ApiKeys__WorkerKey="chave-interna-worker-1234" \
  ong-api-campaigns:local
```

---

## ☸️ Kubernetes

```bash
kubectl apply -f k8s/campaigns/
kubectl get pods -n conexao-solidaria
```

## 🔄 Pipeline CI/CD

| Pipeline | Gatilho | O que faz |
|---|---|---|
| `ci.yml` | Push/PR em `main` | Restore → Build → Testes → Publica resultados |
| `cd.yml` | CI com sucesso | Login GHCR → Docker Build → Push da imagem |

Imagem publicada em: `ghcr.io/<org>/ong-api-campaigns:latest`.

---

## 📊 Observabilidade

Métricas OpenTelemetry expostas em `/metrics` (formato Prometheus):

- `http_server_request_duration_seconds` — latência HTTP por rota
- `http_server_active_requests` — requisições em andamento
- `process_runtime_dotnet_gc_collections_count_total` — coletas do GC

Veja configuração completa em `observability/`.

---

## 🔑 Autenticação

- **Endpoints públicos:** `/campaigns/active`
- **JWT (Doador):** `POST /donations/{id}` — requer role `Doador`
- **JWT (GestorONG):** `POST /campaigns`, `PUT /campaigns/{id}` — requer role `GestorONG`
- **API Key (Worker):** endpoints de Outbox e sincronização — header `x-api-key`