using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApi.Support
{
    public class ApiCrudController : ApiControllerBase
    {
        protected readonly IUrlHelper urlHelper;
        protected ApiCrudController(ISessionFactory sessionFactory, IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor) :base(sessionFactory)
        {
            urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }
    }
}
