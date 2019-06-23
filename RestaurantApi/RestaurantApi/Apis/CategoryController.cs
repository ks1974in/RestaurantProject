using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using NHibernate;
using Restaurant.DataModel.DataFramework;
using Restaurant.DataModel.Entity;
using RestaurantApi.Hateos;
using RestaurantApi.Support;
using static RestaurantApi.Support.ControllerExtensionMethods;

namespace RestaurantApi.Apis
{

    [Route("api/v1/[controller]")]
    [ApiController]

    public class CategoryController : ApiCrudController
    {



        public CategoryController(ISessionFactory sessionFactory, IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor) : base(sessionFactory, urlHelperFactory, actionContextAccessor) { }
        
        // GET: api/Category
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                string sql = "Select * from Category Order By Code";
                IList<Category> categorys = dao.List<Category>(sql);
                foreach (Category category in categorys)
                {
                  
                    CreateHateosLinks(category);
                }

                return Ok(categorys.OrderBy(category=>category.Code));
            }
            catch (Exception e)
            {
                return HandleGetException(e);
            }
        }

        // GET: api/Category/5
        [HttpGet("{id}", Name = nameof(GetCategory))]
        public IActionResult GetCategory(Guid id)
        {
            try
            {
                Category category = LoadEntityById<Category>(id);
                return Ok(CreateHateosLinks(category));
            }
            catch(Exception e)
            {
                return HandleGetException(e);
            }
        }

        // POST: api/Category
        [HttpPost]
        public IActionResult Post([FromBody] Category category)
        {
            try
            {
                ValidateEntityNotInDatabase<Category>(category);
                category=dao.Save<Category>(category);
                return Ok(CreateHateosLinks(category));
                
            }
            catch (Exception e)
            {
                return HandlePostException(e);
            }
        }

        // PUT: api/Category/5
        [HttpPut("{id}", Name = nameof(ModifyCategory))]
        public IActionResult  ModifyCategory(Guid id, [FromBody] Category category)
        {
            try
            {
                ValidateIds<Category>(id, category);
                ValidateEntityInDatabase<Category>(id);
                category = dao.Merge<Category>(category);
                return Ok(CreateHateosLinks(category));

            }
            catch (Exception e)
            {
                return HandlePutException(e);
            }
        }

        // PATCH: api/Category/5
        [HttpPatch("{id}", Name = nameof(PartiallyModifyCategory))]
        public IActionResult PartiallyModifyCategory(Guid id, [FromBody] Category category)
        {
            try
            {
                ValidateIds<Category>(id, category);
                ValidateEntityInDatabase<Category>(id);
                category = dao.Merge<Category>(category);
                return Ok(CreateHateosLinks(category));

            }
            catch (Exception e)
            {
                return HandlePatchException(e);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}", Name = nameof(DeleteCategory))]
        public IActionResult DeleteCategory(Guid id)
        {
            try
            {
                Category category =LoadEntityById<Category>(id);
                category.Items.ToList<Item>().ForEach(item => 
                {
                item.OrderedItems.ToList<OrderedItem>().ForEach(orderedItem =>
                {
                    orderedItem.Order = null;
                    orderedItem.Item = null;
                });
                    item.OrderedItems.Clear();

                    item.Category.Items.Remove(item);
                    item.Category = null;
                });
                category.Items.Clear();


                string sqlOrderedItems = $"Delete from OrderedItem where ItemId in (Select Id from Item where CategoryId='{id}')";
                Debug.WriteLine(sqlOrderedItems);
                dao.Execute(sqlOrderedItems);

                string sqlItems = $"Delete from Item where CategoryId='{ id}'";
                Debug.WriteLine(sqlItems);
                dao.Execute(sqlItems);
                
                category = LoadEntityById<Category>(id);
                Debug.WriteLine(category.ToJson());
                dao.Delete(category);
                return Ok(category);
            }
            catch (Exception e)
            {
                return HandleDeleteException(e);
            }
        }
   
        private Category CreateHateosLinks(Category Category)
        {
            var idObj = new { id = Category.Id };
            Category.Links = new List<HateosLink>();
        Category.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.GetCategory), idObj),
                "self",
                HateosLink.ACTION_GET,HateosLink.MEDIA_TYPE_APPLICATION_JSON));

            Category.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.ModifyCategory), idObj),
                "Category",
                HateosLink.ACTION_PUT, HateosLink.MEDIA_TYPE_APPLICATION_JSON));


            Category.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.PartiallyModifyCategory), idObj),
                "Category",
                HateosLink.ACTION_PATCH,HateosLink.MEDIA_TYPE_URLENCODED));

            Category.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.DeleteCategory), idObj),
                "Category",
                HateosLink.ACTION_DELETE));

            Category.Links.Add(new HateosLink(this.urlHelper.Link(nameof(this.GetCategory), idObj),
                "self",
                HateosLink.ACTION_GET,HateosLink.MEDIA_TYPE_APPLICATION_JSON));


            Category.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.ModifyCategory), idObj),
                "self",
                HateosLink.ACTION_PUT,HateosLink.MEDIA_TYPE_URLENCODED));

            Category.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.PartiallyModifyCategory), idObj),
                "self",
                HateosLink.ACTION_PATCH,HateosLink.MEDIA_TYPE_URLENCODED));

            Category.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.DeleteCategory), idObj),
                "self",
                HateosLink.ACTION_DELETE));
            return Category;
        }
}
}
