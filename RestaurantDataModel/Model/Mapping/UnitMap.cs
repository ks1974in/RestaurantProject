using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using Restaurant.DataModel.Entity; 

namespace Restaurant.DataModel.Mapping {
    
    
    public class UnitMap : ClassMap<Unit> {
        
        public UnitMap() {
			Table("Unit");
			Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
			Map(x => x.Name).Column("Name").Not.Nullable().Unique().Length(50);
            Map(x => x.Code).Column("Code").Not.Nullable().Unique().Length(10);
            HasMany(x => x.Items).KeyColumn("UnitId").Inverse().Cascade.All();
        }
    }
}
