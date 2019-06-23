

using Newtonsoft.Json;
using RestaurantApi.Hateos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Restaurant.DataModel.Entity
{
    
    public  class Entity:INotifyPropertyChanged
    {

        
        private PropertyChangedEventHandler propertyChanged;

      
        public Entity()
        {
           
        }
        public virtual event PropertyChangedEventHandler PropertyChanged
        {
            add { propertyChanged += value; }
            remove { propertyChanged -= value; }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.propertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override bool Equals(object obj)
        {
            IEntity entity = obj as IEntity;
            if (entity !=null)
            {
                return entity.Id == ((IEntity)this).Id;
            }
            else
            {
                return false;
            }
            
        }
        public override int GetHashCode()
        {
            return ((IEntity)this).Id.GetHashCode();
        }
        public virtual string ToJson()
        {

            return JsonConvert.SerializeObject(this, Formatting.Indented,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }
        public virtual IList<HateosLink> Links { get; set; }
        
    }
}
