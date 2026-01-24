# How-To: Run SonarQube locally with VS Code Dev Containers (WSL 2)

This guide walks through the **minimum, working setup** to run **SonarQube Community Edition locally** using:

* Windows
* WSL 2
* Docker Desktop
* VS Code Dev Containers
* A .NET Dev Container

The end state is successfully loading:

```
http://localhost:9000
```

---

## Prerequisites

* Windows with **WSL 2 enabled**
* **Docker Desktop** using the WSL 2 backend
* **VS Code** with:

  * Dev Containers extension
* A repo opened **inside a Dev Container**

---

## 1. Configure WSL 2 memory (required for stability)

Create or edit this file on **Windows** (not inside WSL):

```
C:\Users\<your-user>\.wslconfig
```

```ini
[wsl2]
memory=8GB
processors=6
swap=8GB
localhostForwarding=true
```

Apply it:

```powershell
wsl --shutdown
```

Then restart:

* Docker Desktop
* VS Code

---

## 2. Use a Dev Container that can talk to Docker Desktop

### Key principles

* **Do NOT use Docker-in-Docker**
* Use the **host Docker daemon** via `/var/run/docker.sock`
* Install a **modern Docker CLI** (not Debian’s `docker.io`)

---

## 3. Minimal, working `devcontainer.json`

```jsonc
{
    "name": "qik-rest-api",
    "image": "mcr.microsoft.com/devcontainers/dotnet:1-9.0-bookworm",

    "remoteUser": "root",

    "mounts": [
        "type=bind,source=/var/run/docker.sock,target=/var/run/docker.sock"
    ],

    // Use 'postCreateCommand' to run commands after the container is created.
    "postCreateCommand": [
        "/bin/sh",
        "-c",
        "rm -f /etc/apt/sources.list.d/yarn.list && apt-get update && apt-get install -y ca-certificates curl gnupg && install -m 0755 -d /etc/apt/keyrings && curl -fsSL https://download.docker.com/linux/debian/gpg | gpg --dearmor -o /etc/apt/keyrings/docker.gpg && chmod a+r /etc/apt/keyrings/docker.gpg && echo \"deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/debian bookworm stable\" > /etc/apt/sources.list.d/docker.list && apt-get update && apt-get install -y docker-ce-cli docker-compose-plugin"
    ],
}
```

### Why this works

* Avoids broken Dev Container Docker features on WSL
* Avoids outdated Debian Docker clients
* Ensures Docker CLI ↔ Docker Desktop API compatibility
* Avoids permission and group-ID issues by running as `root`

---

## 4. Rebuild the Dev Container

In VS Code:

```
Dev Containers: Rebuild Container (No Cache)
```

Verify Docker works **inside the container**:

```bash
docker version
docker ps
```

---

## 5. Create a minimal SonarQube `docker-compose.yml`

At the repo root:

```yaml
version: "3.8"

services:
  sonarqube:
    image: sonarqube:community
    ports:
      - "9000:9000"
    environment:
      SONAR_ES_BOOTSTRAP_CHECKS_DISABLE: "true"
    volumes:
      - sonarqube_data:/opt/sonarqube/data
      - sonarqube_extensions:/opt/sonarqube/extensions
      - sonarqube_logs:/opt/sonarqube/logs

volumes:
  sonarqube_data:
  sonarqube_extensions:
  sonarqube_logs:
```

---

## 6. Start SonarQube manually

From the Dev Container terminal:

```bash
docker compose up -d
```

Wait ~30–60 seconds on first startup.

---

## 7. Open SonarQube

In your browser:

```
http://localhost:9000
```

If the welcome page loads, the setup is complete ✅

---

## Persistence guarantees

This setup **persists across**:

* Dev Container restarts
* VS Code restarts
* Docker Desktop restarts

As long as you **do not delete Docker volumes**, SonarQube data is retained.

---

## Summary checklist

| Item                    | Status |
| ----------------------- | ------ |
| WSL 2 memory configured | ✅      |
| Dev Container stable    | ✅      |
| Docker CLI compatible   | ✅      |
| Docker socket mounted   | ✅      |
| SonarQube running       | ✅      |
| Welcome page loads      | ✅      |


# Addendum: Running Analysis with `dotnet sonarscanner`

This section documents the **exact commands required** to run a full SonarQube analysis for a .NET project against a **local SonarQube server**.

It applies to:

* SonarQube Community Edition
* Local Docker-hosted SonarQube (`http://localhost:9000`)
* .NET SDK projects
* VS Code Dev Containers

