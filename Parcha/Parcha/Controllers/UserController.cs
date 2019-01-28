using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Parcha.Data;
using Parcha.Data.Models;
using Parcha.ViewModels;

namespace Parcha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseApiController
    {
        private UserManager<ApplicationUser> userManager;
        #region Constructor
        public UserController(
        ApplicationDbContext context,
        RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration
        )
        : base(context, roleManager, userManager, configuration)
        {
            this.userManager = userManager;
        }
        #endregion

        #region RESTful Conventions
        /// <summary>
        /// POST: api/user
        /// </summary>
        /// <returns>Creates a new User and return it accordingly.
        ///</returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]UserViewModel model)
        {
            if (model == null) return new StatusCodeResult(500);
            ApplicationUser user = await
            UserManager.FindByNameAsync(model.UserName);
            if (user != null) return BadRequest("Username already exists");
           
            user = await UserManager.FindByEmailAsync(model.Email);
            if (user != null) return BadRequest("Email already exists.");
           
            var now = DateTime.Now;
            // create a new Item with the client-sent json data
            user = new ApplicationUser()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                Email = model.Email,
                DisplayName = model.DisplayName,
                CreatedDate = now,
                LastModifiedDate = now
            };
            // Add the user to the Db with the choosen password
            await UserManager.CreateAsync(user, model.Password);
            // Assign the user to the 'RegisteredUser' role.
            await UserManager.AddToRoleAsync(user, "RegisteredUser");
            // Remove Lockout and E-Mail confirmation
            user.EmailConfirmed = true;
            user.LockoutEnabled = false;
            // persist the changes into the Database.
            DbContext.SaveChanges();
            // return the newly-created User to the client.
            return new JsonResult(user.Adapt<UserViewModel>(),JsonSettings);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult("Furqan", JsonSettings);

        }
        #endregion
    }
}
    