using Newtonsoft.Json.Serialization;
using Restaurant.DataModel.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Restaurant.DataModel.Entity
{
    public class JsonSerializeAttribute : Attribute { }

    public class JsonPropertiesResolver : DefaultContractResolver
    {
        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            //Return properties that do NOT have the JsonIgnoreSerializationAttribute
            return objectType.GetProperties()
                             .Where(pi => Attribute.IsDefined(pi, typeof(JsonSerializeAttribute)))
                             .ToList<MemberInfo>();
        }
    }
   
}
