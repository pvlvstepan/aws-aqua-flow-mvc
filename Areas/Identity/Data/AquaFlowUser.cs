using Microsoft.AspNetCore.Identity;

namespace AquaFlow.Areas.Identity.Data;

// Add profile data for application users by adding properties to the AquaFlowUser class
public class AquaFlowUser : IdentityUser
{
    public string Street { get; set; } = "";
    public string City { get; set; } = "";
    public string State { get; set; } = "";
    public string ZipCode { get; set; } = "";

    public string FullName { get; set; } = "";
}

