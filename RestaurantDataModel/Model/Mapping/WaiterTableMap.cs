using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using Restaurant.DataModel.Entity; 

namespace Restaurant.DataModel.Mapping {
    
    
    public class WaiterTableMap : ClassMap<WaiterTable> {
        
        public WaiterTableMap() {
			Table("WaiterTable");
			Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
            Map(x => x.WaiterId).Column("WaiterId").Not.Nullable();
            Map(x => x.TableId).Column("TableId").Not.Nullable();
            
        }
    }
}
