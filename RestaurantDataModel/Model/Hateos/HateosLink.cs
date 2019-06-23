using Restaurant.DataModel.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApi.Hateos
{
    public class HateosLink
    {
        public readonly static string ACTION_GET = "GET";
        public readonly static string POST = "POST";
        public readonly static string ACTION_PUT = "PUT";
        public readonly static string ACTION_PATCH = "PATCH";
        public readonly static string ACTION_DELETE = "DELETE";
        
        public readonly static string MEDIA_TYPE_APPLICATION_JSON = "application/json";
        public readonly static string MEDIA_TYPE_URLENCODED= "application/x-www-form-urlencoded";
        
        private readonly IList<string> _types = new List<string>();
        [JsonSerializeAttribute]
        public string Href { get; private set; }
        [JsonSerializeAttribute]
        public string Rel { get; private set; }
        [JsonSerializeAttribute]
        public string Action { get; private set; }
        [JsonSerializeAttribute]
        public IList<string>Types { get=>_types; }

        public HateosLink() { }

        public HateosLink(string href, string rel, string action)
        {
            this.Href = href;
            this.Rel = rel;
            this.Action = action;
            
        }
        public HateosLink(string href, string rel, string action,string type):this(href, rel, action)
        {   
            _types.Add(type);
          
        }
        public HateosLink(string href, string rel, string action, string[] types) : this(href, rel, action)
        {
            _ = types.All(x => { _types.Add(x); return true; });
        }
    }
}
