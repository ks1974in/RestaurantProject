using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using Restaurant.DataModel.Entity; 

namespace Restaurant.DataModel.Mapping {
    
    
    public class UserMap : ClassMap<User> {
        
        public UserMap() {
			Table("`User`");
			Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
			References(x => x.Role).Column("RoleId");
			Map(x => x.UserName).Column("UserName").Not.Nullable().Unique().Length(50);
			Map(x => x.FirstName).Column("FirstName").Not.Nullable().Length(50);
			Map(x => x.LastName).Column("LastName").Not.Nullable().Length(50);
			Map(x => x.Password).Column("Password").Not.Nullable().Length(50);
			Map(x => x.Enabled).Column("Enabled").Not.Nullable();
			Map(x => x.Locked).Column("Locked").Not.Nullable();
			Map(x => x.CreatedOn).Column("CreatedOn").Not.Nullable();
			Map(x => x.MobileNumber).Column("MobileNumber").Not.Nullable().Length(15);
			Map(x => x.EmailAddress).Column("EmailAddress").Not.Nullable().Length(50);
			HasMany(x => x.UserSessions).KeyColumn("UserId").Inverse().Cascade.All();
            HasMany(x => x.Orders).KeyColumn("WaiterId").Inverse().Cascade.None(); 
            HasMany(x => x.WaiterTables).KeyColumn("WaiterId").Inverse().Cascade.None(); 
        }
    }
}
