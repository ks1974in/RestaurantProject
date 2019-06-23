using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Restaurant.DataModel.Entity {
    
    public class DiningTable : Entity, IEntity {
        
        public DiningTable() {
			Orders = new List<FoodOrder>();
			WaiterTables = new List<WaiterTable>();
        }
        [JsonSerializeAttribute]
        public virtual System.Guid Id { get; set; }
        [JsonSerializeAttribute]
        [Required]
        [StringLength(50)]
        public virtual string Number { get; set; }

        [JsonSerializeAttribute]
        [Required]
        public virtual int SeatingCapacity { get; set; }
        [JsonSerializeAttribute]
        [Required]
        public virtual bool Occupied { get; set; }

        [JsonSerializeAttribute]
        public virtual IList<FoodOrder> Orders { get; set; }
        [JsonSerializeAttribute]
        public virtual IList<WaiterTable> WaiterTables { get; set; }
    }
}
