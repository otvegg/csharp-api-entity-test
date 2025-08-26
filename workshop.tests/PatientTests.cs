using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Transactions;
using workshop.wwwapi.DTO;
using workshop.wwwapi.Models;

namespace workshop.tests;

public class SurgeryTests
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;
    //private TransactionScope _transaction;


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
    public async Task GetPatientsStatus()
    {
        var response = await _client.GetAsync("/surgery/patients");

        Console.WriteLine(response.ToString());

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
    }

    [Test] 
    public async Task GetPatientsCheckValidPatient()
    {

        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        var client = factory.CreateClient();


        var response = await client.GetAsync("/surgery/patients");

        Assert.That(response.StatusCode == System.Net.HttpStatusCode.OK);
        var content =  response.Content;
        //var contentStream = await content.ReadAsStreamAsync();
        //var patient = await JsonSerializer.DeserializeAsync<JsonNode>(
        //    contentStream,
        //    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        //);
        Assert.That(await content.ReadAsStringAsync(), Does.Contain("Barack Trump"));
    }

    [Test]
    public async Task GetDoctorsStatus()
    {
        var response = await _client.GetAsync("/surgery/doctors");

        Console.WriteLine(response.ToString());

        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
    }

    [Test]
    public async Task GetDoctorsCheckValidDoctor()
    {
        var response = await _client.GetAsync("/surgery/doctors");
        var content = response.Content;


        Assert.That(await content.ReadAsStringAsync(), Does.Contain("Dr. Smith"));
    }

    [Test]
    public async Task GetAppointmentsStatus()
    {
        var response = await _client.GetAsync("/surgery/appointment");

        Console.WriteLine(response.ToString());
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
    }

    [Test]
    public async Task GetAppointmentCheckValidAppointment()
    {
        var response = await _client.GetAsync("/surgery/appointment");
        var content = response.Content;

        Assert.That(await content.ReadAsStringAsync(), Does.Contain("2020-12-06T00:00:00"));
    }


    [Test]
    public async Task CreatePatientStatus()
    {
        var patient = new { FullName = "Test" };
        var json = JsonSerializer.Serialize(patient);
        var requestBody = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/surgery/patients", requestBody);
        var content = response.Content;

        
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
          
    }

    [Test]
    public async Task CreateDoctorStatus()
    {
        var doctor = new { FullName = "Dr Test" };
        var json = JsonSerializer.Serialize(doctor);
        var requestBody = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/surgery/doctors", requestBody);
        var content = response.Content;


        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
    }

    [Test]
    public async Task CreateAppointmentStatus()
    {
        AppointmentPost appointment = new AppointmentPost { patientId = 4, doctorId = 2, booking = DateTime.Parse("2024-03-06T00:00:00").ToUniversalTime() };
        var json = JsonSerializer.Serialize(appointment);
        var requestBody = new StringContent(json, Encoding.UTF8, "application/json");

        // THIS TEST FAILS IF THE PATIENT DOCTOR COMBO IS ALREADY IN THE DATABASE
        var response = await _client.PostAsync("/surgery/appointment", requestBody);


        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
    }

}