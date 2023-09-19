Welcome to my project! Before diving in, here are some key things you should know:

Platform Compatibility: This project is optimized for Linux systems. If you intend to run it on a different platform, ensure you swap out the MSSQL Linux Image to match your system. You can check docker-compose.yml file for more detail.

Architecture Design: My design utilizes the Onion Architecture, providing a robust and modular approach to development:

Core Layer: This contains foundational components for every other layer, including certain database fundamentals.

Infrastructure Layer: This layer is dedicated to necessary database operations and other service requirements. Think of it as the persistence layer for the database.

Presentation Layer: This is the main layer where the project gets up and running. You'll find Controllers, DTOs, ViewModels, Views, and all the essential components here.
Pricing Controller Note: At the moment, the Pricing Controller is inactive. It's reserved for future implementations, especially when payment methods are integrated into the project.

Payment Method: Currently, the project does not support automatic payment methods. However, for any order received, a manual approval process is in place for payments.

Please ensure you go through the README to understand the project structure and its features. If you have any questions or concerns, don't hesitate to reach out.
