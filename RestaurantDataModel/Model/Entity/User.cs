using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Restaurant.DataModel.Entity {
    
    public class User : Entity,IEntity
    {
         public User() {
			UserSessions = new List<UserSession>();
			WaiterTables = new List<WaiterTable>();
        }
        public User(Guid id)
        {
            Id = id;
        }
        [JsonSerializeAttribute]
        public virtual System.Guid Id { get; set; }
        [JsonSerializeAttribute]
        public virtual Role Role { get; set; }
        [JsonSerializeAttribute]
        [Required]
        [StringLength(50)]
        public virtual string UserName { get; set; }
        [JsonSerializeAttribute]
        [Required]
        [StringLength(50)]
        public virtual string FirstName { get; set; }
        [JsonSerializeAttribute]
        [Required]
        [StringLength(50)]
        public virtual string LastName { get; set; }


        [JsonSerializeAttribute]
        public virtual string Name { get=>FirstName+ " " +LastName; set { _ = value; } }


        [JsonSerializeAttribute]
        [Required]
        [StringLength(50)]
        public virtual string Password { get; set; }
        [JsonSerializeAttribute]
        [Required]
        public virtual bool Enabled { get; set; }
        [JsonSerializeAttribute]
        [Required]
        public virtual bool Locked { get; set; }
        [JsonSerializeAttribute]
        [Required]
        public virtual DateTime CreatedOn { get; set; }
        [JsonSerializeAttribute]
        [Required]
        [StringLength(15)]
        public virtual string MobileNumber { get; set; }
        [JsonSerializeAttribute]
        [Required]
        [StringLength(50)]
        public virtual string EmailAddress { get; set; }

        [JsonIgnore]
        public virtual IList<UserSession> UserSessions { get; set; }
        [JsonSerializeAttribute]
        public virtual IList<WaiterTable> WaiterTables { get; set; }
        [JsonSerializeAttribute]
        public virtual IList<FoodOrder> Orders { get; set; }
    }
}
