using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ASP.NET_Core_MVC_Piacom.Authorizes
{
    public class ViewClaimHandler : AuthorizationHandler<ViewClaimRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ViewClaimRequirement requirement)
        {
            // Check for Create and Edit claims
            bool hasCreateOrderClaim = context.User.HasClaim(c => c.Type == "Permission" && c.Value == "CreateOrders");
            bool hasEditOrderClaim = context.User.HasClaim(c => c.Type == "Permission" && c.Value == "EditOrders");
            bool hasViewOrderClaim = context.User.HasClaim(c => c.Type == "Permission" && c.Value == "ViewOrders");

            // If the user has both claims, grant them the View claim
            if (hasCreateOrderClaim || hasEditOrderClaim || hasViewOrderClaim)
            {
                context.User.AddIdentity(new ClaimsIdentity(new[]
                {
                new Claim("Permission", "ViewOrders")
            }));

                context.Succeed(requirement);
            }

            bool hasCreateCustomerClaim = context.User.HasClaim(c => c.Type == "Permission" && c.Value == "CreateCustomers");
            bool hasEditCustomerClaim = context.User.HasClaim(c => c.Type == "Permission" && c.Value == "EditCustomers");
            bool hasViewCustomerClaim = context.User.HasClaim(c => c.Type == "Permission" && c.Value == "ViewCustomers");

            // If the user has both claims, grant them the View claim
            if (hasCreateCustomerClaim || hasEditCustomerClaim || hasViewCustomerClaim)
            {
                context.User.AddIdentity(new ClaimsIdentity(new[]
                {
                new Claim("Permission", "ViewCustomers")
            }));

                context.Succeed(requirement);
            }

            bool hasCreateUnitClaim = context.User.HasClaim(c => c.Type == "Permission" && c.Value == "CreateUnits");
            bool hasEditUnitClaim = context.User.HasClaim(c => c.Type == "Permission" && c.Value == "EditUnits");
            bool hasViewUnitClaim = context.User.HasClaim(c => c.Type == "Permission" && c.Value == "ViewUnits");

            // If the user has both claims, grant them the View claim
            if (hasCreateUnitClaim || hasEditUnitClaim || hasViewUnitClaim)
            {
                context.User.AddIdentity(new ClaimsIdentity(new[]
                {
                new Claim("Permission", "ViewUnits")
            }));

                context.Succeed(requirement);
            }

            bool hasCreateProductClaim = context.User.HasClaim(c => c.Type == "Permission" && c.Value == "CreateProducts");
            bool hasEditProductClaim = context.User.HasClaim(c => c.Type == "Permission" && c.Value == "EditProducts");
            bool hasViewProductClaim = context.User.HasClaim(c => c.Type == "Permission" && c.Value == "ViewProducts");

            // If the user has both claims, grant them the View claim
            if (hasCreateProductClaim || hasEditProductClaim || hasViewProductClaim)
            {
                context.User.AddIdentity(new ClaimsIdentity(new[]
                {
                new Claim("Permission", "ViewProducts")
            }));

                context.Succeed(requirement);
            }

            bool hasCreatePriceClaim = context.User.HasClaim(c => c.Type == "Permission" && c.Value == "CreatePrices");
            bool hasEditPriceClaim = context.User.HasClaim(c => c.Type == "Permission" && c.Value == "EditPrices");
            bool hasViewPriceClaim = context.User.HasClaim(c => c.Type == "Permission" && c.Value == "ViewPrices");

            // If the user has both claims, grant them the View claim
            if (hasCreatePriceClaim || hasEditPriceClaim || hasViewPriceClaim)
            {
                context.User.AddIdentity(new ClaimsIdentity(new[]
                {
                new Claim("Permission", "ViewPrices")
            }));

                context.Succeed(requirement);
            }

            bool hasCreateEmployeeClaim = context.User.HasClaim(c => c.Type == "Permission" && c.Value == "CreateEmployees");
            bool hasEditEmployeeClaim = context.User.HasClaim(c => c.Type == "Permission" && c.Value == "EditEmployees");
            bool hasViewEmployeeClaim = context.User.HasClaim(c => c.Type == "Permission" && c.Value == "ViewEmployees");

            // If the user has both claims, grant them the View claim
            if (hasCreateEmployeeClaim || hasEditEmployeeClaim || hasViewEmployeeClaim)
            {
                context.User.AddIdentity(new ClaimsIdentity(new[]
                {
                new Claim("Permission", "ViewEmployees")
            }));

                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
