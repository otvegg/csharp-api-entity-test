using AutoMapper;
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
        public static async Task<IResult> GetPatients(IRepository<Patient> repository,  IMapper mapper)
        {
            var patients = await repository.GetWithIncludes(q => q.Include(p => p.Appointments).ThenInclude(a => a.Doctor));

            List<PatientGet> patientsDto = mapper.Map<List<PatientGet>>(patients);

            //List<PatientGet> patients = new List<PatientGet>();
            //foreach (var patient in await repository.GetWithIncludes(q => q.Include(p => p.Appointments).ThenInclude(a => a.Doctor)))
            //{
            //    List<AppointmentDoctor> appointments = new List<AppointmentDoctor>();
            //    foreach (var appointment in patient.Appointments)
            //    {
            //        appointments.Add(new AppointmentDoctor { 
            //            DoctorId = appointment.DoctorId, 
            //            DoctorFullName = appointment.Doctor.FullName,
            //            Booktime = appointment.Booktime, 
            //            Type=appointment.Type });
            //    }
            //    patients.Add(new PatientGet { FullName = patient.FullName, Appointments = appointments });
            //}


            return TypedResults.Ok(patientsDto);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetPatient(IRepository<Patient> repository, IMapper mapper, int id)
        {
            Patient? patient = await repository.GetById(id, q => q.Include(p => p.Appointments).ThenInclude(a => a.Doctor));
            if (patient == null) return TypedResults.NotFound();

            PatientGet p = mapper.Map<PatientGet>(patient);

            //List<AppointmentDoctor> appointments = new List<AppointmentDoctor>();
            //foreach (var appointment in patient.Appointments)
            //{
            //    appointments.Add(new AppointmentDoctor { 
            //        DoctorId = appointment.DoctorId,
            //        DoctorFullName = appointment.Doctor.FullName,
            //        Booktime = appointment.Booktime,
            //        Type = appointment.Type
            //    });
            //}
            //PatientGet p = new PatientGet { FullName = patient.FullName, Appointments = appointments };
            return TypedResults.Ok(p);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public static async Task<IResult> CreatePatient(IRepository<Patient> repository, IMapper  mapper, PatientPost model)
        {
            if (model == null || model.FullName == "") return TypedResults.UnprocessableEntity("Missing information of patient");
            Patient patient = await repository.Insert(mapper.Map<Patient>(model));
            return TypedResults.Ok(new { FullName = patient.FullName });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetDoctors(IRepository<Doctor> repository, IMapper mapper)
        {
            var doctors = await repository.GetWithIncludes(q => q.Include(d => d.Appointments).ThenInclude(a => a.Patient));
            List<DoctorGet> doctorDTO = mapper.Map<List<DoctorGet>>(doctors);
            //List<PatientGet> patientsDto = mapper.Map<List<PatientGet>>(patients);

            //List<DoctorGet> doctors = new List<DoctorGet>();
            //foreach (var doctor in await repository.GetWithIncludes(q => q.Include(d => d.Appointments).ThenInclude(a => a.Patient)))
            //{
            //    List<AppointmentPatient> appointments = new List<AppointmentPatient>();
            //    foreach ( var appointment in doctor.Appointments)
            //    {
            //        appointments.Add(new AppointmentPatient { 
            //            PatientId = appointment.PatientId, 
            //            PatientFullName = appointment.Patient.FullName,
            //            Booktime = appointment.Booktime,
            //            Type = appointment.Type
            //        });
            //    }
            //    doctors.Add(new DoctorGet { FullName = doctor.FullName, Appointments = appointments });
            //}
            return TypedResults.Ok(doctorDTO);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetDoctor(IRepository<Doctor> repository, IMapper mapper, int id)
        {
            Doctor? doctor = await repository.GetById(id, q => q.Include(d => d.Appointments).ThenInclude(a => a.Patient));
            if (doctor == null) return TypedResults.NotFound();

            DoctorGet doctorDTO = mapper.Map<DoctorGet>(doctor);
            //List<AppointmentPatient> appointments = new List<AppointmentPatient>();
            //foreach (var appointment in doctor.Appointments)
            //{
            //    appointments.Add(new AppointmentPatient { 
            //        PatientId = appointment.PatientId, 
            //        PatientFullName = appointment.Patient.FullName,
            //        Booktime = appointment.Booktime,
            //        Type = appointment.Type
            //    });
            //}
            //DoctorGet p = new DoctorGet { FullName = doctor.FullName, Appointments = appointments };
            return TypedResults.Ok(doctorDTO);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        private static async Task<IResult> CreateDoctor(IRepository<Doctor> repository, IMapper mapper, DoctorPost model)
        {
            if (model == null || model.FullName == "") return TypedResults.UnprocessableEntity("Missing information of doctor.");
            //Doctor doctor = await repository.Insert(new Doctor { FullName = model.FullName });
            Doctor doctor = await repository.Insert(mapper.Map<Doctor>(model));
            return TypedResults.Ok(new DoctorGet { FullName = doctor.FullName });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        private static async Task<IResult> GetAppointments(IRepository<Appointment> repository, IMapper mapper)
        {
            var appointments = await repository.GetWithIncludes(q => q.Include(a => a.Patient).Include(a => a.Doctor));
            List<AppointmentGet> appointmentsDTO = mapper.Map<List<AppointmentGet>>(appointments);
            //List<AppointmentGet> appointments = new List<AppointmentGet>();
            //foreach (var appointment in await repository.GetWithIncludes(q => q.Include(a => a.Patient).Include(a => a.Doctor)))
            //{

            //    appointments.Add(new AppointmentGet
            //    {
            //        Booktime = appointment.Booktime,
            //        DoctorId = appointment.DoctorId,
            //        DoctorFullName = appointment.Doctor.FullName,
            //        PatientId = appointment.PatientId,
            //        PatientFullName = appointment.Patient.FullName,
            //        Type = appointment.Type
            //    });
            //}
            return TypedResults.Ok(appointmentsDTO);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> GetAppointment(IRepository<Appointment> repository, IMapper mapper, int patientId, int doctorId)
        {
            Appointment? appointment = await repository.GetById(patientId, doctorId, q => q.Include(a => a.Patient).Include(a => a.Doctor));
            if (appointment == null) return TypedResults.NotFound();
            AppointmentGet appointmentDTO = mapper.Map<AppointmentGet>(appointment);

            //AppointmentGet AppGet = new AppointmentGet
            //{
            //    Booktime = appointment.Booktime,
            //    DoctorId = appointment.DoctorId,
            //    DoctorFullName = appointment.Doctor.FullName,
            //    PatientId = appointment.PatientId,
            //    PatientFullName = appointment.Patient.FullName,
            //    Type = appointment.Type
            //};
            return TypedResults.Ok(appointmentDTO);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> GetAppointmentsByDoctor(IRepository<Doctor> repository, IMapper mapper, int id)
        {
            
            Doctor? dooctor = await repository.GetById(id, q=>q.Include(d=>d.Appointments).ThenInclude(a => a.Patient));
            
            if (dooctor == null || dooctor.FullName == null) return TypedResults.NotFound();
            List<AppointmentGet> appointmentsDTO = mapper.Map<List<AppointmentGet>>(dooctor.Appointments);
            //List<AppointmentGet> AppGets = new List<AppointmentGet>();
            //foreach (var appointment in dooctor.Appointments)
            //{
            //    AppGets.Add(new AppointmentGet
            //    {
            //        Booktime = appointment.Booktime,
            //        DoctorId = appointment.DoctorId,
            //        DoctorFullName = appointment.Doctor.FullName,
            //        PatientId = appointment.PatientId,
            //        PatientFullName = appointment.Patient.FullName,
            //        Type = appointment.Type
            //    });
            //}
            return appointmentsDTO.Count() > 0 ? TypedResults.Ok(appointmentsDTO) : TypedResults.Ok("No booked appointments");
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> GetAppointmentsByPatient(IRepository<Patient> repository, IMapper mapper, int id)
        {
            Patient? patient = await repository.GetById(id, q => q.Include(p => p.Appointments).ThenInclude(a => a.Doctor));

            if (patient == null || patient.FullName == null) return TypedResults.NotFound();
            List<AppointmentGet> appointmentsDTO = mapper.Map<List<AppointmentGet>>(patient.Appointments);
            //List<AppointmentGet> AppGets = new List<AppointmentGet>();
            //foreach (var appointment in patient.Appointments)
            //{
            //    AppGets.Add(new AppointmentGet
            //    {
            //        Booktime = appointment.Booktime,
            //        DoctorId = appointment.DoctorId,
            //        DoctorFullName = appointment.Doctor.FullName,
            //        PatientId = appointment.PatientId,
            //        PatientFullName = appointment.Patient.FullName,
            //        Type = appointment.Type
            //    });
            //}
            return appointmentsDTO.Count() > 0 ? TypedResults.Ok(appointmentsDTO) : TypedResults.Ok("No booked appointments");
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        private static async Task<IResult> CreateAppointment(IRepository<Appointment> repository, IRepository<Doctor> docrep, IRepository<Patient> patrep, IMapper mapper, AppointmentPost model)
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
            //Appointment appointment = await repository.Insert(new Appointment { PatientId = model.patientId, DoctorId = model.doctorId, Booktime = model.Booktime });
            Appointment appointment = await repository.Insert(mapper.Map<Appointment>(model));


            string s = $"Patient {patient.FullName} got appointment with {doctor.FullName} at {appointment.Booktime}";
            return TypedResults.Ok(s);
        }

    }
}
