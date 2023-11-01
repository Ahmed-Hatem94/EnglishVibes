using EnglishVibes.API.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EnglishVibes.API.Controllers
{
    //[Authorize(Roles = "admin")]
    public class RoleController : BaseAPIController
    {
        private readonly RoleManager<IdentityRole<Guid>> roleManager;
        public RoleController(RoleManager<IdentityRole<Guid>> _roleManager)
        {
            roleManager = _roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(RoleDTO roleDTO)
        {
            if (ModelState.IsValid)
            {
                // create role
                var role = new IdentityRole<Guid>();
                role.Name = roleDTO.RoleName;
                IdentityResult result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {                    
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return BadRequest(ModelState);
        }
    }
}
