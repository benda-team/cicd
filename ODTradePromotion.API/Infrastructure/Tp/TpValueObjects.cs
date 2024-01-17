using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Infrastructure.Tp
{
    public class TpValueObjects
    {
        public T ToValueObjects<T>() where T : class, new()
        {
            T destination = new T();
            foreach (PropertyInfo srcProperty in this.GetType().GetProperties())
            {
                if (srcProperty.GetGetMethod().IsVirtual) // Do not clone virtual efcore navigation property due to not expected entity tracking issue
                    continue;

                foreach (PropertyInfo destProperty in destination.GetType().GetProperties())
                {
                    if (destProperty.Name == srcProperty.Name)
                    {
                        destProperty.SetValue(destination, srcProperty.GetValue(this));
                    }
                }
            }

            return destination;
        }
    }
}
