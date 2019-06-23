using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DataModel.Entity
{
    public interface IEntity
    {

        System.Guid Id { get; set; }
        string ToJson();
    }

}
