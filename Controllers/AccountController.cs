using ASP.NET_Core_MVC_Piacom.Models.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
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




        [Authorize(Roles = "SuperAdmin")]
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



        // GET: Display details of a role, including its permissions/claims
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ViewAllRoles()
        {
            var roles = roleManager.Roles.ToList(); // Get all roles
            var roleList = new List<ViewRoleViewModel>();

            // Iterate through each role to get permissions
            foreach (var role in roles)
            {
                var roleClaims = await roleManager.GetClaimsAsync(role);
                var permissions = roleClaims.Where(c => c.Type == "Permission").Select(c => c.Value).ToList();

                roleList.Add(new ViewRoleViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    Permissions = permissions
                });
            }

            return View(roleList);
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
        [Authorize(Roles = "SuperAdmin")]
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



        // GET: Display the role with existing permissions for editing
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> EditRole(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound($"Role with ID {roleId} not found.");
            }

            var roleClaims = await roleManager.GetClaimsAsync(role);

            var model = new EditRoleViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name,
                SelectedPermissions = roleClaims.Where(c => c.Type == "Permission").Select(c => c.Value).ToList(),
                AvailablePermissions = new List<string>
                {
            "View Customers", "Create Customers", "Edit Customers",
            "View Orders", "Create Orders", "Edit Orders",
            "View Units", "Create Units", "Edit Units",
            "View Products", "Create Products", "Edit Products",
            "View Prices", "Create Prices", "Edit Prices",
            "View Employees", "Create Employees", "Edit Employees"
                }
            };

            return View(model);
        }




        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var role = await roleManager.FindByIdAsync(model.RoleId);
            if (role == null)
            {
                return NotFound($"Role with ID {model.RoleId} not found.");
            }

            // Update role name
            role.Name = model.RoleName;
            role.NormalizedName = model.RoleName.ToUpper();
            var result = await roleManager.UpdateAsync(role);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Error updating the role.");
                return View(model);
            }

            // Get existing role claims
            var existingClaims = await roleManager.GetClaimsAsync(role);

            // Remove all existing permission claims
            foreach (var claim in existingClaims.Where(c => c.Type == "Permission"))
            {
                await roleManager.RemoveClaimAsync(role, claim);
            }

            // Add new selected permission claims
            foreach (var permission in model.SelectedPermissions)
            {
                await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
            }

            var usersInRole = await userManager.GetUsersInRoleAsync(model.RoleName);

            foreach (var user in usersInRole)
            {
                // Remove existing permission claims for the user
                var userClaims = await userManager.GetClaimsAsync(user);
                foreach (var claim in userClaims.Where(c => c.Type == "Permission"))
                {
                    await userManager.RemoveClaimAsync(user, claim);
                }

                // Add updated role claims to the user
                foreach (var permission in model.SelectedPermissions)
                {
                    await userManager.AddClaimAsync(user, new Claim("Permission", permission));
                }
            }
            TempData["SuccessMessage"] = $"Role {model.RoleName} updated successfully!";
            return RedirectToAction("EditRole", new { roleId = model.RoleId });
        }




        // GET: Display confirmation for deleting a role
        [HttpGet]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound($"Role with ID {roleId} not found.");
            }

            var model = new DeleteRoleViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name
            };

            return View(model);
        }




        // POST: Handle the deletion of a role
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole(DeleteRoleViewModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.RoleId))
            {
                return BadRequest("Invalid role data.");
            }

            var role = await roleManager.FindByIdAsync(model.RoleId);
            if (role == null)
            {
                return NotFound($"Role with ID {model.RoleId} not found.");
            }

            // Delete the role
            var result = await roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = $"Role '{model.RoleName}' has been deleted successfully.";
                return RedirectToAction(nameof(ViewAllRoles));
            }

            // If deletion fails, add errors to ModelState
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model); // Return the same view with error messages
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


        //[HttpPost]
        //public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(loginViewModel);
        //    }

        //    // perform the asynchronous validation for username and password
        //    var user = await userManager.FindByNameAsync(loginViewModel.Username);
        //    if (user == null)
        //    {
        //        ModelState.AddModelError(string.Empty, "Invalid username or password");
        //        return View(loginViewModel);
        //    }

        //    var result = await signInManager.CheckPasswordSignInAsync(user, loginViewModel.Password, false);
        //    if (!result.Succeeded)
        //    {
        //        ModelState.AddModelError(string.Empty, "Invalid username or password");
        //        return View(loginViewModel);
        //    }
        //    var signInResult = await signInManager
        //        .PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, false, false);

        //    if (signInResult != null && signInResult.Succeeded)
        //    {
        //        if (!string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl))
        //        {
        //            return Redirect(loginViewModel.ReturnUrl);
        //        }
        //        return RedirectToAction("Index", "Home");
        //    }
        //    return View(loginViewModel);
        //}

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            // Perform the asynchronous validation for username and password
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

            // Create claims for the user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
        // Add other claims as needed, e.g. roles
            };

            // You can also add roles as claims if needed
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // Sign in the user with claims
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            // Redirect based on returnUrl or to the home page
            if (!string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl) && Url.IsLocalUrl(loginViewModel.ReturnUrl))
            {
                return Redirect(loginViewModel.ReturnUrl);
            }

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult LoginWithGoogle(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider
            var redirectUrl = Url.Action(nameof(GoogleResponse), "Account", new { returnUrl });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, provider);
        }

        [HttpGet]
        public async Task<IActionResult> GoogleResponse(string returnUrl = null)
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (result.Succeeded && result.Principal != null)
            {
                var email = result.Principal.FindFirstValue(ClaimTypes.Email);
                var user = await userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    //Create a new user if they don't exist
                    var identityUser = new IdentityUser
                    {
                        UserName = email,
                        Email = email
                    };

                    var createUserResult = await userManager.CreateAsync(identityUser);
                    if (createUserResult.Succeeded)
                    {
                        //Assign the new user to the "User" role
                        await userManager.AddToRoleAsync(identityUser, "User");
                        return RedirectToAction("CompleteRegistration", "Account", new { userId = identityUser.Id });

                    }
                    else
                    {
                        //Handle user creation failure(e.g., log errors)
                        ModelState.AddModelError(string.Empty, "User creation failed.");
                        return RedirectToAction("Login", "Account");
                    }
                }

                // Set up the custom claims for the signed-in user
                var claims = new List<Claim>
                {
                     new Claim(ClaimTypes.Name, user.UserName),
                     new Claim(ClaimTypes.Email, user.Email)
                };
                // Get all roles for the user and their claims
                var roles = await userManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));

                    // Get claims associated with the role and add them
                    var identityRole = await roleManager.FindByNameAsync(role);
                    if (identityRole != null)
                    {
                        var roleClaims = await roleManager.GetClaimsAsync(identityRole);
                        foreach (var claim in roleClaims)
                        {
                            claims.Add(claim);
                        }
                    }
                }

                // Create a ClaimsIdentity and sign in using these claims
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                // Sign in with the custom ClaimsPrincipal
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal,
                    new AuthenticationProperties { IsPersistent = false });

                return RedirectToAction("Index", "Home");

            }

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult CompleteRegistration(string userId)
        {
            var model = new CompleteRegistrationViewModel { UserId = userId };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CompleteRegistration(CompleteRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.UserId);
                if (user != null)
                {
                    var result = await userManager.AddPasswordAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
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
        [Authorize(Roles = "SuperAdmin")]
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
        [Authorize(Roles = "SuperAdmin")]
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

            return RedirectToAction(nameof(UserRoles));
        }
    }
}
