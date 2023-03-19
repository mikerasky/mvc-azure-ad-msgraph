using Microsoft.Graph;

namespace dev365App.Services
{
    public interface IGraphService
    {
        Task<User> GetUser();

        Task<IUserAppRoleAssignmentsCollectionPage> GetRoles();

        Task<IUserAppRoleAssignmentsCollectionPage> GetRoles(User? user);

        Task<ServicePrincipal> GetServicePrincipal(AppRoleAssignment? appRole);

        Task<IUserMemberOfCollectionWithReferencesPage> GetMemberOf(User? user);
    }
}
