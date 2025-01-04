
# Docker Compose Setup: Keycloak, Redis, and MongoDB

This repository contains a `docker-compose.yml` file that sets up a development environment with **Keycloak**, **Redis**, and **MongoDB**. The services are connected using a custom Docker network for seamless communication. 

These services are required for the basic URL shortner to run which can also be built as a docker container

## Prerequisites

- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)

## Services

### Keycloak
- Open Source Identity and Access Management.
- Exposed on port **8080**.
- Admin credentials:
  - Username: `admin`
  - Password: `admin`

**Note:** Authentication and authorization flows are not pre-configured in this setup. You need to manually set up realms, clients, and user roles in Keycloak to suit your application's requirements. Additionally, there is no automatic redirection to Keycloak for authentication in this project. The integration of Keycloak with your application will require custom configuration of URLs and handling token exchanges.

### Redis
- In-memory data structure store.
- Exposed on port **6379**.

### MongoDB
- NoSQL database.
- Exposed on port **27017**.
- Default credentials:
  - Username: `root`
  - Password: `password`

## Getting Started

### Clone the Repository
```bash
git clone <repository-url>
cd <repository-folder>
```

### Start the Services
Run the following command to start all services:
```bash
docker-compose up -d
```

### Access Keycloak
1. Open your browser and navigate to: [http://localhost:8080/auth](http://localhost:8080/auth).
2. Log in with the admin credentials provided above.

### Stop the Services
To stop all running containers, use:
```bash
docker-compose down
```

## Customization

### Keycloak Database
By default, Keycloak uses an embedded H2 database. For production, it is recommended to use a more robust database such as PostgreSQL or MySQL. Update the `docker-compose.yml` file accordingly.

### MongoDB Initialization
Modify the environment variables or mount a custom initialization script to `/docker-entrypoint-initdb.d/` to pre-load data into MongoDB.

## Project Structure
```
.
├── docker-compose.yml
├── README.md
```

## Useful Commands

### View Logs
```bash
docker-compose logs -f
```

### Restart a Service
```bash
docker-compose restart <service-name>
```

### Remove Containers, Networks, and Volumes
```bash
docker-compose down -v
```
