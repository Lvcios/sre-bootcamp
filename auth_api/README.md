# Implementation Checklist
- [x] API Code
- [x] Services Code
- [x] Unit-tests
- [x] Dockerfile
- [x] It Compiles
- [x] It runs

# Api Services
- http://localhost:8000/login Receives a valid username and password and returns a JWT.
- http://localhost:8000/protected Returns protected data with a valid token, otherwise returns unauthenticated.

# How to run
- Open a console and enter the csharp directory, next run the following commands
- docker build -t wize-lucio-flores ./
- docker run -p 8000:80 wize-lucio-flores
- Open a browser in http://localhost:8000/ it must be show a message with the text "Ok"
