namespace Botticelli.Server.Data.Entities.Auth
{
    public class ApplicationUserPost
    {
        public Guid FarmId {get;set;}
        public string FullName { get; set; }
        public int StatusId {get;set;}
        public ICollection<Guid> UserRoleIds { get; set; }
    }
}