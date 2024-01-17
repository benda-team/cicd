using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.User
{
    public class AssignPermissionModel
    {
        public bool IsPrincipal { get; set; }
        public bool IsRDOS { get; set; }
        public bool IsVisible { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; } //FeatureCode
        public virtual List<ActionModel> Actions { get; set; }
        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                AssignPermissionModel p = (AssignPermissionModel)obj;
                return Id == p.Id;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
