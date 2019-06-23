using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using NHibernate;
using Restaurant.DataModel.DataFramework;
using Restaurant.DataModel.Entity;
using RestaurantApi.Hateos;
using RestaurantApi.Support;

namespace RestaurantApi.Apis
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RoleController : ApiCrudController
    {
       
        
        
        public RoleController(ISessionFactory sessionFactory, IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor) : base(sessionFactory, urlHelperFactory, actionContextAccessor) { }
        // GET: api/Role
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                IList<Role> roles = dao.List<Role>();
                foreach (Role role in roles)
                {
                    CreateHateosLinks(role);
                }
                return Ok(roles);
            }
            catch(Exception e)
            {
                return HandleGetException(e);
            }
        }

        // GET: api/Role/5
        [HttpGet("{id}", Name = nameof(GetRole))]
        public IActionResult GetRole(Guid id)
        {
            try
            {
                Role role = LoadEntityById<Role>(id);
                return Ok(CreateHateosLinks(role));
            }
            catch(Exception e)
            {
                return HandleGetException(e);
            }
        }



        [HttpGet("/api/v1/Role/Name/{name}", Name = nameof(GetRoleByName))]
        public IActionResult GetRoleByName(string name)
        {
            try
            {
                Role role = LoadEntityByKeyValue<Role>("Name", name);
                return Ok(CreateHateosLinks(role));
            }
            catch (Exception e)
            {
                return HandleGetException(e);
            }
        }

        // POST: api/Role
        [HttpPost]
        public IActionResult Post([FromBody] Role role)
        {
            try
            {

                ValidateEntityNotInDatabase<Role>(role);
                role=dao.Save<Role>(role);
                return Ok(CreateHateosLinks(role));
                
            }
            catch (Exception e)
            {
                return HandlePostException(e);

            }
        }

        // PUT: api/Role/5
        [HttpPut("{id}", Name = nameof(ModifyRole))]
        public IActionResult  ModifyRole(Guid id, [FromBody] Role role)
        {
            try
            {
                ValidateIds<Role>(id, role);
                ValidateEntityInDatabase<Role>(id);
                role = dao.Merge<Role>(role);
                return Ok(CreateHateosLinks(role));

            }
            catch (Exception e)
            {
                return HandlePutException(e);

            }
        }

        // PATCH: api/Role/5
        [HttpPatch("{id}", Name = nameof(PartiallyModifyRole))]
        public IActionResult PartiallyModifyRole(Guid id, [FromBody] Role role)
        {
            try
            {
                ValidateIds<Role>(id, role);
                ValidateEntityInDatabase<Role>(id);
                role=dao.Merge<Role>(role);
                return Ok(CreateHateosLinks(role));

            }
            catch (Exception e)
            {
                return HandlePatchException(e);

            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}", Name = nameof(DeleteRole))]
        public IActionResult DeleteRole(Guid id)
        {
            try
            {
                Role role = LoadEntityById<Role>(id);
                dao.Delete(role);
                return Ok(role);
            }
            catch (Exception e)
            {
                return HandleDeleteException(e);

            }
        }
   
        private Role CreateHateosLinks(Role role)
        {
            role.Users = null;
           
            var idObj = new { id = role.Id };
            role.Links = new List<HateosLink>();
            role.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.GetRole), idObj),
                "self",
                HateosLink.ACTION_GET,HateosLink.MEDIA_TYPE_APPLICATION_JSON));

            role.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.ModifyRole), idObj),
                "role",
                HateosLink.ACTION_PUT, HateosLink.MEDIA_TYPE_APPLICATION_JSON));


            role.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.PartiallyModifyRole), idObj),
                "role",
                HateosLink.ACTION_PATCH,HateosLink.MEDIA_TYPE_URLENCODED));

            role.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.DeleteRole), idObj),
                "role",
                HateosLink.ACTION_DELETE));

            role.Links.Add(new HateosLink(this.urlHelper.Link(nameof(this.GetRole), idObj),
                "self",
                HateosLink.ACTION_GET,HateosLink.MEDIA_TYPE_APPLICATION_JSON));


            role.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.ModifyRole), idObj),
                "self",
                HateosLink.ACTION_PUT,HateosLink.MEDIA_TYPE_URLENCODED));

            role.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.PartiallyModifyRole), idObj),
                "self",
                HateosLink.ACTION_PATCH,HateosLink.MEDIA_TYPE_URLENCODED));

            role.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.DeleteRole), idObj),
                "self",
                HateosLink.ACTION_DELETE));
            return role;
        }
}
}
