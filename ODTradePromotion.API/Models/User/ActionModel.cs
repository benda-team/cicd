using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.User
{
    public class ActionModel : BaseActionModel
    {
        public bool IsPrincipal { get; set; }
        public string Icon { get; set; }
        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                ActionModel p = (ActionModel)obj;
                return Id == p.Id;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class BaseActionModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

    }
}
