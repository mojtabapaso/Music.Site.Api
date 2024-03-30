# Project Name

Music.API

## Description
Clean Architecture
This project is an API for a music site developed using .NET 8. It allows users to authenticate using JSON Web Tokens (JWT) and provides features for regular and subscribed users. The API allows searching for music based on singer name, genre, and music type.

## Features

- User Authentication: Users are authenticated using JWT. Upon successful authentication, a token is generated and included in the Authorization header for subsequent API requests.
- Access Control: Different APIs have different access levels based on users' subscription status. Users with active subscriptions have access to premium APIs, while users without a subscription can only access basic APIs.
- Regular Users: Regular users have access to basic functionalities of the music site, such as searching for music and viewing singer information.
- Subscribed Users: Subscribed users have additional privileges, including access to premium content, personalized recommendations, and the ability to create playlists.
- Subscription Management: The system utilizes cron jobs to periodically check the subscription status of users. If a user's subscription expires, their access to premium APIs is revoked until they renew their subscription.
- API Documentation: The project includes comprehensive documentation using Swagger/OpenAPI. The documentation outlines the available endpoints, request/response structures, and authentication requirements.

## Technologies Used

- .NET 8: The project is built using the latest version of .NET, providing access to modern features and performance improvements.
- Entity Framework Core: EF Core is used as the ORM (Object-Relational Mapping) tool for database operations, providing a simple and efficient way to interact with the underlying data store.
- SQLServer: The project utilizes SQLServer as the database management system to store user information, subscription details, and other relevant data.
- JWT: JSON Web Tokens are used for user authentication and authorization. Tokens are securely generated and validated to ensure the integrity and security of the API.
- Swagger/OpenAPI: Swagger is integrated into the project to automatically generate API documentation. It provides an interactive UI to explore and test the available endpoints.
- Cron Jobs: Cron jobs are employed to periodically check and update the subscription status of users. This ensures that users' access to premium APIs is dynamically managed based on their subscription validity.
