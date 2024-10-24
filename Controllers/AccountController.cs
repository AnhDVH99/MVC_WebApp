using ASP.NET_Core_MVC_Piacom.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Security.Claims;

namespace ASP.NET_Core_MVC_Piacom.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }

            var identityUser = new IdentityUser
            {
                UserName = registerViewModel.Username,
                Email = registerViewModel.Email
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerViewModel.Password);

            if (identityResult.Succeeded)
            {
                // assign this user the User role
                var roleIdentityResult = await userManager.AddToRoleAsync(identityUser, "User");
                if (roleIdentityResult.Succeeded)
                {
                    TempData["SuccessMessage"] = "User registered successfully!";
                    return RedirectToAction("Register");
                }
            }
            return View(registerViewModel);
        }
        public async Task<IActionResult> UserRoles()
        {
            var users = userManager.Users.ToList();
            var userRolesViewModels = new List<UserRoleViewModel>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                userRolesViewModels.Add(new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Roles = roles.ToList()
                });
            }

            return View(userRolesViewModels);
        }

        // GET: Display form to create a new role
        [HttpGet]
        public IActionResult CreateRole()
        {
            var model = new CreateRoleViewModel
            {
                Permissions = new List<string> { "View Customers", "Create Customers", "Edit Customers", 
                "View Orders", "Create Orders", "Edit Orders",
                "View Units", "Create Units", "Edit Units",
                "View Products", "Create Products", "Edit Products",
                "View Prices", "Create Prices", "Edit Prices",
                "View Employees", "Create Employees", "Edit Employees"} // Example permissions
            };
            return View(model);
        }

        // POST: Handle the creation of a new role
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel createRoleViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(createRoleViewModel);
            }
            

            // Check if the role already exists
            var roleExists = await roleManager.RoleExistsAsync(createRoleViewModel.RoleName);
            if (!roleExists)
            {
                var role = new IdentityRole
                {
                    Name = createRoleViewModel.RoleName,
                    NormalizedName = createRoleViewModel.RoleName.ToUpper(),
                    Id = Guid.NewGuid().ToString(),  // New role ID
                    ConcurrencyStamp = Guid.NewGuid().ToString()  // New concurrency stamp
                };

                var result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    foreach (var permission in createRoleViewModel.Permissions)
                    {
                        await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                    }

                    TempData["SuccessMessage"] = $"Role {createRoleViewModel.RoleName} created successfully!";
                    return RedirectToAction("CreateRole");
                }
            }

            ModelState.AddModelError(string.Empty, $"Role '{createRoleViewModel.RoleName}' already exists.");
            return View(createRoleViewModel);
        }


        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            var model = new LoginViewModel
            {
                ReturnUrl = ReturnUrl
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            // perform the asynchronous validation for username and password
            var user = await userManager.FindByNameAsync(loginViewModel.Username);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password");
                return View(loginViewModel);
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, loginViewModel.Password, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password");
                return View(loginViewModel);
            }
            var signInResult = await signInManager
                .PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, false, false);

            if (signInResult != null && signInResult.Succeeded)
            {
                if (!string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl))
                {
                    return Redirect(loginViewModel.ReturnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            return View(loginViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> AddRoleToUser()
        {
            var users = userManager.Users.ToList();
            var roles = roleManager.Roles.Select(r => r.Name).ToList();

            var model = new List<AddRoleToUsers>();
            foreach (var user in users)
            {
                // Get user roles for the current user
                var userRoles = await userManager.GetRolesAsync(user);


                model.Add(new AddRoleToUsers
                {
                    UserId = user.Id,
                    Username = user.UserName,
                    Roles = roles
                    
                });
            }

            return View(model);
        }


        // Handle POST request to assign a role
        [HttpPost]
        [Authorize(Roles ="SuperAdmin")]
        public async Task<IActionResult> AddRoleToUser(string userId, string role)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            var roleExists = await roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
                ModelState.AddModelError("", $"Role {role} does not exist.");
                return RedirectToAction("AddRoleToUser");
            }

            var result = await userManager.AddToRoleAsync(user, role);
            if (result.Succeeded)
            {
                // Assign claims
                var identityRole = await roleManager.FindByNameAsync(role);
                if (identityRole != null)
                {
                    // Assign claims from the role to the user
                    var roleClaims = await roleManager.GetClaimsAsync(identityRole);
                    foreach (var claim in roleClaims)
                    {
                        await userManager.AddClaimAsync(user, claim);
                    }
                }
                TempData["SuccessMessage"] = $"Role {role} has been successfully assigned to user {user.UserName}.";
            }
            else
            {
                TempData["ErrorMessage"] = $"Failed to assign role {role} to user {user.UserName}.";
            }

            return RedirectToAction(nameof(UserRoles));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserRole(string userId, string roleName)
        {
            // Find the user
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // Check if trying to remove superadmin role
                if (roleName.Equals("superadmin", StringComparison.OrdinalIgnoreCase))
                {
                    TempData["ErrorMessage"] = "Cannot remove the 'superadmin' role.";
                    return RedirectToAction(nameof(UserRoles));
                }

                // Get all roles of the user
                var userRoles = await userManager.GetRolesAsync(user);

                // Check if user has only one role left
                if (userRoles.Count() <= 1)
                {
                    TempData["ErrorMessage"] = "User must have at least one role remaining.";
                    return RedirectToAction(nameof(UserRoles));
                }
                var result = await userManager.RemoveFromRoleAsync(user, roleName);

                if (result.Succeeded)
                {
                    // Remove associated claims with the role
                    var identityRole = await roleManager.FindByNameAsync(roleName);
                    if (identityRole != null)
                    {
                        // Get all claims for the role and remove them from the user
                        var roleClaims = await roleManager.GetClaimsAsync(identityRole);
                        foreach (var claim in roleClaims)
                        {
                            var userClaims = await userManager.GetClaimsAsync(user);
                            if (userClaims.Any(c => c.Type == claim.Type && c.Value == claim.Value))
                            {
                                await userManager.RemoveClaimAsync(user, claim);
                            }
                        }
                    }
                    TempData["SuccessMessage"] = $"Role '{roleName}' and associated claims removed from user '{user.UserName}'.";


                    // Refresh the model after deletion
                    var users = userManager.Users.ToList();
                    var userRolesViewModel = new List<UserRoleViewModel>();

                    foreach (var usr in users)
                    {
                        var roles = await userManager.GetRolesAsync(usr);
                        userRolesViewModel.Add(new UserRoleViewModel
                        {
                            UserId = usr.Id,
                            UserName = usr.UserName,
                            Roles = roles.ToList()
                        });
                    }

                    
                    // Return the view with the refreshed data
                    return View("UserRoles", userRolesViewModel);
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to remove the role.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "User not found.";
            }

            // Return the view if there's an error (you might want to refactor this part to avoid duplicate code)
            return RedirectToAction(nameof(UserRoles));
        }
    }
}
