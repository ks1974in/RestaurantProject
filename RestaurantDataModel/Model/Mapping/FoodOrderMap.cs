using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using Restaurant.DataModel.Entity; 

namespace Restaurant.DataModel.Mapping {
    
    
    public class FoodOrderMap : ClassMap<FoodOrder> {
        
        public FoodOrderMap() {
			Table("FoodOrder");
			Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
			References(x => x.Table).Column("TableId");
            References(x => x.Waiter).Column("WaiterId");
            Map(x => x.OrderNumber).Column("OrderNumber").Not.Nullable().Unique().Precision(10);
			Map(x => x.Date).Column("Date").Not.Nullable().Unique();
			Map(x => x.Completed).Column("Completed").Not.Nullable();
			Map(x => x.Billed).Column("Billed").Not.Nullable();
			Map(x => x.Amount).Column("Amount").Not.Nullable().Precision(18);
			Map(x => x.Taxes).Column("taxes").Not.Nullable().Precision(18);
			Map(x => x.Subtotal).Column("SubTotal").Not.Nullable().Precision(18);
			Map(x => x.Discount).Column("Discount").Not.Nullable().Precision(18);
			Map(x => x.Total).Column("Total").Not.Nullable().Precision(18);
			Map(x => x.Remarks).Column("Remarks").Length(2147483647);
			HasMany(x => x.OrderedItems).KeyColumn("OrderId").Inverse().Cascade.None();
        }
    }
}
