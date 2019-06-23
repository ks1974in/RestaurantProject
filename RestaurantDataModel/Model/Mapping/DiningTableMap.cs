using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using Restaurant.DataModel.Entity; 

namespace Restaurant.DataModel.Mapping {
    
    
    public class DiningTableMap : ClassMap<DiningTable> {
        
        public DiningTableMap() {
			Table("DiningTable");
			Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
			Map(x => x.Number).Column("Number").Not.Nullable().Unique().Length(50);
			Map(x => x.SeatingCapacity).Column("SeatingCapacity").Not.Nullable().Precision(3);
			Map(x => x.Occupied).Column("Occupied").Not.Nullable();
			HasMany(x => x.Orders).KeyColumn("TableId").Inverse().Cascade.None();
			HasMany(x => x.WaiterTables).KeyColumn("TableId").Inverse().Cascade.None(); 
        }
    }
}
