# Prescription & Doctor Visit System

## 📌 Project Overview
The **Prescription & Doctor Visit System** is a web-based solution that facilitates:
- Storing and managing **prescriptions** issued by doctors.
- Managing **medicine records** in a NoSQL database.
- Sending **notifications** to patients and pharmacies.
- Handling **authentication** for users.
- **RabbitMQ integration** for message-based communication.
- **API Gateway** using Ocelot for routing and request handling.
- **Frontend application** for doctors and pharmacies, deployed via **Azure Static Web Apps**.

This system ensures smooth prescription management and medicine tracking by integrating **Azure SQL Database**, **MongoDB Atlas**, **Azure Cache for Redis**, **CloudAMQP (RabbitMQ)**, **Azure Static Web Apps**, and **Azure Logic Apps** for automation.

🔗 **You can reach the frontend repository through this link:** [Prescription System Frontend](https://github.com/handehazan/PrescriptionSysemFrontend)

---

## 🛠️ Tech Stack
- **Frontend:** React.js (deployed via Azure Static Web Apps)
- **Backend:** .NET 8 Web API
- **API Gateway:** Ocelot
- **Database:**
  - **MongoDB Atlas** (for medicine storage)
  - **Azure SQL Database** (for prescriptions and user data)
  - **Azure Cache for Redis** (for caching frequently used medicines)
- **Messaging Queue:** CloudAMQP (RabbitMQ)
- **Automation:** Azure Logic Apps
  - **Email Notifications:** Sends emails daily at 1 AM.
  - **Medicine Data Refresh:** Refreshes MongoDB medicines every two weeks on Sundays at 22:00.
- **Authentication:** JWT (JSON Web Token)
- **ORM:** Entity Framework Core
- **Web Scraping:** HtmlAgilityPack
- **Excel Parsing:** ExcelDataReader
- **Dependency Injection:** .NET Core built-in DI
- **Logging:** Serilog

---

## 📂 Project Architecture
This system is structured as a **multi-layered application**, with clear separation between:
1. **Frontend** – React-based UI for doctors and pharmacies.
2. **Controllers** – Handles HTTP requests.
3. **Services** – Implements business logic.
4. **Data Access Layer (DAL)** – Handles database interactions.
5. **Models** – Defines the structure of data entities.
6. **Contexts** – Manages database connections.
7. **Message Queue (RabbitMQ via CloudAMQP)** – Asynchronous communication.
8. **Caching Layer (Azure Redis)** – Caches frequently used medicines.
9. **Web Scraping & Data Parsing** – Downloads and processes medicine data.
10. **API Gateway (Ocelot)** – Handles request routing.
11. **Azure Logic Apps** – Automates scheduled tasks.

### 🔹 **Key Components**
| Layer        | Component                       | Description |
|-------------|--------------------------------|-------------|
| **Frontend** | React.js | UI for doctors and pharmacies deployed via Azure Static Web Apps |
| **Controllers** | MedicineController | Manages medicine-related endpoints with pagination and search |
|  | NotificationController | Handles sending and retrieving notifications via RabbitMQ |
|  | PrescriptionController | Manages prescription creation, retrieval, and submission |
|  | AuthController | Handles authentication (JWT-based login) |
| **Services** | MedicineService | Business logic for medicines, web scraping, and Redis caching |
|  | NotificationService | Manages message notifications and pharmacy alerts via RabbitMQ |
|  | PrescriptionService | Handles prescription operations, medicine assignments, and interactions with RabbitMQ |
|  | RabbitMQService | Publishes and subscribes to events via RabbitMQ |
| **Access Layer** | MedicineAccess | Handles MongoDB operations for medicines |
|  | PrescriptionAccess | Handles SQL-based prescription management |
| **Contexts** | NoSqlContext | MongoDB Atlas connection setup |
|  | SqlDbContext | Azure SQL Database context |
| **Automation** | Azure Logic Apps | Automates email notifications and medicine data refresh |
| **API Gateway** | Ocelot | Routes API requests and manages authentication |

---

## 📡 API Gateway Configuration (Ocelot)
The API Gateway is configured using **Ocelot** to route and manage API requests. The gateway exposes simplified upstream endpoints while forwarding requests to the appropriate backend services.

### **Ocelot Route Mappings**
| Upstream Endpoint | Downstream Endpoint | HTTP Method | Authentication |
|------------------|--------------------|-------------|---------------|
| `/auth/login` | `/api/auth/login` | POST | No |
| `/medicine/search` | `/api/v1/medicine/SearchMedicine` | GET | No |
| `/prescription/create` | `/api/v1/prescription/CreatePrescription` | POST | Yes (JWT) |
| `/prescription/{patientTC}` | `/api/v1/prescription/{patientTC}` | GET | No |
| `/prescription/medicines/{prescriptionId}` | `/api/v1/prescription/medicines/{prescriptionId}` | GET | No |
| `/prescription/submit` | `/api/v1/prescription/submit` | POST | Yes (JWT) |

**Base Gateway URL:** `https://prescriptionservicegateway-egfnhudhbwbnh5ct.canadacentral-01.azurewebsites.net`

---

## 📩 Cloud Services Integration
### **RabbitMQ via CloudAMQP**
This system uses **RabbitMQ** hosted on **CloudAMQP** for messaging. The `RabbitMQService` ensures:
- **Event-based communication** between services.
- **Asynchronous handling** of missing medicine alerts to pharmacies.

### **Azure Cache for Redis**
Redis is used to cache frequently searched medicines.

### **Azure Logic Apps**
Azure Logic Apps handle:
1. **Email Notifications** – Triggers a **daily HTTP request at 1 AM** to notify pharmacies.
2. **Medicine Data Refresh** – Triggers a **HTTP request every two weeks on Sundays at 22:00** to refresh MongoDB data.

---

## ⚙️ Installation & Setup

### 1️⃣ **Clone the Repository**
```bash
git clone https://github.com/your-username/prescription-system.git
cd prescription-system
```

### 2️⃣ **Run the Application**
```bash
dotnet run
```

---

## 🤝 Contributing
We welcome contributions! To contribute:
1. **Fork** the repository.
2. **Create a branch** for your feature (`git checkout -b feature-name`).
3. **Commit changes** (`git commit -m 'Added new feature'`).
4. **Push to GitHub** (`git push origin feature-name`).
5. **Submit a Pull Request**.

---

## 📜 License
This project is licensed under the **MIT License**.

---

🚀 **Now you're ready to start using the Prescription & Doctor Visit System!** 🚀

