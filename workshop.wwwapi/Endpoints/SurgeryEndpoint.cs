using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Reflection;
using workshop.wwwapi.DTO;
using workshop.wwwapi.Models;
using workshop.wwwapi.Repository;


namespace workshop.wwwapi.Endpoints
{
    public static class SurgeryEndpoint
    {
        //TODO:  add additional endpoints in here according to the requirements in the README.md 
        public static void ConfigurePatientEndpoint(this WebApplication app)
        {
            var surgeryGroup = app.MapGroup("surgery");

            surgeryGroup.MapGet("/patients", GetPatients);
            surgeryGroup.MapGet("/patients/{id}", GetPatient);
            surgeryGroup.MapPost("/patients", CreatePatient);
            surgeryGroup.MapGet("/doctors", GetDoctors);
            surgeryGroup.MapGet("/doctors/{id}", GetDoctor);
            surgeryGroup.MapPost("/doctors/", CreateDoctor);
            surgeryGroup.MapGet("/appointment", GetAppointments);
            surgeryGroup.MapGet("/appointment/{patientId}/{doctorId}", GetAppointment);
            surgeryGroup.MapGet("/appointmentsbydoctor/{id}", GetAppointmentsByDoctor);
            surgeryGroup.MapGet("/appointmentsbypatient/{id}", GetAppointmentsByPatient);
            surgeryGroup.MapPost("/appointment", CreateAppointment);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetPatients(IRepository<Patient> repository)
        {
            List<PatientGet> patients = new List<PatientGet>();
            foreach (var patient in await repository.GetWithIncludes(q => q.Include(p => p.Appointments).ThenInclude(a => a.Doctor)))
            {
                List<AppointmentDoctor> appointments = new List<AppointmentDoctor>();
                foreach (var appointment in patient.Appointments)
                {
                    appointments.Add(new AppointmentDoctor { DoctorId = appointment.DoctorId, DoctorName = appointment.Doctor.FullName, booking = appointment.Booking });
                }
                patients.Add(new PatientGet { Name = patient.FullName, Appointments = appointments });
            }
            return TypedResults.Ok(patients);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetPatient(IRepository<Patient> repository, int id)
        {
            Patient? patient = await repository.GetById(id, q => q.Include(p => p.Appointments).ThenInclude(a => a.Doctor));
            if (patient == null) return TypedResults.NotFound();
            List<AppointmentDoctor> appointments = new List<AppointmentDoctor>();
            foreach (var appointment in patient.Appointments)
            {
                appointments.Add(new AppointmentDoctor { DoctorId = appointment.DoctorId, DoctorName = appointment.Doctor.FullName, booking = appointment.Booking });
            }
            PatientGet p = new PatientGet { Name = patient.FullName, Appointments = appointments };
            return TypedResults.Ok(p);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public static async Task<IResult> CreatePatient(IRepository<Patient> repository, PatientNameOnly model)
        {
            if (model == null || model.FullName == "") return TypedResults.UnprocessableEntity("Missing information of patient");
            Patient patient = await repository.Insert(new Patient { FullName = model.FullName });
            return TypedResults.Ok(new PatientNameOnly { FullName = patient.FullName });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetDoctors(IRepository<Doctor> repository)
        {
            List<DoctorGet> doctors = new List<DoctorGet>();
            foreach (var doctor in await repository.GetWithIncludes(q => q.Include(d => d.Appointments).ThenInclude(a => a.Patient)))
            {
                List<AppointmentPatient> appointments = new List<AppointmentPatient>();
                foreach ( var appointment in doctor.Appointments)
                {
                    appointments.Add(new AppointmentPatient { PatientId = appointment.PatientId, PatientName = appointment.Patient.FullName, booking = appointment.Booking });
                }
                doctors.Add(new DoctorGet { Name = doctor.FullName, Appointments = appointments });
            }
            return TypedResults.Ok(doctors);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetDoctor(IRepository<Doctor> repository, int id)
        {
            Doctor? doctor = await repository.GetById(id, q => q.Include(d => d.Appointments).ThenInclude(a => a.Patient));
            if (doctor == null) return TypedResults.NotFound();
            List<AppointmentPatient> appointments = new List<AppointmentPatient>();
            foreach (var appointment in doctor.Appointments)
            {
                appointments.Add(new AppointmentPatient { PatientId = appointment.PatientId, PatientName = appointment.Patient.FullName, booking = appointment.Booking });
            }
            DoctorGet p = new DoctorGet { Name = doctor.FullName, Appointments = appointments };
            return TypedResults.Ok(p);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        private static async Task<IResult> CreateDoctor(IRepository<Doctor> repository, DoctorNameOnly model)
        {
            if (model == null || model.FullName == "") return TypedResults.UnprocessableEntity("Missing information of doctor.");
            Doctor patient = await repository.Insert(new Doctor { FullName = model.FullName });
            return TypedResults.Ok(new PatientNameOnly { FullName = patient.FullName });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        private static async Task<IResult> GetAppointments(IRepository<Appointment> repository)
        {
            List<AppointmentGet> appointments = new List<AppointmentGet>();
            foreach (var appointment in await repository.GetWithIncludes(q => q.Include(a => a.Patient).Include(a => a.Doctor)))
            {

                appointments.Add(new AppointmentGet
                {
                    booktime = appointment.Booking,
                    DoctorId = appointment.DoctorId,
                    DoctorName = appointment.Doctor.FullName,
                    PatientId = appointment.PatientId,
                    PatientName = appointment.Patient.FullName
                });
            }
            return TypedResults.Ok(appointments);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> GetAppointment(IRepository<Appointment> repository, int patientId, int doctorId)
        {
            Appointment? appointment = await repository.GetById(patientId, doctorId, q => q.Include(a => a.Patient).Include(a => a.Doctor));
            if (appointment == null) return TypedResults.NotFound();
            AppointmentGet AppGet = new AppointmentGet
            {
                booktime = appointment.Booking,
                DoctorId = appointment.DoctorId,
                DoctorName = appointment.Doctor.FullName,
                PatientId = appointment.PatientId,
                PatientName = appointment.Patient.FullName
            };
            return TypedResults.Ok(AppGet);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> GetAppointmentsByDoctor(IRepository<Doctor> repository, int id)
        {
            List<AppointmentGet> AppGets = new List<AppointmentGet>();
            Doctor? dooctor = await repository.GetById(id, q=>q.Include(d=>d.Appointments).ThenInclude(a => a.Patient));
            
            if (dooctor == null || dooctor.FullName == null) return TypedResults.NotFound();
            foreach (var appointment in dooctor.Appointments)
            {
                AppGets.Add(new AppointmentGet
                {
                    booktime = appointment.Booking,
                    DoctorId = appointment.DoctorId,
                    DoctorName = appointment.Doctor.FullName,
                    PatientId = appointment.PatientId,
                    PatientName = appointment.Patient.FullName
                });
            }
            return AppGets.Count() > 0 ? TypedResults.Ok(AppGets) : TypedResults.Ok("No booked appointments");
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> GetAppointmentsByPatient(IRepository<Patient> repository, int id)
        {
            List<AppointmentGet> AppGets = new List<AppointmentGet>();
            Patient? patient = await repository.GetById(id, q => q.Include(p => p.Appointments).ThenInclude(a => a.Doctor));

            if (patient == null || patient.FullName == null) return TypedResults.NotFound();
            foreach (var appointment in patient.Appointments)
            {
                AppGets.Add(new AppointmentGet
                {
                    booktime = appointment.Booking,
                    DoctorId = appointment.DoctorId,
                    DoctorName = appointment.Doctor.FullName,
                    PatientId = appointment.PatientId,
                    PatientName = appointment.Patient.FullName
                });
            }
            return AppGets.Count() > 0 ? TypedResults.Ok(AppGets) : TypedResults.Ok("No booked appointments");
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        private static async Task<IResult> CreateAppointment(IRepository<Appointment> repository, IRepository<Doctor> docrep, IRepository<Patient> patrep, AppointmentPost model)
        {
            if (model == null) return TypedResults.UnprocessableEntity("Missing information of doctor.");
            // verify that doctor and patient exists
            Doctor? doctor = await docrep.GetById(model.doctorId);
            Patient? patient = await patrep.GetById(model.patientId);
            if (doctor == null && patient == null) return TypedResults.NotFound("Neither valid doctor or patient");
            else if (patient == null) return TypedResults.NotFound("Patient not found");
            else if (doctor == null) return TypedResults.NotFound("Doctor not found");

            // check if doctor or patient already has an appointment at the time?
            // this will fail if the patient doctor combo already exists as we use the composite key of their ids
            Appointment appointment = await repository.Insert(new Appointment { PatientId = model.patientId, DoctorId = model.doctorId, Booking = model.booking });

            string s = $"Patient {patient.FullName} got appointment with {doctor.FullName} at {appointment.Booking}";
            return TypedResults.Ok(s);
        }

    }
}
