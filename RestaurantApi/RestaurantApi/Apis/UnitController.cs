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
    public class UnitController : ApiCrudController
    {
        
        
        public UnitController(ISessionFactory sessionFactory, IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor) : base(sessionFactory, urlHelperFactory, actionContextAccessor) { }
        // GET: api/unit
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                IList<Unit> Units = dao.List<Unit>();
                foreach (Unit unit in Units)
                {
                    CreateHateosLinks(unit);
                }
                return Ok(Units);
            }
            catch(Exception e)
            {
                return HandleGetException(e);
            }
        }

        // GET: api/unit/5
        [HttpGet("{id}", Name = nameof(GetUnit))]
        public IActionResult GetUnit(Guid id)
        {
            try
            {
                Unit unit = LoadEntityById<Unit>(id);
                return Ok(CreateHateosLinks(unit));
            }
            catch(Exception e)
            {
                return HandleGetException(e);
            }
        }

        // POST: api/unit
        [HttpPost]
        public IActionResult Post([FromBody] Unit unit)
        {
            try
            {
                ValidateEntityNotInDatabase<Unit>(unit);
                unit=dao.Save<Unit>(unit);
                return Ok(CreateHateosLinks(unit));
                
            }
            catch (Exception e)
            {
                return HandlePostException(e);
            }
        }

        // PUT: api/unit/5
        [HttpPut("{id}", Name = nameof(ModifyUnit))]
        public IActionResult  ModifyUnit(Guid id, [FromBody] Unit unit)
        {
            try
            {
                ValidateIds<Unit>(id, unit);
                ValidateEntityInDatabase<Unit>(id);
                dao.Merge<Unit>(unit);
                return Ok(CreateHateosLinks(unit));

            }
            catch (Exception e)
            {
                return HandlePutException(e);
            }
        }

        // PATCH: api/unit/5
        [HttpPatch("{id}", Name = nameof(PartiallyModifyUnit))]
        public IActionResult PartiallyModifyUnit(Guid id, [FromBody] Unit unit)
        {
            try
            {

                ValidateIds<Unit>(id, unit);
                ValidateEntityInDatabase<Unit>(id);
                dao.Merge<Unit>(unit);
                return Ok(CreateHateosLinks(unit));

            }
            catch (Exception e)
            {
                return HandlePatchException(e);            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}", Name = nameof(DeleteUnit))]
        public IActionResult DeleteUnit(Guid id)
        {
            try
            {
                Unit unit = LoadEntityById<Unit>(id);
                dao.Delete(unit);
                return Ok(unit);
            }
            catch (Exception e)
            {
                return HandleDeleteException(e);
            }
        }
   
        private Unit CreateHateosLinks(Unit unit)
        {
            var idObj = new { id = unit.Id };
            unit.Links = new List<HateosLink>();
        unit.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.GetUnit), idObj),
                "self",
                HateosLink.ACTION_GET,HateosLink.MEDIA_TYPE_APPLICATION_JSON));

            unit.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.ModifyUnit), idObj),
                "unit",
                HateosLink.ACTION_PUT, HateosLink.MEDIA_TYPE_APPLICATION_JSON));


            unit.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.PartiallyModifyUnit), idObj),
                "unit",
                HateosLink.ACTION_PATCH,HateosLink.MEDIA_TYPE_URLENCODED));

            unit.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.DeleteUnit), idObj),
                "unit",
                HateosLink.ACTION_DELETE));

            unit.Links.Add(new HateosLink(this.urlHelper.Link(nameof(this.GetUnit), idObj),
                "self",
                HateosLink.ACTION_GET,HateosLink.MEDIA_TYPE_APPLICATION_JSON));


            unit.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.ModifyUnit), idObj),
                "self",
                HateosLink.ACTION_PUT,HateosLink.MEDIA_TYPE_URLENCODED));

            unit.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.PartiallyModifyUnit), idObj),
                "self",
                HateosLink.ACTION_PATCH,HateosLink.MEDIA_TYPE_URLENCODED));

            unit.Links.Add(
                new HateosLink(this.urlHelper.Link(nameof(this.DeleteUnit), idObj),
                "self",
                HateosLink.ACTION_DELETE));
            return unit;
        }
}
}
