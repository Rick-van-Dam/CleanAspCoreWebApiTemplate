# A productive ASP .NET minimal api template

This is a template repository showing how one can implement a clean api with ASP.NET using minimal apis. The focus on 'features' in this template is on dev productivity, the actual features
of the api itself has been kept basic on purpose. Feel free to copy this repository or reuse parts of it, don't forget to give a star if you do.

Some features in this template:

- [Vertical Slice architecture](https://www.jimmybogard.com/vertical-slice-architecture/) (grouping based on features instead of technical layers)
- An easy to use and fast to run integration tests setup that runs in seconds with the only dependency being docker with the use
  of [TestExamplesDotnet](https://github.com/Rick-van-Dam/TestExamplesDotnet.git)
- Authentication and authorization using jwt tokens. This is also used by the tests which means you can check if your authentication and authorization is working properly inside the tests.
- Ready for [OpenTelemetry](https://opentelemetry.io/)
- Launchprofile for [dotnet watch](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-watch)
- Automated build using github actions that checks code formatting, builds and runs the tests.
- .NET Aspire

## Running tests

Only docker is required to run the tests (Tip: podman is free to use). First run can take a bit longer as the docker image is downloaded.
Run the following command to run the tests:

```cmd
dotnet test
```

## Running the app

1. First generate a jwt that you can use for local testing by running the following command in the `CleanAspCore.Api` directory:

```cmd
dotnet user-jwts create --role "read" --role "write"
```

NOTE: The jobs and department endpoints only require authentication but the employee endpoints require that you have the correct claims in the jwt token.

2. Run the `CleanAspCore.AppHost` project. You can do this by running the following command in the `CleanAspCore.AppHost` directory:

```cmd
dotnet run
```

3. The url of the Aspire dashboard will be printed and from there you can navigate easily to the other services.
