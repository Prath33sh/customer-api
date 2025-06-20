# customer-api

This repo provides a basic CRUD API interface for Customer entity. The supported operations are creation(POST) of the customer entity, retrieval(GET) of the entity by Id, updating(PUT) the whole entity and deletion of the entity by id. Partial updates (PATCH) and retrieval of multiple entities(paginated resuts) is not supported at this time.

## How to run the project

The project is enabled with Swagger UI that can be used for viewing/testing the endpoints. The database used in development was PostgreSQL.
The repo contains docker-compose scripts to launch the docker images.
Run 
```
docker-compose up
```
This will launch the Postgres DB instance and also startup the application in port 8080.
Access the swagger UI at: http://localhost:8080/swagger/index.html

Additionally, a console based test client application is also provided as project in the solution. Build customer-api-client.csproj and launch the app. The console shows an interactive screen for available options.

## Running tests

The tests for this project include unit tests and integration tests (using an in-memory db).
You can run tests using the following:

```
docker-compose run --rm tests
```
This can be integrated into the CI/CD automation by building, testing and deploying the images via GHA/pipeline after pushing the image to a registry.

## Observability

For now this app relies on logging for most insights. A healthcheck endpoint is also provided.
The healthcheck endpoint is '/health'.

## CI/CD

A sample pipeline and k8s yaml has been added to the project and can be further configured if needed.

## Assumptions:

1. As per the requirement the Id field is a primary key in the entity and is UUID type. While it is typical to use a long value for DB Id and a separate UUID based user identifier for API and public exposure I am following what was stated in the initial requirement. 

2. For the initial version PATCH is not added for simplicity.

3. Also, the initial version supports get by Id only.

4. For DELETE operation a soft delete is used as done in many real applications.

5. Included, created and updated fields were added in addition to keep track of the time of action/auditing.

6. Auth, RBAC etc are not in scope for this project at this time.