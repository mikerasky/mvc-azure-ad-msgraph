using Microsoft.Graph;

namespace dev365App.Services
{
    public class GraphService: IGraphService
    {
        private readonly GraphServiceClient _graphServiceClient;
        public GraphService(GraphServiceClient graphServiceClient)
        {
            _graphServiceClient = graphServiceClient ?? throw new ArgumentNullException(nameof(graphServiceClient));    
        }

        public async Task<User> GetUser() =>
            await _graphServiceClient.Me.Request().GetAsync();

        public async Task<IUserAppRoleAssignmentsCollectionPage> GetRoles() =>
            await _graphServiceClient.Me.AppRoleAssignments.Request().GetAsync();

        public async Task<IUserAppRoleAssignmentsCollectionPage> GetRoles(User? user) =>
            await _graphServiceClient.Users[user?.Id].AppRoleAssignments.Request().GetAsync();

        public async Task<ServicePrincipal> GetServicePrincipal(AppRoleAssignment? appRole) =>
            await _graphServiceClient.ServicePrincipals[appRole?.ResourceId.ToString()]
                  .Request().GetAsync();

        public async Task<IUserMemberOfCollectionWithReferencesPage> GetMemberOf(User? user) =>
            await _graphServiceClient.Users[user?.Id].MemberOf
                  .Request().GetAsync();

    }
}
