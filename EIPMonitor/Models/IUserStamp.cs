namespace EIPMonitor.Model
{
    public interface IUserStamp
    {
        string Address { get; }
        string EmployeeId { get; set; }
        string UserName { get; set; }

        void Assign(IUserStamp source);
        bool IsAvailable();
    }
}