using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinGPS
{
    public interface ILocationManager
    {
        /// <summary>
        /// 启用位置监听服务
        /// </summary>
        void StartMonitoringSignificantLocationChanges();

        /// <summary>
        /// 停用位置监听
        /// </summary>
        void StopMonitoringSignificantLocationChanges();

        /// <summary>
        /// 坐标更改事件
        /// </summary>
        /// <param name="GetLocation"></param>
        void LocationChang(Action<double,double> GetLocation);

        List<double> GetLocation();
    }
}
