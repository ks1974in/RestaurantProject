using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Restaurant.DataModel.Entity {

    public class OrderedItem : Entity, IEntity {
       
        [JsonSerializeAttribute]
        public virtual System.Guid Id { get; set; }
        [JsonSerializeAttribute]
        public virtual FoodOrder Order { get; set; }
        [JsonSerializeAttribute]
        public virtual Item Item { get; set; }
        [JsonSerializeAttribute]
        [Required]
        public virtual byte Quantity { get; set; }
    
            [JsonSerializeAttribute]
        [Required]
        public virtual decimal Price { get; set; }
            [JsonSerializeAttribute]
        [Required]
        public virtual decimal Subtotal { get; set; }
            [JsonSerializeAttribute]
        [StringLength(2147483647)]
        public virtual string Remarks { get; set; }
            public virtual void Compute()
            {
                Subtotal = Decimal.Multiply(Price, Quantity);
            }
        }
}
