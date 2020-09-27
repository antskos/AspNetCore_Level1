using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Domain.Entities.Identity;

namespace WebStore.ServiceHosting.Controllers
{
    [Route(WebAPI.Identity.Roles)]
    [ApiController]
    public class RolesApiController : ControllerBase
    {
        private readonly RoleStore<Role> _roleStore;

        public RolesApiController(WebStoreDB db)
        {
            _roleStore = new RoleStore<Role>(db);
        }

        [HttpGet("all")]
        public async Task<IEnumerable<Role>> GetAllRoles() => await _roleStore.Roles.ToArrayAsync();

        [HttpPost]
        public async Task<bool> CreateAsync(Role role)
        {
            var creation_result = await _roleStore.CreateAsync(role);
            return creation_result.Succeeded;
        }

        [HttpPut]
        public async Task<bool> UpdateAsync(Role role)
        {
            var uprate_result = await _roleStore.UpdateAsync(role);
            return uprate_result.Succeeded;
        }

        [HttpPost("Delete")]
        public async Task<bool> DeleteAsync(Role role)
        {
            var delete_result = await _roleStore.DeleteAsync(role);
            return delete_result.Succeeded;
        }

        [HttpPost("GetRoleId")]
        public async Task<string> GetRoleIdAsync([FromBody] Role role) => await _roleStore.GetRoleIdAsync(role);

        [HttpPost("GetRoleName")]
        public async Task<string> GetRoleNameAsync([FromBody] Role role) => await _roleStore.GetRoleNameAsync(role);

        [HttpPost("SetRoleName/{name}")]
        public async Task SetRoleNameAsync(Role role, string name)
        {
            await _roleStore.SetRoleNameAsync(role, name);
            await _roleStore.UpdateAsync(role);
        }

        [HttpPost("GetNormalizedRoleName")]
        public async Task<string> GetNormalizedRoleNameAsync(Role role) => await _roleStore.GetNormalizedRoleNameAsync(role);

        [HttpPost("SetNormalizedRoleName/{name}")]
        public async Task SetNormalizedRoleNameAsync(Role role, string name)
        {
            await _roleStore.SetNormalizedRoleNameAsync(role, name);
            await _roleStore.UpdateAsync(role);
        }

        [HttpGet("FindById/{id}")]
        public async Task<Role> FindByIdAsync(string id) => await _roleStore.FindByIdAsync(id);

        [HttpGet("FindByName/{name}")]
        public async Task<Role> FindByNameAsync(string name) => await _roleStore.FindByNameAsync(name);
    }
}