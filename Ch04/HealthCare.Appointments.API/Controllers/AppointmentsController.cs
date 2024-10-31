using AutoMapper;
using Google.Cloud.PubSub.V1;
using HealthCare.Appointments.API.Constants;
using HealthCare.Appointments.API.Dtos;
using HealthCare.Appointments.API.Models;
using HealthCare.Appointments.API.Repositories;
using HealthCare.Appointments.API.Services;
using HealthCare.SharedAssets.Messages;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Appointments.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IPubSubMessagePublisher _pubSubMessagePublisher;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IDoctorsApiRepository _doctorsRepository;
    private readonly IPatientsApiRepository _patientsRepository;
    private readonly IMapper _mapper;
    private readonly IMessagePublisher _messageBus;
    private readonly ApiEndpoints _apiEndpoints;

    public AppointmentsController(
        IPubSubMessagePublisher pubSubMessagePublisher,
        IPublishEndpoint publishEndpoint,
        IAppointmentRepository appointmentRepository,
        IDoctorsApiRepository doctorsRepository,
        IPatientsApiRepository patientsRepository,
        IMapper mapper,
        IMessagePublisher messageBus,
        ApiEndpoints apiEndpoints
    )
    {
        _pubSubMessagePublisher = pubSubMessagePublisher;
        _publishEndpoint = publishEndpoint;
        _appointmentRepository = appointmentRepository;
        _doctorsRepository = doctorsRepository;
        _patientsRepository = patientsRepository;
        _mapper = mapper;
        _messageBus = messageBus;
        _apiEndpoints = apiEndpoints;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
    {
        return await _appointmentRepository.GetAll();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AppointmentDetailsDto>> GetAppointment(string id)
    {
        var appointment = await _appointmentRepository.Get(id);

        if (appointment == null)
            return NotFound();

        var doctor = await _doctorsRepository.Get(
            _apiEndpoints.GetDocumentsApi(),
            appointment.DoctorId.ToString()
        );
        var patient = await _patientsRepository.Get(
            _apiEndpoints.GetPatientsApi(),
            appointment.PatientId.ToString()
        );
        var appointmentDto = _mapper.Map<AppointmentDetailsDto>(appointment);
        appointmentDto.Doctor = _mapper.Map<DoctorDto>(doctor);
        appointmentDto.Patient = _mapper.Map<PatientDto>(patient);
        return appointmentDto;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAppointment(string id, Appointment appointment)
    {
        if (id != appointment.Id.ToString())
        {
            return BadRequest();
        }

        try
        {
            await _appointmentRepository.Update(appointment);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await AppointmentExistsAsync(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAppointment(AppointmentDto appointmentDto)
    {
        var appointment = _mapper.Map<Appointment>(appointmentDto);

        var appointmentMessage = new AppointmentMessage()
        {
            Id = appointment.Id,
            CustomerId = appointmentDto.PatientId,
            DoctorId = appointmentDto.DoctorId,
            Date = appointmentDto.Date,
        };

        // Publish to RabbitMQ with MassTransit
        await _publishEndpoint.Publish(appointmentMessage);

        // Publish to Azure Service Bus
        await _messageBus.PublishMessage(appointmentMessage, "appointments");

        // Publish to Google Pub/Sub
        await _pubSubMessagePublisher.PublishMessage(appointmentMessage, "appointments");
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAppointment(string id)
    {
        var appointment = await _appointmentRepository.Get(id);
        if (appointment == null)
            return NotFound();

        await _appointmentRepository.Delete(id);

        return NoContent();
    }

    private async Task<bool> AppointmentExistsAsync(string id)
    {
        return await _appointmentRepository.Exists(id);
    }
}
