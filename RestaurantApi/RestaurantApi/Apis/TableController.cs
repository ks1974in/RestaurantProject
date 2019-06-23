using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;
using NHibernate;
using Restaurant.DataModel.DataFramework;
using Restaurant.DataModel.Entity;
using RestaurantApi.Hateos;
using RestaurantApi.Support;

namespace RestaurantApi.Apis
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TableController : ApiCrudController
    {
       
       
        public TableController(ISessionFactory sessionFactory, IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor) : base(sessionFactory, urlHelperFactory, actionContextAccessor) { }
        // GET: api/Table
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                IList<DiningTable> Tables = dao.List<DiningTable>();
                foreach (DiningTable Table in Tables)
                {
                    CreateHateosLinks(Table);
                }
                return Ok(Tables);
            }
            catch (Exception e)
            {
                return HandleGetException(e);

            }
        }

        // GET: api/Table/5
        [HttpGet("{id}", Name = nameof(GetTable))]
        public IActionResult GetTable(Guid id)
        {
            try
            {
                DiningTable table = LoadEntityById<DiningTable>(id);
                return Ok(CreateHateosLinks(table));
            }
            catch (Exception e)
            {
                return HandleGetException(e);
            }
        }

        // POST: api/Table
        [HttpPost]
        public IActionResult Post([FromBody] DiningTable table)
        {
            try
            {
                ValidateEntityNotInDatabase<DiningTable>(table);
                table=dao.Save<DiningTable>(table);
                return Ok(CreateHateosLinks(table));

            }
            catch (Exception e)
            {
                return HandlePostException(e);
            }
        }

        private void LoadChildObjects(DiningTable table)
        {
            foreach(FoodOrder order in table.Orders)
            {
               Debug.WriteLine("Before:"+order.ToJson());
               order.Table = table;
               dao.Refresh<FoodOrder>(order);
                Debug.WriteLine("After:"+order.ToJson());
            }
            
            foreach(WaiterTable waiterTable in table.WaiterTables)
            {
                Debug.WriteLine("Before:"+waiterTable.ToJson());
                waiterTable.TableId = table.Id;
                dao.Refresh<WaiterTable>(waiterTable);
                Debug.WriteLine("After:" + waiterTable.ToJson());
            }
            
        }

        // PUT: api/Table/5
        [HttpPut("{id}", Name = nameof(ModifyTable))]
        public IActionResult ModifyTable(Guid id, [FromBody] DiningTable table)
        {
            try
            {
                ValidateIds<DiningTable>(id, table);
                ValidateEntityInDatabase<DiningTable>(id);
                LoadChildObjects(table);
                table = dao.Merge<DiningTable>(table);
                return Ok(CreateHateosLinks(table));

            }
            catch (Exception e)
            {
                return HandlePutException(e);
            }
        }

        // PATCH: api/Table/5
        [HttpPatch("{id}", Name = nameof(PartiallyModifyTable))]
        public IActionResult PartiallyModifyTable(Guid id, [FromBody] DiningTable table)
        {
            try
            {
                ValidateIds<DiningTable>(id, table);
                ValidateEntityInDatabase<DiningTable>(id);
                LoadChildObjects(table);
                table = dao.Merge<DiningTable>(table);
                return Ok(CreateHateosLinks(table));

            }
            catch (Exception e)
            {
                return HandlePatchException(e);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}", Name = nameof(DeleteTable))]
        public IActionResult DeleteTable(Guid id)
        {
            try
            {
                DiningTable table = LoadEntityById<DiningTable>(id);
              

                table.Orders.ToList<FoodOrder>().ForEach(order => { order.OrderedItems.Clear(); });
                table.Orders.Clear();
                table.WaiterTables.Clear();

                string sqlOrderedItems = $"Delete from OrderedItem where OrderId in (Select Id from FoodOrder where TableId='{id}')";
                Debug.WriteLine(sqlOrderedItems);
                dao.Execute(sqlOrderedItems);
                string sqlOrders=$"Delete from FoodOrder where TableId='{id}'";
                Debug.WriteLine(sqlOrders);
                dao.Execute(sqlOrders);
                string sqlWaiterTables=$"Delete from WaiterTable where TableId='{id}'";
                Debug.WriteLine(sqlWaiterTables);
                dao.Execute(sqlWaiterTables);

                

                


                table = LoadEntityById<DiningTable>(id);
                Debug.WriteLine(table.ToJson());
                dao.Delete(table);
                return Ok(table);
            }
            catch (Exception e)
            {
                return HandleDeleteException(e);

            }
        }

        private DiningTable CreateHateosLinks(DiningTable Table)
        {
            var idObj = new { id = Table.Id };
            Table.Links = new List<HateosLink>();
            Table.Links.Add(
                    new HateosLink(this.urlHelper.Link(nameof(this.GetTable), idObj),
                    "self",
                    HateosLink.ACTION_GET, HateosLink.MEDIA_TYPE_APPLICATION_JSON));

            Table.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.ModifyTable), idObj),
                "Table",
                HateosLink.ACTION_PUT, HateosLink.MEDIA_TYPE_APPLICATION_JSON));


            Table.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.PartiallyModifyTable), idObj),
                "Table",
                HateosLink.ACTION_PATCH, HateosLink.MEDIA_TYPE_URLENCODED));

            Table.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.DeleteTable), idObj),
                "Table",
                HateosLink.ACTION_DELETE));

            Table.Links.Add(new HateosLink(this.urlHelper.Link(nameof(this.GetTable), idObj),
                "self",
                HateosLink.ACTION_GET, HateosLink.MEDIA_TYPE_APPLICATION_JSON));


            Table.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.ModifyTable), idObj),
                "self",
                HateosLink.ACTION_PUT, HateosLink.MEDIA_TYPE_URLENCODED));

            Table.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.PartiallyModifyTable), idObj),
                "self",
                HateosLink.ACTION_PATCH, HateosLink.MEDIA_TYPE_URLENCODED));

            Table.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.DeleteTable), idObj),
                "self",
                HateosLink.ACTION_DELETE));
            return Table;
        }
    }
}
