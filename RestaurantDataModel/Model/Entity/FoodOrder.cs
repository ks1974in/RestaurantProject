using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Diagnostics;

namespace Restaurant.DataModel.Entity {
    
    public class FoodOrder : Entity, IEntity {
      
        public FoodOrder() {
			OrderedItems = new List<OrderedItem>();
        }
        [JsonSerializeAttribute]
        public virtual System.Guid Id { get; set; }
        [JsonSerializeAttribute]
        public virtual DiningTable Table { get; set; }
        [JsonSerializeAttribute]
        [Required]
        public virtual User Waiter { get; set; }

        [JsonSerializeAttribute]
        [Required]
        public virtual int OrderNumber { get; set; }
        [JsonSerializeAttribute]
        [Required]
        public virtual DateTime Date { get; set; }
        [JsonSerializeAttribute]
        [Required]
        public virtual bool Completed { get; set; }
        [JsonSerializeAttribute]
        [Required]
        public virtual bool Billed { get; set; }
        [JsonSerializeAttribute]
        [Required]
        public virtual decimal Amount { get; set; }
        [JsonSerializeAttribute]
        [Required]
        public virtual decimal Taxes { get; set; }
        [JsonSerializeAttribute]
        [Required]
        public virtual decimal Subtotal { get; set; }
        [JsonSerializeAttribute]
        [Required]
        public virtual decimal Discount { get; set; }
        [JsonSerializeAttribute]
        [Required]
        public virtual decimal Total { get; set; }
        [JsonSerializeAttribute]
        [StringLength(2147483647)]
        public virtual string Remarks { get; set; }
        [JsonSerializeAttribute]
        public virtual IList<OrderedItem> OrderedItems { get; set; }
        public virtual void Compute()
        {

            OrderedItems.ToList<OrderedItem>().ForEach(item => item.Compute());


            Amount = OrderedItems.Sum(x => x.Subtotal);
            Discount = Decimal.Multiply(Amount, new decimal(0.05));
            Subtotal = Amount - Discount;
            Taxes = Decimal.Multiply(Amount, new decimal(0.12));
            Total = Subtotal + Taxes;
            string msg = $"Computations\nAmount:'{Amount}'\n:Discount:'{Discount}'\nSubTotal:'{Subtotal}'\nTaxes:'{ Taxes}'\n'Total'{Total}'";
            Debug.WriteLine(msg);
        }
    }
}