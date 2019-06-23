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
    public class ItemController : ApiCrudController
    {
      
        public ItemController(ISessionFactory sessionFactory, IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor) : base(sessionFactory, urlHelperFactory, actionContextAccessor) { }

        // GET: api/Item
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                string sql = "Select * from Item Order By Code";
                IList<Item> Items = dao.List<Item>(sql);
                foreach (Item Item in Items)
                {
                    
                    CreateHateosLinks(Item);
                }
                return Ok(Items);
            }
            catch(Exception e)
            {
                return HandleGetException(e);
            }
        }

        // GET: api/Item/5
        [HttpGet("{id}", Name = nameof(GetItem))]
        public IActionResult GetItem(Guid id)
        {
            try
            {
                Item item = LoadEntityById<Item>(id);
                return Ok(CreateHateosLinks(item));
            }
            catch(Exception e)
            {
                return HandleGetException(e);
            }
        }


        [HttpGet("/api/v1/Item/Category/{categoryId}", Name = nameof(GetItemsByCategory))]
        public IActionResult GetItemsByCategory(Guid categoryId)
        {
            try
            {
                Category category = LoadEntityById<Category>(categoryId);
                IList<Item>items = category.Items;
                foreach(Item item in items)
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


        // POST: api/Item
        [HttpPost]
        public IActionResult Post([FromBody] Item item)
        {
            try
            {
                ValidateEntityNotInDatabase<Item>(item);
                Category category = LoadEntityById<Category>(item.Category.Id);
                Unit uom = LoadEntityById<Unit>(item.Unit.Id);
                
                item.Category = category;
                item.Category.Items.Add(item);
                item.Unit = uom;
                uom.Items.Add(item);
                item=dao.Save<Item>(item);
                return Ok(CreateHateosLinks(item));
                
            }
            
            catch (Exception e)
            {
                return HandlePostException(e);
            }
        }

        // PUT: api/Item/5
        [HttpPut("{id}", Name = nameof(ModifyItem))]
        public IActionResult  ModifyItem(Guid id, [FromBody] Item item)
        {
            try
            {
                ValidateIds<Item>(id, item);
                ValidateEntityInDatabase<Item>(id);
                Category category = LoadEntityById<Category>(item.Category.Id);
                item.Category =category;
                item.Category.Items.Add(item);
                item=dao.Merge<Item>(item);
                return Ok(CreateHateosLinks(item));

            }
            
            catch (Exception e)
            {
                return HandlePutException(e);
            }
        }

        // PATCH: api/Item/5
        [HttpPatch("{id}", Name = nameof(PartiallyModifyItem))]
        public IActionResult PartiallyModifyItem(Guid id, [FromBody] Item item)
        {
            try
            {
                ValidateIds<Item>(id, item);
                ValidateEntityInDatabase<Item>(id);
                Category category = LoadEntityById<Category>(item.Category.Id);
                item.Category = category;
                item.Category.Items.Add(item);
                item = dao.Merge<Item>(item);
                return Ok(CreateHateosLinks(item));
            }
            catch (Exception e)
            {
                return HandlePatchException(e);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}", Name = nameof(DeleteItem))]
        public IActionResult DeleteItem(Guid id)
        {
            try
            {
                Item item = dao.Load<Item>(id);
                item.Category.Items.Remove(item);
                item.Category = null;

                item.Unit.Items.Remove(item);
                item.Unit = null;

                dao.Delete(item);
                return Ok(item);
            }
            
            catch (Exception e)
            {
                return HandleDeleteException(e);
            }
        }
   
        private Item CreateHateosLinks(Item Item)
        {
            var idObj = new { id = Item.Id };
            Item.Links = new List<HateosLink>();
        Item.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.GetItem), idObj),
                "self",
                HateosLink.ACTION_GET,HateosLink.MEDIA_TYPE_APPLICATION_JSON));

            Item.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.ModifyItem), idObj),
                "Item",
                HateosLink.ACTION_PUT, HateosLink.MEDIA_TYPE_APPLICATION_JSON));


            Item.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.PartiallyModifyItem), idObj),
                "Item",
                HateosLink.ACTION_PATCH,HateosLink.MEDIA_TYPE_URLENCODED));

            Item.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.DeleteItem), idObj),
                "Item",
                HateosLink.ACTION_DELETE));

            Item.Links.Add(new HateosLink(this.urlHelper.Link(nameof(this.GetItem), idObj),
                "self",
                HateosLink.ACTION_GET,HateosLink.MEDIA_TYPE_APPLICATION_JSON));


            Item.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.ModifyItem), idObj),
                "self",
                HateosLink.ACTION_PUT,HateosLink.MEDIA_TYPE_URLENCODED));

            Item.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.PartiallyModifyItem), idObj),
                "self",
                HateosLink.ACTION_PATCH,HateosLink.MEDIA_TYPE_URLENCODED));

            Item.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.DeleteItem), idObj),
                "self",
                HateosLink.ACTION_DELETE));
            return Item;
        }
}
}
