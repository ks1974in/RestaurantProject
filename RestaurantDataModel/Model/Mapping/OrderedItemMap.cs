using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using Restaurant.DataModel.Entity; 

namespace Restaurant.DataModel.Mapping {
    
    
    public class OrderedItemMap : ClassMap<OrderedItem> {
        
        public OrderedItemMap() {
			Table("OrderedItem");
			Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
			References(x => x.Order).Column("OrderId");
			References(x => x.Item).Column("ItemId");
			Map(x => x.Quantity).Column("Quantity").Not.Nullable().Precision(3);
			Map(x => x.Price).Column("Price").Not.Nullable().Precision(18);
			Map(x => x.Subtotal).Column("Subtotal").Not.Nullable().Precision(18);
			Map(x => x.Remarks).Column("Remarks").Length(2147483647);
        }
    }
}
