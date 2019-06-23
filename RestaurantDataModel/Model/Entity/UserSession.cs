using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Restaurant.DataModel.Entity {
    
    public class UserSession : Entity, IEntity {
        
        [JsonSerializeAttribute]
        public virtual System.Guid Id { get; set; }
        [JsonSerializeAttribute]
        public virtual User User { get; set; }
        [JsonSerializeAttribute]
        [Required]
        public virtual DateTime Start { get; set; }
        [JsonSerializeAttribute]
        public virtual DateTime? End { get; set; }
        [JsonSerializeAttribute]
        [Required]
        public virtual bool Active { get; set; }
    }
}
