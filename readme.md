# Notes

For demonstration purposes, the API is configured to run under HTTP, not HTTPS, to avoid untrusted SSL certificate errors.

In CustomerController, there is duplication between the **response code="xx"** comments and the **ProducesResponseType** annotations. This is intentional. The former improves the Swagger page, while the latter are for Web API analyzers, which ensure that all HTTP statuses returned by an action are documented by a ProducesResponseType annotation.

The test runner in some Visual Studio 2017 installs may not discover the xUnit tests - this is a known issue.
However  the tests run fine in my VS 2017 and VS 2019 installs.
The tests can also be run from the command line by executing "dotnet test" in the CustomerApiTests folder.
