using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using Restaurant.DataModel.Entity; 

namespace Restaurant.DataModel.Mapping {
    
    
    public class CategoryMap : ClassMap<Category> {
        
        public CategoryMap() {
			Table("Category");
			Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
            Map(x => x.Code).Column("Code").Not.Nullable().Unique().Length(10);
            Map(x => x.Name).Column("Name").Not.Nullable().Unique().Length(50);
			Map(x => x.Description).Column("Description").Length(2147483647);
            HasMany(x => x.Items).KeyColumn("CategoryId").Inverse().Cascade.All();
        }
    }
}
