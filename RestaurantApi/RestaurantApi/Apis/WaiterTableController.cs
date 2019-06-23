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
    public class WaiterTableController : ApiCrudController
    {
        
        public WaiterTableController(ISessionFactory sessionFactory, IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor) : base(sessionFactory, urlHelperFactory, actionContextAccessor) { }


        // POST: api/WaiterTable
        [HttpPost("{waiterId}", Name = nameof(Post))]
        public IActionResult Post(Guid waiterId,[FromBody] DiningTable[] tables)
        {
            try
            {
                User waiter = LoadEntityById<User>(waiterId);
                waiter.WaiterTables.Clear();
                string sqlClear = $"Delete from WaiterTable where WaiterId='{waiterId}'";
                dao.Execute(sqlClear);
                waiter=dao.Merge<User>(waiter);

                foreach (DiningTable table in tables)
                {
                    WaiterTable waiterTable = new WaiterTable(waiter, table);
                    waiter.WaiterTables.Add(waiterTable);
                    dao.Save<WaiterTable>(waiterTable);
                    CreateHateosLinks(table);
                }
                _=dao.Merge<User>(waiter);
                return Ok(tables);
                
            }
            catch (Exception e)
            {
                return HandlePostException(e);
            }
        }

      
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}", Name = nameof(DeleteWaiterTable))]
        public IActionResult DeleteWaiterTable(Guid id)
        {
            try
            {
                WaiterTable table =LoadEntityById<WaiterTable>(id);
                dao.Delete(table);
                return Ok(table);
            }
            catch (Exception e)
            {
                return HandleDeleteException(e);
            }
        }
   
        

        private void CreateHateosLinks(DiningTable table)
        {
            var idObj = new { id = table.Id };
            table.Links = new List<HateosLink>();
      
         
            table.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.DeleteWaiterTable), idObj),
                "table",
                HateosLink.ACTION_DELETE));

         
            table.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.DeleteWaiterTable), idObj),
                "self",
                HateosLink.ACTION_DELETE));
           
        }
    }
}
