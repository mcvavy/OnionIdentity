namespace Core.Entities
{
    public class ExternalLogin
    {
        public virtual string LoginProvider { get; set; }
        public virtual string ProviderKey { get; set; }
        public virtual int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
