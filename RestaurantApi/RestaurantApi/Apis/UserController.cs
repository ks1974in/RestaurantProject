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
    public class UserController : ApiCrudController
    {
        public UserController(ISessionFactory sessionFactory, IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor) : base(sessionFactory, urlHelperFactory, actionContextAccessor) { }
        // GET: api/User
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                IList<User> users = dao.List<User>();
                foreach (User user in users)
                {
                    CreateHateosLinks(user);
                }
                return Ok(users);
            }
            catch(Exception e)
            {
                return HandleGetException(e);

            }
        }

        // GET: api/User/5
        [HttpGet("{id}", Name = nameof(GetUser))]
        public IActionResult GetUser(Guid id)
        {
            try
            {
                User user = LoadEntityById<User>(id);
                return Ok(CreateHateosLinks(user));
            }
            catch(Exception e)
            {
                return HandleGetException(e,$"User with id {id} was not found");
            }
        }


        [HttpGet("/api/vi/User/Role/Id/{roleId}", Name = nameof(GetUsersByRoleId))]
        public IActionResult GetUsersByRoleId(Guid roleId)
        {
            try
            {
                ValidateEntityInDatabase<Role>(roleId);
                IList<User> users = ListEntitiesByKeyValue<User>("RoleId", roleId);
                foreach (User user in users)
                {
                    CreateHateosLinks(user);
                }
                return Ok(users);
            }
            catch (Exception e)
            {
                return HandleGetException(e,$"Role with id {roleId} was not found");
            }
        }

        [HttpGet("/api/v1/User/Role/Name/{name}", Name = nameof(GetUsersByRoleName))]
        public IActionResult GetUsersByRoleName(string name)
        {
            try
            {
                Role role = LoadEntityByKeyValue<Role>("Name", name);  
                IList<User> users = role.Users; 
                
                foreach (User user in users)
                {
                    CreateHateosLinks(user);
                }
                return Ok(users);
            }
            catch (Exception e)
            {
                return HandleGetException(e);
            }
        }


        [HttpGet("/api/v1/User/Waiter/Table/Number/{number}", Name = nameof(GetWaitersByTableNumber))]
        public IActionResult GetWaitersByTableNumber(string number)
        {
            try
            {
                
                string sql = "Select * from [User] where Id in (Select WaiterId from WaiterTable where TableId=(Select Id from DiningTable where Number='"+number+"'))";
                Debug.WriteLine(sql);
                IList<User> waiters = dao.List<User>(sql);
                foreach(User waiter in waiters)
                {
                    CreateHateosLinks(waiter);
                }
                return Ok(waiters);
            }
            catch (Exception e)
            {
                return HandleGetException(e);
            }
        }



        // POST: api/User
        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            try
            {
                ValidateEntityNotInDatabase<User>(user);
                Role role = LoadEntityById<Role>(user.Role.Id);
                user.Role = role;
                user.Role.Users.Add(user);
                user.CreatedOn = DateTime.Now;
                user=dao.Save<User>(user);
                return Ok(CreateHateosLinks(user));
                
            }
            catch (Exception e)
            {
                return HandlePostException(e);

            }
        }

        // PUT: api/User/5
        [HttpPut("{id}", Name = nameof(ModifyUser))]
        public IActionResult  ModifyUser(Guid id, [FromBody] User user)
        {
            try
            {
                ValidateIds<User>(id, user);
                Role role = LoadEntityById<Role>(user.Role.Id);
                user.Role = role;
                user=dao.Merge<User>(user);
                return Ok(CreateHateosLinks(user));

            }
            catch (Exception e)
            {
                return HandlePutException(e);

            }
        }

        // PATCH: api/User/5
        [HttpPatch("{id}", Name = nameof(PartiallyModifyUser))]
        public IActionResult PartiallyModifyUser(Guid id, [FromBody] User user)
        {
            try
            {
                ValidateIds<User>(id, user);
                Role role = LoadEntityById<Role>(user.Role.Id);
                user.Role = role;
                user = dao.Merge<User>(user);
                return Ok(CreateHateosLinks(user));

            }
            catch (Exception e)
            {
                return HandlePatchException(e);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}", Name = nameof(DeleteUser))]
        public IActionResult DeleteUser(Guid id)
        {
            try
            {
                User user = LoadEntityById<User>(id);
                user.Role.Users.Remove(user);
                user.Role = null;
                dao.Delete(user);
                return Ok(user);
            }
            catch (Exception e)
            {
                return HandleDeleteException(e);
            }
        }
   
        private User CreateHateosLinks(User user)
        {
            var idObj = new { id = user.Id };
            user.Links = new List<HateosLink>();
        user.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.GetUser), idObj),
                "self",
                HateosLink.ACTION_GET,HateosLink.MEDIA_TYPE_APPLICATION_JSON));

            user.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.ModifyUser), idObj),
                "user",
                HateosLink.ACTION_PUT, HateosLink.MEDIA_TYPE_APPLICATION_JSON));


            user.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.PartiallyModifyUser), idObj),
                "user",
                HateosLink.ACTION_PATCH,HateosLink.MEDIA_TYPE_URLENCODED));

            user.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.DeleteUser), idObj),
                "user",
                HateosLink.ACTION_DELETE));

            user.Links.Add(new HateosLink(this.urlHelper.Link(nameof(this.GetUser), idObj),
                "self",
                HateosLink.ACTION_GET,HateosLink.MEDIA_TYPE_APPLICATION_JSON));


            user.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.ModifyUser), idObj),
                "self",
                HateosLink.ACTION_PUT,HateosLink.MEDIA_TYPE_URLENCODED));

            user.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.PartiallyModifyUser), idObj),
                "self",
                HateosLink.ACTION_PATCH,HateosLink.MEDIA_TYPE_URLENCODED));

            user.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.DeleteUser), idObj),
                "self",
                HateosLink.ACTION_DELETE));
            return user;
        }
}
}
