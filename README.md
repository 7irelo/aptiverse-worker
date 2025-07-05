# 🧠 Aptiverse Worker – AI Task Processor for RabbitMQ Queue

The **Aptiverse Worker** is a background service responsible for consuming messages from **RabbitMQ**, processing requests related to AI/ML models (hosted in a Python FastAPI service), and returning results or updating relevant data stores.

This service acts as a decoupling layer between the **Aptiverse API (.NET 8)** and the **FastAPI-based AI services**, enabling **asynchronous**, **resilient**, and **scalable** model execution.

---

## 🎯 Purpose

* ✅ Decouple long-running AI inference from the main API
* ✅ Asynchronously process tasks such as summarization, emotion detection, or learning analysis
* ✅ Communicate with FastAPI microservices over HTTP or internal APIs
* ✅ Enable retry, failure logging, and dead-letter queueing
* ✅ Support concurrent message consumption with worker scaling

---

## 🔧 Tech Stack

| Component        | Technology                         |
| ---------------- | ---------------------------------- |
| Language         | .NET 8 / C#                        |
| Message Broker   | RabbitMQ                           |
| API Bridge       | RestSharp (to call FastAPI)        |
| Hosting          | Docker, AWS ECS / EC2              |
| Config & Secrets | `appsettings.json`, ENV vars       |
| Monitoring       | Serilog / OpenTelemetry (optional) |

---

## 📦 Project Structure

```
Aptiverse.Worker
├── Program.cs               → Worker entry point (HostBuilder)
├── Services
│   └── MessageProcessor.cs  → Handles incoming messages
├── Consumers
│   └── RabbitMqConsumer.cs  → Connects to queue and consumes tasks
├── Clients
│   └── FastApiClient.cs     → Uses RestSharp to call Python models
├── Models
│   └── TaskPayload.cs       → Task definitions and data contracts
├── Config
│   └── RabbitMqSettings.cs  → Queue & connection configuration
├── Utilities
│   └── RetryPolicy.cs       → Polly-based retry handling
└── appsettings.json         → Config for queue, API, and logging
```

---

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/your-org/aptiverse-worker.git
cd aptiverse-worker
```

### 2. Configure RabbitMQ and FastAPI Endpoint

Update `appsettings.json`:

```json
"RabbitMQ": {
  "Host": "rabbitmq://localhost",
  "Queue": "ai-tasks"
},
"FastApi": {
  "BaseUrl": "http://localhost:8000/api"
}
```

Or use environment variables in deployment.

### 3. Run the Worker

```bash
dotnet build
dotnet run
```

Or run with Docker:

```bash
docker build -t aptiverse-worker .
docker run --env-file .env aptiverse-worker
```

---

## 📬 Message Format (Example)

```json
{
  "taskId": "abc123",
  "userId": "student001",
  "taskType": "summarization",
  "inputText": "Photosynthesis is the process by which..."
}
```

---

## 🔁 Message Flow

1. `.NET API` sends JSON payload to RabbitMQ queue (`ai-tasks`)
2. `RabbitMqConsumer` listens for messages
3. `MessageProcessor` routes the task type (e.g., summarization → `/summarize`)
4. `FastApiClient` calls the correct endpoint
5. Result is either:

   * Sent back via HTTP webhook or callback
   * Stored in database
   * Logged and acknowledged

---

## 🧪 Testing the Worker

You can simulate a job by manually publishing to the queue using tools like:

* **RabbitMQ Management UI**
* `rabbitmqadmin` CLI
* `Postman + API endpoint` that enqueues test jobs

---

## 🛡️ Error Handling

* ✅ Retry policy using [Polly](https://github.com/App-vNext/Polly)
* ❌ Failed messages routed to a **dead-letter queue**
* 📦 Logs are emitted to console, file, or centralized log service

---

## 📈 Monitoring & Scaling

* Add health check endpoints with [AspNetCore.Diagnostics.HealthChecks](https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks)
* Use **Prometheus + Grafana** or **AWS CloudWatch** for queue depth & worker status
* Run multiple worker instances to scale horizontally

---

## 📌 Example Use Cases

| Task Type         | FastAPI Endpoint    | Output Example              |
| ----------------- | ------------------- | --------------------------- |
| `summarization`   | `/summarize`        | Short paragraph summary     |
| `emotion_check`   | `/emotion-analysis` | `["stressed", "motivated"]` |
| `topic_extractor` | `/topics`           | `["algebra", "cells"]`      |

---

## 🧠 Future Improvements

* Add OpenTelemetry tracing to visualize full API → MQ → AI call chain
* Add job result callback to `.NET API` or store results in PostgreSQL
* Add in-memory cache for duplicate task detection
* Rate limit queue submissions per student or IP

---

## 🤝 Contribution

We welcome feedback and contributions! Open an issue, create a pull request, or suggest improvements to task routing and error handling.

---

## 🪪 License

This project is proprietary under Aptiverse Labs. Licensing options for collaborators will be available in the public launch phase.

---

## 💡 Part of the Aptiverse Ecosystem

> *“Bridging the gap between intelligence, emotion, and access.”*
> The Aptiverse Worker is the engine that powers real-time educational insights, smart recommendations, and scalable AI processing across South Africa's next-generation learning platform.

---

Would you like this in `.md` file format or Docker-compose integration with RabbitMQ for local testing as well?
