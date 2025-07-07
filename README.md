# IssueManager

**IssueManager** is a .NET 9 Web API that enables creating, updating, and closing issues on supported Git hosting platforms: **GitHub** and **GitLab**. It provides a unified interface to manage issues without needing direct interaction with the specific platform APIs.

---

## 📦 Project Structure

```
IssueManager/
├── IssueManager.API/       # Web API layer (controllers, middleware, config)
├── IssueManager.Core/      # Core logic (interfaces, models, services)
└── IssueManager.Tests/     # Unit tests for services and validators
```

---

## 🚀 How to Run

1. **Clone the repository:**

   ```bash
   git clone https://github.com/kinan11/IssueManager
   cd IssueManager
   ```

2. **Run the API:**

   ```bash
   dotnet run --launch-profile "https" --project IssueManager.API
   ```

   The API will be available at `https://localhost:7166` and `http://localhost:5208`.

---

## 🧪 Run Tests

Run unit tests using the following command:

```bash
dotnet test IssueManager.Tests
```

---

## 🛠️ Endpoints

### ➕ Add Issue

**POST** `/issues/addIssue`

Creates a new issue on GitHub or GitLab.

#### Request Body:

```json
{
  "title": "Sample issue title",
  "description": "Detailed description of the issue.",
  "repo": {
    "provider": "GitHub",
    "owner": "username_or_org",
    "name": "repository-name"
  }
}
```

#### Example:

```bash
curl -X POST https://localhost:7166/issues/addIssue \
  -H "Content-Type: application/json" \
  -d '{"title":"Bug","description":"Found a bug","repo":{"provider":"GitHub","owner":"myuser","name":"myrepo"}}'
```

---

### 🔒 Close Issue

**PATCH** `/issues/closeIssue/{issueNumber}`

Closes an existing issue on the specified platform.

#### Request Body:

```json
{
  "provider": "GitLab",
  "owner": "mygroup",
  "name": "project-name"
}
```

#### Example:

```bash
curl -X PATCH https://localhost:7166/issues/closeIssue/12 \
  -H "Content-Type: application/json" \
  -d '{"provider":"GitLab","owner":"mygroup","name":"project-name"}'
```

---

### ✏️ Update Issue

**PATCH** `/issues/updateIssue/{issueNumber}`

Updates the title or description of an existing issue.

#### Request Body:

```json
{
  "title": "Updated issue title",
  "description": "Updated description",
  "repo": {
    "provider": "GitHub",
    "owner": "myuser",
    "name": "myrepo"
  }
}
```

#### Example:

```bash
curl -X PATCH https://localhost:7166/issues/updateIssue/7 \
  -H "Content-Type: application/json" \
  -d '{"title":"Updated","description":"Fix applied","repo":{"provider":"GitHub","owner":"myuser","name":"myrepo"}}'
```

---

## 🧹 Technologies

- **.NET 9**
- **ASP.NET Core Web API**
- **xUnit** for unit testing

---

## 📝 Notes

- The application does **not** require authentication.
- All logic for service resolution (GitHub/GitLab) is abstracted through a factory pattern.
- Issue validation is handled via custom validators in the `Core.Models.Validators` namespace.

---

## 📢 Contact

For questions or feedback, please open an issue or contact the maintainer.
