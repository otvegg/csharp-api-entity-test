using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Transactions;

namespace workshop.tests;

public class SurgeryTests
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;


    [SetUp]
    public void SetUp()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }


    [Test]
    public async Task GetEndpointStatus()
    {
        var response = await _client.GetAsync("/surgery/patients");

        Console.WriteLine(response.ToString());

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
    }



    [TestCase(1)]
    public async Task GetPatientIdStatus( int id)
    {
        var response = await _client.GetAsync($"/surgery/patients/{id}");

        Console.WriteLine(response.ToString());
        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
    }





}