using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using Restaurant.DataModel.Entity; 

namespace Restaurant.DataModel.Mapping {
    
    
    public class ItemMap : ClassMap<Item> {
        
        public ItemMap() {
			Table("Item");
			Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
			References(x => x.Category).Column("CategoryId");
            References(x => x.Unit).Column("UnitId");
            Map(x => x.Name).Column("Name").Not.Nullable().Length(50);
            Map(x => x.Code).Column("Code").Not.Nullable().Length(10);
            Map(x => x.Price).Column("Price").Not.Nullable().Precision(18);
			Map(x => x.Available).Column("Available").Not.Nullable();
            Map(x => x.Description).Column("Description").Length(2147483647);
			HasMany(x => x.OrderedItems).KeyColumn("ItemId").Inverse().Cascade.None();
        }
    }
}
