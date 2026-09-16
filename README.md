# ExpenseTracker

ExpenseTracker is a simple ASP.NET Core Web API for managing expenses.

The project uses:

- .NET 8
- ASP.NET Core
- Entity Framework Core
- PostgreSQL
- Docker
- Docker Compose

## Running the project

### Prerequisites

Make sure you have installed:

- Docker
- Docker Compose

### 1. Clone the repository

```bash
git clone https://github.com/sssofi0101/ExpenseTracker.git
cd ExpenseTracker/ExpenseTracker
```

### 2. Create the environment file

Create a `.env` file in the current directory.

You can use `.env.example` as a template:

```env
POSTGRES_PASSWORD=your_password_here
```

Replace `your_password_here` with your own PostgreSQL password.

### 3. Start the application

Run:

```bash
docker compose up --build
```

Docker Compose will:

- build the ASP.NET Core API image;
- start the PostgreSQL container;
- wait until PostgreSQL is ready;
- start the API container;
- apply EF Core migrations automatically.

The API will be available at:

```text
http://localhost:8080
```

For example:

```text
http://localhost:8080/api/expenses
```

### 4. Stop the application

Press `Ctrl+C` in the terminal or run:

```bash
docker compose down
```

The PostgreSQL data is stored in a Docker volume, so it will persist between container restarts.

To remove the containers together with the database volume:

```bash
docker compose down -v
```

> Warning: this command removes the PostgreSQL volume and all locally stored database data.

## Configuration

The PostgreSQL password is stored in the local `.env` file and passed to the containers through Docker Compose.

The `.env` file should not be committed to Git.

Example:

```env
POSTGRES_PASSWORD=your_password_here
```
