using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Restaurant.DataModel.Entity {
    
    public class Role : Entity,IEntity
    {
        public Role() {
			Users = new List<User>();
        }

        [JsonSerializeAttribute]
        public virtual Guid Id { get; set; }
        [JsonSerializeAttribute]
        [Required]
        [StringLength(50)]
        public virtual string Name { get; set; }
        //[JsonSerialize]
        public virtual IList<User> Users { get; set; }
    }
}
