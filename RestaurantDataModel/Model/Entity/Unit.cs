using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Restaurant.DataModel.Entity
{
 
    public class Unit: Entity, IEntity
    {
        [JsonSerializeAttribute]
        public virtual Guid Id { get; set; }
        [JsonSerializeAttribute]
        public virtual string Name { get; set; }
        [JsonSerializeAttribute]
        public virtual string Code { get; set; }
        [JsonIgnore]
        public virtual IList<Item> Items { get; set; }
    }
}