---

## 1. Prerequisites (one-time per Dev Container)

### Install the SonarScanner for .NET

Inside the Dev Container terminal:

```bash
dotnet tool install --global dotnet-sonarscanner
```

Ensure the tool directory is on your `PATH` (usually already true in Dev Containers):

```bash
export PATH="$PATH:/root/.dotnet/tools"
```

Verify installation:

```bash
dotnet sonarscanner --version
```

---

## 2. Required inputs

Before running analysis, you must have:

| Value               | Description               |
| ------------------- | ------------------------- |
| `SONAR_HOST_URL`    | URL of SonarQube server   |
| `SONAR_PROJECT_KEY` | Stable project identifier |
| `SONAR_TOKEN`       | Authentication token      |

### Recommended environment variables

```bash
export SONAR_HOST_URL=http://localhost:9000
export SONAR_PROJECT_KEY=qik-rest-api
export SONAR_TOKEN=xxxxxxxxxxxxxxxxxxxx
```

(These can also come from a `.env` file or shell profile.)

---

## 3. The analysis sequence (important)

**SonarScanner for .NET always runs in three steps:**

1. `begin` – prepares analysis and hooks into MSBuild
2. `build` – compiles the solution and gathers metrics
3. `end` – sends results to SonarQube

You **must** run all three in the same shell session.

---

## 4. Exact commands (canonical)

### Step 1️⃣ — Begin analysis

Run this from the **solution root**:

```bash
dotnet sonarscanner begin \
  /k:"$SONAR_PROJECT_KEY" \
  /d:sonar.host.url="$SONAR_HOST_URL" \
  /d:sonar.login="$SONAR_TOKEN"
```

### What this does

* Registers the project key
* Authenticates with SonarQube
* Injects analyzers into the build

No code is analyzed yet.

---

### Step 2️⃣ — Build the solution

Use **your normal build command**:

```bash
dotnet build
```

or, if you have multiple solutions:

```bash
dotnet build path/to/YourSolution.sln
```

### Why this matters

* SonarScanner for .NET analyzes **compiled code**
* Skipping this step = no analysis data

---

### Step 3️⃣ — End analysis

```bash
dotnet sonarscanner end \
  /d:sonar.login="$SONAR_TOKEN"
```

### What this does

* Packages all collected metrics
* Sends them to SonarQube
* Finalizes the analysis run

At this point, the scan is complete.

---

## 5. Verify the results

Open:

```
http://localhost:9000
```

Navigate to your project and confirm:

* Latest analysis timestamp updated
* Issues, code smells, and metrics visible
* Quality Gate status computed

---

## 6. Common optional parameters (use only if needed)

### Specify source encoding

```bash
/d:sonar.sourceEncoding=UTF-8
```

### Exclude generated files

```bash
/d:sonar.exclusions=**/bin/**,**/obj/**
```

### Set project version

```bash
/d:sonar.projectVersion=1.0.0
```

These are passed to `begin`, for example:

```bash
dotnet sonarscanner begin \
  /k:"$SONAR_PROJECT_KEY" \
  /d:sonar.host.url="$SONAR_HOST_URL" \
  /d:sonar.login="$SONAR_TOKEN" \
  /d:sonar.exclusions=**/bin/**,**/obj/**
```

---

## 7. Recommended helper script (optional but clean)

To avoid retyping commands, create:

```
scripts/sonar-scan.sh
```

```bash
#!/usr/bin/env bash
set -e

dotnet sonarscanner begin \
  /k:"$SONAR_PROJECT_KEY" \
  /d:sonar.host.url="$SONAR_HOST_URL" \
  /d:sonar.login="$SONAR_TOKEN"

dotnet build

dotnet sonarscanner end \
  /d:sonar.login="$SONAR_TOKEN"
```

Make it executable:

```bash
chmod +x scripts/sonar-scan.sh
```

Run with:

```bash
./scripts/sonar-scan.sh
```

---

## 8. Relationship to SonarLint (important clarification)

* **SonarLint**: instant, local feedback in the editor
* **SonarScanner**: authoritative analysis pushed to the server

Even when SonarLint is connected:

* You **still** need to run the scanner
* SonarLint does **not** publish results

They are complementary, not redundant.

---

## TL;DR (drop-in summary)

```bash
dotnet sonarscanner begin /k:"<project-key>" /d:sonar.host.url=http://localhost:9000 /d:sonar.login=<token>
dotnet build
dotnet sonarscanner end /d:sonar.login=<token>
```

Run all three, in order, from the same shell.
