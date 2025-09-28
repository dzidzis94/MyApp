# Database Structure for MyApp.Web

This document provides an overview of the database tables, columns, and relationships for the MyApp.Web project.

---

## Users Table

| Column       | Type        | Description                  |
|--------------|------------|------------------------------|
| Id           | int        | Primary key                  |
| UserName     | string     | User's login name            |
| Email        | string     | User email                   |
| PasswordHash | string     | Hashed password              |
| Role         | string     | Role: Admin or Client        |
| CreatedAt    | DateTime   | Record creation timestamp    |

---

## Admins Table

| Column    | Type    | Description          |
|-----------|--------|--------------------|
| Id        | int    | Primary key         |
| UserId    | int    | FK to Users table   |

---

## Clients Table

| Column    | Type    | Description          |
|-----------|--------|--------------------|
| Id        | int    | Primary key         |
| UserId    | int    | FK to Users table   |

---

## Projects Table

| Column        | Type        | Description                  |
|---------------|------------|------------------------------|
| Id            | int        | Primary key                  |
| Title         | string     | Project title                |
| Description   | string     | Optional project description |
| CreatedById   | int        | FK to Admins table           |
| CreatedAt     | DateTime   | Record creation timestamp    |

---

## ProjectSubSections Table

| Column      | Type      | Description                          |
|-------------|-----------|--------------------------------------|
| Id          | int       | Primary key                           |
| ProjectId   | int       | FK to Projects table                  |
| Title       | string    | Subsection title                      |
| Description | string    | Optional description                  |

---

## ProjectSubSectionSubmissions Table

| Column             | Type      | Description                              |
|--------------------|-----------|------------------------------------------|
| Id                 | int       | Primary key                               |
| SubSectionId       | int       | FK to ProjectSubSections table            |
| ClientId           | int       | FK to Clients table                       |
| Content            | string    | Submitted content by the client          |
| SubmittedAt        | DateTime  | Timestamp of submission                   |

---

## RecentActivity Table

| Column       | Type      | Description                                    |
|--------------|-----------|-----------------------------------------------|
| Id           | int       | Primary key                                   |
| UserId       | int       | FK to Users table                             |
| Action       | string    | Description of activity                       |
| RelatedId    | int?      | Optional FK to related entity (Project/SubSection) |
| CreatedAt    | DateTime  | Timestamp of activity                          |

---

## Relationships Overview

- Users → Admins (1:1)
- Users → Clients (1:1)
- Admins → Projects (1:N)
- Projects → ProjectSubSections (1:N)
- ProjectSubSections → ProjectSubSectionSubmissions (1:N)
- Clients → ProjectSubSectionSubmissions (1:N)
- Users → RecentActivity (1:N)

