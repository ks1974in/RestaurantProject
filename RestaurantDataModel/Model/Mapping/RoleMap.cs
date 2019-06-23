using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using Restaurant.DataModel.Entity; 

namespace Restaurant.DataModel.Mapping {
    
    
    public class RoleMap : ClassMap<Role> {
        
        public RoleMap() {
			Table("`Role`");
			Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
			Map(x => x.Name).Column("Name").Not.Nullable().Unique().Length(50);
            HasMany(x => x.Users).KeyColumn("RoleId").Inverse().Cascade.All(); 
        }
    }
}
