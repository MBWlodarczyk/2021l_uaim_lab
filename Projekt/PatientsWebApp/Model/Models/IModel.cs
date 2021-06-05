namespace Model.Models
{
    using IData;
    using IOperation;

    public interface IModel : IDoctorData, IDoctorOperation, IPatientData, IPatientOperation
    {
    }
}