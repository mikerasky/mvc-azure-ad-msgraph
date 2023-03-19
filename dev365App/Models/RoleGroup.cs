namespace dev365App.Models
{
    public class RoleGroups
    {
        public string? UserId { get; set; }
        public string? Role { get; set; }
        public List<GroupObject>? Groups { get;set; }
    }

    public class GroupObject
    {
        public string? Name { get; set; }
    }
}
