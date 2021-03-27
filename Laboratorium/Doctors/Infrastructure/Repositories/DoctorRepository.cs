namespace Doctors.Infrastructure
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.DoctorsAggregate;
    using EntityFramework;

    public class DoctorRepository : IDoctorRepository
    {


        private Doctor[] Doctors;

        public IEnumerable<Doctor> GetAll(DoctorContext doctorContext)
        {
            Doctors = doctorContext.Doctors.ToArray();
            return Doctors;
        }

        public IEnumerable<Doctor> GetBySpecialization(DoctorContext doctorContext, int certificationType)
        {
            Doctors = doctorContext.Doctors.ToArray();
            return Doctors.Where(ld => ld.Specializations.Any(s => s.Number == certificationType));
        }
    }
}