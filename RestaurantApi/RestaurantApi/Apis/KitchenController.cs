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
using RestaurantApi.ComputedModels;
using RestaurantApi.Hateos;
using RestaurantApi.Support;

namespace RestaurantApi.Apis
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class KitchenController : ApiControllerBase
    {


        public KitchenController(ISessionFactory sessionFactory) : base(sessionFactory) { }

        [HttpGet("/api/v1/Kitchen/ScreenData/{date}", Name = nameof(GetScreenDataByDate))]
        public IActionResult GetScreenDataByDate(DateTime date)
        {
            try
            {
                ScreenModel model = new ScreenModel(date,dao);
                string json = model.ToJson();
                Debug.WriteLine(json);
                return Ok(model);
            }
            catch (Exception e)
            {
                return HandleGetException(e);
            }
        }
    }
}
