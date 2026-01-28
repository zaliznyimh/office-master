# OfficeMaster - Conference Room Booking System

---

This project was developed as a coursework assignment for the **"Programowanie MVC"** subject.
It serves as a practical demonstration of ASP.NET Core MVC concepts and database management

---

### Key Features

**User Module:**
* **Room Search & Filtering:** Search capabilities by capacity, room type, date availability and amenities
* **Reservation System:** Booking requests with automatic conflict detection
* **Profile Management:** Ability to view booking history and update personal information

**Admin Module:**
* **Dashboard:** Statistics including total revenue, active bookings and pending requests
* **Room Management (CRUD):** Functionality to create, edit and delete rooms
* **Reservation Workflow:** Approval and rejection system for pending reservation requests

## Technology Stack

* **Framework:** ASP.NET Core MVC (.NET 9)
* **Database ORM:** Entity Framework Core
* **Authentication:** ASP.NET Core Identity
* **Frontend:** Razor Views, Bootstrap 5, HTML5, CSS3

## Getting Started

Follow these instructions to set up and run the project on your local machine.

### Installation

1.  **Clone the repository**
    ```bash
    git clone https://github.com/zaliznyimh/office-master.git
    cd OfficeMaster
    ```

2.  **Configure the Database**
    Open `appsettings.json` and configure the connection string:
    ```json
    "ConnectionStrings" : {
      "DefaultConnection": "Host=localhost;Port=5432;Database=office-master-db;Username=postgres;Password=your-password:)"
    }
    ```

3.  **Apply Migrations**
    Initialize the database schema using Entity Framework Core tools:
    ```bash
    dotnet ef database update
    ```

4.  **Run the Application**
    ```bash
    dotnet run
    ```

### Data Seeding

At the first launch, the application checks if the database is empty. The `DataSeeder` service will automatically populate the database with:


* **Admin Credentials:**
  **Email:** `admin@officemaster.com`
  **Password:** `Admin123!`
* Sample Conference Rooms

## Application Structure

* **Controllers:** Handle incoming HTTP requests and coordinate responses
* **Services:** Contain business logic
* **ViewModels:** Objects for passing data between Views and Controllers
* **Models:** Entity definitions representing the database schema
* **Views:** Razor templates for the interfaces
