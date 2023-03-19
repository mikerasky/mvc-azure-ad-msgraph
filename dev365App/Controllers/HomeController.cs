using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using dev365App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using dev365App.Services;

namespace dev365App.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IGraphService _graphService;
    public HomeController(ILogger<HomeController> logger,
                          IGraphService graphService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _graphService = graphService ?? throw new ArgumentNullException(nameof(graphService));
    }

    [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
    public async Task<IActionResult> Index()
    {
        var user = await _graphService.GetUser();
        var roles = await _graphService.GetRoles();

        var rolesByUser = await _graphService.GetRoles(user);
        var appRole = rolesByUser.Where(a => a.AppRoleId != Guid.Empty).First();
        var principal = await _graphService.GetServicePrincipal(appRole);
        var apa = principal.AppRoles.Where(a => a.Id == appRole.AppRoleId).First().DisplayName;

        var memberOf = await _graphService.GetMemberOf(user);

        var userGroups = new List<RoleGroups>();
        var roleGroups = new RoleGroups();
        roleGroups.UserId = user.Id;
        roleGroups.Groups = new List<Models.GroupObject>();
        foreach (var directoryObject in memberOf.CurrentPage)
        {
            if (directoryObject.GetType() == typeof(DirectoryRole))
                roleGroups.Role = ((DirectoryRole)(directoryObject)).DisplayName;
            else
            {
                var groupName = ((Group)(directoryObject)).DisplayName;
                roleGroups.Groups.Add(new() { Name = groupName });
            }
        }
        ViewData["MyClaimRole"] = apa;
        ViewData["MyGroups"] = roleGroups;
        ViewData["MyUserName"] = user.DisplayName;
        ViewData["MyRole"] = roles.AsEnumerable();
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
