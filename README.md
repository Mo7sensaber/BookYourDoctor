# 🏥 BookYourDoctor API

BookYourDoctor is a **RESTful Web API** built with **ASP.NET Core** that enables patients to book appointments with doctors, manage schedules, and complete secure online payments using **Paymob**.

The project is designed and implemented following **Clean Architecture** and **Clean Code principles**, making it scalable, maintainable, and easy to understand for both developers and technical reviewers.

---

## 🚀 Project Overview

The system handles the complete medical booking lifecycle:

* User registration & authentication (Doctor / Patient)
* Specialty management
* Doctor profile & schedule (slots) management
* Appointment booking, updating, and cancellation
* Secure online payment using **Paymob**
* Automatic payment confirmation via **Paymob Webhook**

---

## 🧱 Architecture

This project follows **Clean Architecture**, ensuring a clear separation of concerns:


### Why Clean Architecture?

* High testability
* Loose coupling
* Easy maintenance & scalability
* Clear business logic separation

---

## 🔐 Authentication & Authorization

* Implemented using **ASP.NET Identity**
* JWT-based authentication
* Users register and login without initially choosing a role
* Role behavior (Doctor / Patient) is determined by user actions

---

## 🧠 System Scenario (Business Flow)

### 1️⃣ User Registration & Login

* Any user can register using `/api/Authentication/register`
* User logs in and receives a **JWT Token**

---

### 2️⃣ Specialty Management

* Admin or authorized user can:

  * Add specialty
  * Update specialty
  * Delete specialty
  * Get specialty by ID

Endpoints are available under:

```
/api/Specialty
```

---

### 3️⃣ Doctor Flow

If a user chooses to become a **Doctor**:

* Add doctor profile
* Assign specialty
* Manage available slots:

  * Add slot
  * Edit slot
  * Delete slot
  * View slots

Doctor-related endpoints:

```
/api/Doctor
```

---

### 4️⃣ Patient Flow

A patient can:

* View doctors
* View doctor available slots
* Book an appointment
* Update appointment status
* Cancel appointment
* View all appointments

Appointment endpoints:

```
/api/Appointment
```

---

### 5️⃣ Payment Flow (Paymob Integration)

After booking an appointment:

1. Patient requests payment link using appointment ID
2. API communicates with **Paymob**
3. Paymob returns a secure payment URL
4. Patient completes payment
5. Paymob sends a **Webhook Callback**
6. System verifies transaction and updates payment status

Payment endpoints:

```
/api/Payment/GetPaymentLink/{appointmentId}
/api/PaymobWebhook/callback
```

✔️ Payment status is saved automatically after successful verification

---

## 🔌 API Modules & Endpoints

> 📌 **Note**: All endpoint contracts, routes, and request/response DTOs are defined inside a **Shared Library**.
> This ensures reusability, consistency, and clear separation between API layer and business logic.

The API layer only consumes these shared definitions.

### 🔑 Authentication

* POST `/api/Authentication/register`
* POST `/api/Authentication/login`
* GET  `/api/Authentication/emailexist`

### 🩺 Doctor

* GET    `/api/Doctor/GetAllDoctors`
* POST   `/api/Doctor/AddDoctor`
* GET    `/api/Doctor/GetDoctorByID`
* POST   `/api/Doctor/EditDoctor`
* DELETE `/api/Doctor/DeleteDoctor`
* POST   `/api/Doctor/AddDoctorSlot`
* POST   `/api/Doctor/EditDoctorSlot`
* DELETE `/api/Doctor/DeleteDoctorSlot`
* GET    `/api/Doctor/GetDoctorSlots`

### 📅 Appointment

* POST   `/api/Appointment/CreateAppointment`
* GET    `/api/Appointment/GetAllAppointPerDoctor`
* GET    `/api/Appointment/GetAllAppointPerPatient`
* PUT    `/api/Appointment/ChangeStatus`
* DELETE `/api/Appointment/CancelAppointment`

### 💳 Payment

* POST `/api/Payment/GetPaymentLink/{appointmentId}`

### 🔁 Paymob Webhook

* GET `/api/PaymobWebhook/callback`

---

## 🛠️ Technologies Used

* **ASP.NET Core Web API**
* **Entity Framework Core**
* **SQL Server**
* **ASP.NET Identity**
* **JWT Authentication**
* **Paymob Payment Gateway**
* **Swagger / OpenAPI**
* **Clean Architecture**
* **Clean Code Principles**

---

## 📌 Key Highlights

✔ Real-world payment integration (Paymob)
✔ Secure webhook handling
✔ Role-independent authentication design
✔ Clean & scalable architecture
✔ Production-ready API structure

---

## 👨‍💻 Author

**Mohsen Saber**
Backend Developer | ASP.NET Core

🔗 GitHub: [https://github.com/Mo7sensaber](https://github.com/Mo7sensaber)

---

## ⭐ Final Note

This project reflects strong backend fundamentals, real-world payment integration, and professional API design principles. It is built to be easily extended, tested, and deployed in production environments.

If you find this project helpful or impressive, feel free to ⭐ the repository.
