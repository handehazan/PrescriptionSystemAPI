# Prescription & Doctor Visit System
The Prescription & Doctor Visit System is a cloud-based web application designed to streamline prescription management for doctors and pharmacies. The system allows doctors to issue prescriptions, pharmacies to validate and fulfill prescriptions, and pharmacies to receive notifications about their missing submissions.

## 📌 Project Overview

The **Prescription & Doctor Visit System** is a web-based solution that facilitates:

- Storing and managing **prescriptions** issued by doctors.
- Managing **medicine records** in a NoSQL database.
- Sending **notifications** to pharmacies about uncompleted prescriptions.
- Handling **authentication** for users using **JWT tokens**.
- **RabbitMQ integration** for message-based communication.
- **API Gateway** using Ocelot for routing and request handling.
- **Frontend application** for doctors and pharmacies, deployed via **Azure Static Web Apps**.

This system ensures smooth prescription management and medicine tracking by integrating **Azure SQL Database**, **MongoDB Atlas**, **Azure Cache for Redis**, **CloudAMQP (RabbitMQ)**, **Azure Static Web Apps**, and **Azure Logic Apps** for automation.


🔗 **You can reach the web site through this link:** [Web Site](https://wonderful-river-0db27400f.4.azurestaticapps.net/index.html)

🔗 **You can reach the frontend repository through this link:** [Prescription System Frontend](https://github.com/handehazan/PrescriptionSysemFrontend)

🔗 **You can reach the swagger through this link:** [Swagger](https://prescriptionsystem-chhsbsebereue3a4.northeurope-01.azurewebsites.net/index.html)

🎥 **Watch the project walkthrough here:** [Project Video](https://youtu.be/Nrh2yDSBDUU)

---

## 🛠️ Tech Stack

- **Frontend:** HTML, CSS, JavaScript (deployed via Azure Static Web Apps)
- **Backend:** .NET 8 Web API
- **API Gateway:** Ocelot
- **Database:**
  - **MongoDB Atlas** (for medicine storage)
  - **Azure SQL Database** (for prescriptions)
  - **Azure Cache for Redis** (for caching frequently used medicines)
- **Messaging Queue:** CloudAMQP (RabbitMQ)
- **Automation:** Azure Logic Apps
  - **Email Notifications:** Sends emails daily at 1 AM to pharmacies.
  - **Medicine Data Refresh:** Refreshes MongoDB medicines every two weeks on Sundays at 22:00.
- **Authentication:** JWT (JSON Web Token) for secure API access.
- **Web Scraping:** HtmlAgilityPack
- **Excel Parsing:** ExcelDataReader
- **Dependency Injection:** .NET Core built-in DI

---

## 💼 Medicine Data Handling

### **Why We Need to Handle Medicine Data Manually**
- There is no **medicine lookup service** provided by **Sağlık Bakanlığı**.
- The official **medicine list** is published **weekly** at [TİTCK](https://www.titck.gov.tr/dinamikmodul/43).
- To ensure **searchability**, we implemented a **REST API** that allows querying medicine names.
- Example: Searching for medicines containing `ASP` should return:
  ```json
  {
    "medicationNames":"ASPIRIN", "CASPOBIEM", "CASPOPOL", "CORASPIN", "SIGMASPORIN", "VASPARIN" ...
  }
  ```
- A **web scraping service** downloads the latest medicine file, parses it, and uploads it to MongoDB.

---

## 🏥 User Authentication & Testing Credentials

This system uses **JWT authentication**. The following **test users** can be used to access different roles:

### **Doctor Account**
```
Username: doctor1 Password: pass Role: doctor Name: Şükrü
```

### **Pharmacy Account**
```
Username: phar3 Password: pass Role: pharmacy Name: p3 Email: nomail2@hotmail.com
```

Use these credentials to test login functionality and role-based access control.

---

## 📂 Project Architecture

This system is structured as a **multi-layered application**, with clear separation between:

1. **Frontend** – HTML, CSS, JavaScript UI for doctors and pharmacies.
2. **Controllers** – Handles HTTP requests.
3. **Services** – Implements business logic.
4. **Data Access Layer (DAL)** – Handles database interactions.
5. **Models** – Defines the structure of data entities.
6. **Contexts** – Manages database connections.
7. **Message Queue (RabbitMQ via CloudAMQP)** – Asynchronous communication.
8. **Caching Layer (Azure Redis)** – Caches frequently used medicines.
9. **Web Scraping & Data Parsing** – Downloads and processes medicine data.
10. **API Gateway (Ocelot)** – Handles request routing and load balancing.
11. **Azure Logic Apps** – Automates scheduled tasks.

---

## 📊 ER Diagram

Below is the **Entity-Relationship Diagram (ERD)** for the **Prescription System**, showing the relationships between **Prescriptions** and **Medicines**.

<p align="center">
  <img src="https://github.com/user-attachments/assets/08f55ddb-75d0-4c44-b24a-5bbe0fdd46b0" width="600">
</p>

---

## 📸 Screenshots of the frontend

| Login Page | Doctor Dashboard | Pharmacy Dashboard |
|------------|----------------|----------------|
| ![Login](https://github.com/user-attachments/assets/0979ea52-6153-47a8-8d26-ec007e7633be) | ![Doctor](https://github.com/user-attachments/assets/ab19390a-794c-4c7c-b081-742fdb5e4969) | ![Pharmacy](https://github.com/user-attachments/assets/bccb89e3-2666-45ba-af92-fab7aa71ad5c) |





---

## 🤝 Contributing

We welcome contributions! To contribute:

1. **Fork** the repository.
2. **Create a branch** for your feature (`git checkout -b feature-name`).
3. **Commit changes** (`git commit -m 'Added new feature'`).
4. **Push to GitHub** (`git push origin feature-name`).
5. **Submit a Pull Request**.

---

## 📝 License

This project is licensed under the **MIT License**.

---

🚀 **THANK YOU FOR READING** 🚀

