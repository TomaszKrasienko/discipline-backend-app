<div align="center">
    <img src="assets/discipline_logo.png" width="500">
</div>

<div align="center">
    Discipline - Backend Application
</div>

### Purpose
Discipline was created to enable users to monitor their daily tasks and support the formation of positive habits. With flexible activity rules, users can tailor schedules to their needs and effectively manage daily productivity.

### Implemented Features
- **Activities:**
    * Allowing users to add daily tasks,
    * Marking Activities as Completed,
    * Deleting Activities
- **Activity Rules:**
    * Users can define various activity rule modes, such as: Daily, First day of the week, Last day of the week, First day of the month,
  Last day of the month, Custom - select specific days of the week
    * Editing Activity Rules: Allows users to modify existing activity rules.
    * Deleting Activity Rules: Users can remove activity rules.
    * Generating activities from activity rules by themselfs
- **Automatic Activity Generation:** The application automatically generates activities based on the defined rules for a given day.

### Features to be Implemented
- **User Adaptation**: Modify the application to support multiple users with personalized settings.

---
### Architecture

- **Feature-Based Organization:** Each feature (or slice) of the application is encapsulated within its own module. This means that all relevant files—such as models, commands, validators, and controllers—are grouped together based on the feature they support.

- **Domain Models:** The application employs domain models where domain rules are validated. This ensures that business logic is centralized within the domain layer, promoting a clean and maintainable codebase.

- **Database:** Discipline uses MongoDB, a NoSQL database, which provides flexibility in handling diverse data types and structures. MongoDB's document-oriented storage is well-suited for the dynamic and evolving data models typical in modern applications.

- **CQRS (Command Query Responsibility Segregation):** This pattern separates the read and write operations of the application, enhancing scalability and performance:

- **Minimal API:** Each feature slice implements a minimal API to handle its specific functions. This keeps the API endpoints lean and focused, making it easier to manage and scale individual features.
