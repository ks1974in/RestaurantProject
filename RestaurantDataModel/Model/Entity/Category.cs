using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Restaurant.DataModel.Entity {
    
    public class Category : Entity,IEntity
    {
        public Category() {
			Items = new List<Item>();
        }
        [JsonSerializeAttribute]
        public virtual System.Guid Id { get; set; }
        [JsonSerializeAttribute]
        [Required]
        [StringLength(50)]
        public virtual string Name { get; set; }
        [JsonSerializeAttribute]
        [StringLength(2147483647)]
        public virtual string Description { get; set; }
       
        [JsonSerializeAttribute]
        public virtual string Code { get; set; }


        //[JsonSerialize]
        public virtual IList<Item> Items { get; set; }
       
    }
}
