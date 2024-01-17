using Sys.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.User
{
    public class RoleModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
        public bool IsPrincipal { get; set; }
        public string FeatureAction { get; set; }
        public string DynamicFieldValue { get; set; }
        public List<AssignPermissionModel> FeatureActions
        {
            get
            {
                if (FeatureAction != null && FeatureAction != string.Empty)
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<List<AssignPermissionModel>>(FeatureAction);
                else return new List<AssignPermissionModel>();
            }
            set
            {
                FeatureAction = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            }
        }
        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                RoleModel p = (RoleModel)obj;
                return Id == p.Id;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    public class RoleListModel
    {
        public List<RoleModel> Items { get; set; }
        public MetaData MetaData { get; set; }
    }
}
