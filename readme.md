# Backend Project Setup

This document outlines the steps required to set up the project and run the database using Docker. Follow the instructions below to get started.

## Table of Contents

- [Prerequisites](#-prerequisites)
- [Installation](#installation)
- [Running the Application](#running-the-application)
- [Testing](#testing)

## 🔧 Prerequisites

Before you begin, ensure you have the following installed on your machine:

- 🐳 [Docker](https://www.docker.com/get-started) (latest version)
- [Docker Compose](https://docs.docker.com/compose/install/) (if not included with Docker)
- [.NET SDK](https://dotnet.microsoft.com/download) (required for building the application)

## Installation

1. **Clone the Repository**
2. **Open the Solution**

Navigate to the PadelSeasonPlanner solution directory

```bash
cd backend
```

3. **Navigate to the Padel.Application Directory**

```bash
cd Padel.Application
```

4. **Start the PostgreSQL Database**

Use Docker Compose to start the PostgreSQL database in a Docker container:

```bash
docker compose up
```

You can check the Docker app to verify that the PostgreSQL database container is running.

## Running the Application

Once the PostgreSQL database is running, you can build and run the Padel.Api to establish a connection to the database:

1. **Navigate to the Padel.API Directory**

Change back to the main directory and then into Padel.API:

```bash
cd ../Padel.API
```

2. **Run the Api**

Enable HTTPS on the Padel.Api project and run it. This will build the application, which is necessary for running tests.

- In your IDE, look for the option to run the project with HTTPS (often a button or a context menu option).

## Testing

1. **Navigate to the Padel.Tests Directory**
2. **Run the Tests**
   one time build of Padel.Api is needed for the tests to run.
