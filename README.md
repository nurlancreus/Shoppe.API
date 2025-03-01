# Shoppe

Shoppe is a full-stack e-commerce platform designed for a boujiteria shop. The backend is built with **ASP.NET Core** using **Onion Architecture**, ensuring scalable and maintainable APIs for managing inventory, orders, authentication, and a blog service with reactions and replies. The project also integrates **Stripe and PayPal payment gateways** with **webhook support** for real-time payment processing and notifications.

## Project Structure

The solution follows an onion architecture and consists of multiple projects:

```
│-- src
│   │-- Core
│   │   │-- Shoppe.Application
│   │   │-- Shoppe.Domain
│   │-- Infrastructure
│   │   │-- Shoppe.Infrastructure
│   │   │-- Shoppe.Persistence
│   │   │-- Shoppe.SignalR
│   │-- Presentation
│   │   │-- Shoppe.API
```

### Core
- **Shoppe.Application**: Contains business logic and service layer.
- **Shoppe.Domain**: Contains entity models and domain logic.

### Infrastructure
- **Shoppe.Infrastructure**: Provides external dependencies like email services, payment integrations, and caching.
- **Shoppe.Persistence**: Handles database access and migrations.
- **Shoppe.SignalR**: Manages real-time communication using SignalR.

### Presentation
- **Shoppe.API**: The main entry point for API requests, handling authentication, order processing, and other core functionalities.

## Features

- User authentication and authorization with JWT
- Product and inventory management
- Order processing and checkout system
- Payment gateway integrations with **Stripe** and **PayPal**
- Webhook support for real-time payment updates
- Blog service with post reactions and replies
- Caching for optimized API responses
- Secure email notifications

## Configuration

To run the project, clients must provide valid values for the necessary configurations in the `appsettings.json` file.

### Required Configuration

#### Database
```json
"ConnectionStrings": {
  "Default": "Server=.;Database=ShoppeDb;Integrated Security=true;TrustServerCertificate=true;"
}
```

#### Token Settings
```json
"Token": {
  "Access": {
    "Audience": "https://localhost:3000/",
    "Issuer": "https://localhost:7223/",
    "SecurityKey": "<your-security-key>",
    "AccessTokenLifeTimeInMinutes": 10
  },
  "Refresh": {
    "RefreshTokenLifeTimeInMinutes": 60
  }
}
```

#### Storage (AWS S3)
```json
"Storage": {
  "AWS": {
    "AccessKey": "<your-access-key>",
    "SecretAccessKey": "<your-secret-key>",
    "Region": "eu-north-1",
    "AWSS3": {
      "BucketName": "<your-bucket-name>"
    }
  }
}
```

#### Email Configuration
```json
"EmailConfiguration": {
  "From": "<your-email>",
  "SmtpServer": "smtp.gmail.com",
  "Port": 587,
  "Username": "<your-email>",
  "Password": "<your-app-password>"
}
```

#### Payment Gateways

**Stripe:**
```json
"Payment": {
  "Stripe": {
    "PublishableKey": "<your-publishable-key>",
    "SecretKey": "<your-secret-key>",
    "WebhookSecret": "<your-webhook-secret>"
  }
}
```

**PayPal:**
```json
"Payment": {
  "PayPal": {
    "ClientId": "<your-client-id>",
    "ClientSecret": "<your-client-secret>",
    "Mode": "Sandbox",
    "WebhookId": "<your-webhook-id>"
  }
}
```

### API Integrations
```json
"API": {
  "CountryAPI": {
    "BaseUrl": "https://restcountries.com/",
    "Version": 3.1,
    "ApiKey": null,
    "ApiSecret": null
  },
  "AmadeusAPI": {
    "BaseUrl": "https://test.api.amadeus.com/",
    "Version": 1,
    "ApiKey": "<your-api-key>",
    "ApiSecret": "<your-api-secret>"
  }
}
```

## Contribution Guidelines

- **Do not add new features** unless explicitly requested.
- Follow the **Onion Architecture** structure and respect domain separation.
- Ensure all contributions include.
- Use **meaningful commit messages** and follow the repository's coding standards.
- Secure sensitive information (e.g., API keys, database credentials) and do not commit them.

## Running the Project

### Prerequisites
- .NET 8 or later
- SQL Server or an alternative database supported by Entity Framework Core

1. Clone the repository.
2. Provide valid values in `appsettings.json`.
3. Apply database 'init' migration:
   ```sh
   dotnet ef database update
   ```
4. Run the application:
   ```sh
   dotnet run --project src/Presentation/Shoppe.API
   ```

### Super Admin Credentials

To use the API, log in with the following super admin credentials:

- **Email:** superadmin@example.com
- **Password:** gGjrfmdb8wp13658$%

## License
This project is licensed under the MIT License.

