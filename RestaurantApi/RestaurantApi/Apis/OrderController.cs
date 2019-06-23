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
    public class OrderController : ApiCrudController
    {
       
        public OrderController(ISessionFactory sessionFactory, IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor) : base(sessionFactory, urlHelperFactory, actionContextAccessor) { }
        // GET: api/FoodOrder
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                IList<FoodOrder> FoodOrders = dao.List<FoodOrder>();
                foreach (FoodOrder FoodOrder in FoodOrders)
                {
                    
                    CreateHateosLinks(FoodOrder);
                }
                return Ok(FoodOrders);
            }
            catch(Exception e)
            {
                return HandleGetException(e);
            }
        }

        // GET: api/FoodOrder/5
        [HttpGet("{id}", Name = nameof(GetFoodOrder))]
        public IActionResult GetFoodOrder(Guid id)
        {
            try
            {
                FoodOrder order = LoadEntityById<FoodOrder>(id);
                return Ok(CreateHateosLinks(order));
            }
            catch(Exception e)
            {
                return HandleGetException(e);
            }
        }

        [HttpGet("/api/v1/Order/OrderedItems/{id}", Name = nameof(GetOrderedItems))]
        public IActionResult GetOrderedItems(Guid id)
        {
            try
            {
                FoodOrder order = LoadEntityById<FoodOrder>(id);
                
                IList<OrderedItem> items = order.OrderedItems;
                foreach(OrderedItem item in items)
                {
                    CreateHateosLinks(item);
                }
                return Ok(items);
            }
            catch (Exception e)
            {
                return HandleGetException(e);
            }
        }

        [HttpPost("/api/v1/Order/OrderedItems/{id}")]
        public IActionResult AddOrderedItem([FromBody] OrderedItem orderedItem)
        {
            try
            {
                FoodOrder order = LoadEntityById<FoodOrder>(orderedItem.Order.Id);
               
                Item item = LoadEntityById<Item>(orderedItem.Item.Id);
             

                string sql = $"Select * from OrderedItem where ItemId='{ item.Id}' and OrderId='{order.Id}'";
                Debug.WriteLine(sql);
                OrderedItem dbItem = dao.Query<OrderedItem>(sql);
                if (dbItem == null)
                {
                    orderedItem.Order = order;
                    orderedItem.Item = item;
                    orderedItem.Compute();
                    order.OrderedItems.Add(orderedItem);
                    dao.SaveOrUpdate(orderedItem);
                }
                else
                {
                    dbItem.Quantity = orderedItem.Quantity;
                    dbItem.Price = orderedItem.Price;
                    dbItem.Compute();
                    order.Compute();
                    _=dao.Merge<OrderedItem>(dbItem);
                }
                order.Compute();
                _=dao.Merge<FoodOrder>(order);

                return Ok(orderedItem);
            }
            catch (Exception e)
            {
                return HandleGetException(e);
            }
        }



        // DELETE: api/ApiWithActions/5
        [HttpDelete("/api/v1/Order/OrderedItems/{id}", Name = nameof(DeleteOrderedItem))]
        public IActionResult DeleteOrderedItem(Guid id)
        {
            try
            {
                OrderedItem item = LoadEntityById<OrderedItem>(id);
                
                FoodOrder order = LoadEntityById<FoodOrder>(item.Order.Id);
               

                order.OrderedItems.Remove(item);
                item.Order = null;



                item.Item.OrderedItems.Remove(item);
                item.Item = null;

                dao.Delete(item);

                order.Compute();
                _=dao.Merge<FoodOrder>(order);

                return Ok(item);
            }

            catch (Exception e)
            {
                return HandleDeleteException(e);
            }
        }


        // POST: api/FoodOrder
        [HttpPost]
        public IActionResult Post([FromBody] FoodOrder order)
        {
            try
            {

                ValidateEntityNotInDatabase<FoodOrder>(order);
                order = LoadChildObjects(order);
                order=dao.Save<FoodOrder>(order);
                return Ok(CreateHateosLinks(order));
                
            }
            
            catch (Exception e)
            {
                return HandlePostException(e);
            }
        }

        // PUT: api/FoodOrder/5
        [HttpPut("{id}", Name = nameof(ModifyOrder))]
        public IActionResult  ModifyOrder(Guid id, [FromBody] FoodOrder order)
        {
            try
            {
                ValidateIds<FoodOrder>(id, order);
                ValidateEntityInDatabase<FoodOrder>(id);
                order = LoadChildObjects(order);
                order = dao.Merge<FoodOrder>(order);
                return Ok(CreateHateosLinks(order));

            }
            
            catch (Exception e)
            {
                return HandlePutException(e);
            }
        }

        // PATCH: api/FoodOrder/5
        [HttpPatch("{id}", Name = nameof(PartiallyModifyFoodOrder))]
        public IActionResult PartiallyModifyFoodOrder(Guid id, [FromBody] FoodOrder order)
        {
            try
            {
                ValidateIds<FoodOrder>(id, order);
                ValidateEntityInDatabase<FoodOrder>(id);
                order = LoadChildObjects(order);
                order=dao.Merge<FoodOrder>(order);
                return Ok(CreateHateosLinks(order));

            }
            catch (Exception e)
            {
                return HandlePatchException(e);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}", Name = nameof(DeleteOrder))]
        public IActionResult DeleteOrder(Guid id)
        {
            try
            {
                FoodOrder order = LoadEntityById<FoodOrder>(id);
                order.Waiter.Orders.Remove(order);
                order.Waiter = null;
                order.Table.Orders.Remove(order);
                order.Table = null;
              

                foreach(OrderedItem item in order.OrderedItems)
                {
                    
                    item.Item.OrderedItems.Remove(item);
                    item.Item = null;
                    item.Order = null;
                }
                order.OrderedItems.Clear();

                string sql = $"Delete from OrderedItem where OrderId='{order.Id}'";
                dao.Execute(sql);
                order=dao.Refresh<FoodOrder>(order);
                Debug.WriteLine(order.ToJson());
                dao.Delete(order);
                return Ok(order);
            }
            
            catch (Exception e)
            {
                return HandleDeleteException(e);
            }
        }
   
        private FoodOrder CreateHateosLinks(FoodOrder FoodOrder)
        {
            var idObj = new { id = FoodOrder.Id };
            FoodOrder.Links = new List<HateosLink>();
        FoodOrder.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.GetFoodOrder), idObj),
                "self",
                HateosLink.ACTION_GET,HateosLink.MEDIA_TYPE_APPLICATION_JSON));

            FoodOrder.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.ModifyOrder), idObj),
                "FoodOrder",
                HateosLink.ACTION_PUT, HateosLink.MEDIA_TYPE_APPLICATION_JSON));


            FoodOrder.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.PartiallyModifyFoodOrder), idObj),
                "FoodOrder",
                HateosLink.ACTION_PATCH,HateosLink.MEDIA_TYPE_URLENCODED));

            FoodOrder.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.DeleteOrder), idObj),
                "FoodOrder",
                HateosLink.ACTION_DELETE));

            FoodOrder.Links.Add(new HateosLink(this.urlHelper.Link(nameof(this.GetFoodOrder), idObj),
                "self",
                HateosLink.ACTION_GET,HateosLink.MEDIA_TYPE_APPLICATION_JSON));


            FoodOrder.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.ModifyOrder), idObj),
                "self",
                HateosLink.ACTION_PUT,HateosLink.MEDIA_TYPE_URLENCODED));

            FoodOrder.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.PartiallyModifyFoodOrder), idObj),
                "self",
                HateosLink.ACTION_PATCH,HateosLink.MEDIA_TYPE_URLENCODED));

            FoodOrder.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.DeleteOrder), idObj),
                "self",
                HateosLink.ACTION_DELETE));
            return FoodOrder;
        }
        private void CreateHateosLinks(OrderedItem item)
        {
            var idObj = new { id = item.Id };
            item.Links = new List<HateosLink>();
            item.Links.Add(
                    new HateosLink(this.urlHelper.Link(nameof(this.GetOrderedItems), idObj),
                    "self",
                    HateosLink.ACTION_GET, HateosLink.MEDIA_TYPE_APPLICATION_JSON));

            
        }
       
        private FoodOrder LoadChildObjects(FoodOrder order)
        {
            User waiter = LoadEntityById<User>(order.Waiter.Id);
            
            DiningTable table = LoadEntityById<DiningTable>(order.Table.Id);
          
            order.Waiter = waiter;
            order.Table = table;
            return order;
        }
    }
}
