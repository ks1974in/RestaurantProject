using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using Restaurant.DataModel.Entity; 

namespace Restaurant.DataModel.Mapping {
    
    
    public class UserSessionMap : ClassMap<UserSession> {
        
        public UserSessionMap() {
			Table("UserSession");
			Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
			References(x => x.User).Column("UserId");
			Map(x => x.Start).Column("Start").Not.Nullable();
			Map(x => x.End).Column("`End`");
			Map(x => x.Active).Column("Active").Not.Nullable();
        }
    }
}
