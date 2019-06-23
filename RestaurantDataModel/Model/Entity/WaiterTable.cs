using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Restaurant.DataModel.Entity {
    
    public class WaiterTable : Entity,IEntity
    {
       
        public WaiterTable() { }
        public WaiterTable(User waiter,DiningTable diningTable)
        {
            this.Id = Guid.NewGuid();
            this.WaiterId = waiter.Id;
            this.TableId = diningTable.Id;
          
        }
        [JsonSerializeAttribute]
        public virtual System.Guid Id { get; set; }
        [JsonSerializeAttribute]
        public virtual System.Guid WaiterId { get; set; }
        [JsonSerializeAttribute]
        public virtual System.Guid TableId { get; set; }
        
        #region NHibernate Composite Key Requirements
        public override bool Equals(object other) {
			if (other == null) return false;
			var t = other as WaiterTable;
			if (t == null) return false;
			if (WaiterId == t.WaiterId
			 && TableId == t.TableId)
				return true;

			return false;
        }
        public override int GetHashCode() {
			int hash = GetType().GetHashCode();
			hash = (hash * 397) ^ WaiterId.GetHashCode();
			hash = (hash * 397) ^ TableId.GetHashCode();

			return hash;
        }
        #endregion
    }
}
