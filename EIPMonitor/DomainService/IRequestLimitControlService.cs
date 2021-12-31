namespace EIPMonitor.DomainService
{
    public interface IRequestLimitControlService
    {
        void ReleaseClickPermission(string className, string controlName);
        bool RequestClickPermission(string className, string controlName);
    }
}