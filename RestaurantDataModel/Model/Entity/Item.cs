using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Restaurant.DataModel.Entity {
    
    public class Item : Entity, IEntity {
       
        public Item() {
			OrderedItems = new List<OrderedItem>();
        }
        [JsonSerializeAttribute]
        public virtual System.Guid Id { get; set; }
        [JsonSerializeAttribute]
        public virtual Category Category { get; set; }


        [JsonSerializeAttribute]
        [Required]

        public virtual Unit Unit { get; set; }
        [JsonSerializeAttribute]
        [StringLength(10)]
        public virtual string Code { get; set; }


        [JsonSerializeAttribute]
        [Required]
        [StringLength(50)]
        public virtual string Name { get; set; }
        [JsonSerializeAttribute]
        [Required]
        public virtual decimal Price { get; set; }
        [JsonSerializeAttribute]
        [Required]
        public virtual bool Available { get; set; }
        [JsonSerializeAttribute]
        [StringLength(2147483647)]
        public virtual string Description { get; set; }
        [JsonIgnore]
        public virtual IList<OrderedItem> OrderedItems { get; set; }
        
        
    }
}
