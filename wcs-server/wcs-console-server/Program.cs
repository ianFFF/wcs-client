using log4net;
using log4net.Config;
using System.Reflection;
using System.Threading;
using wcs.webapi;
using wcs.core;
using wcs.core.common;
using Microsoft.Extensions.DependencyInjection;
using wcs.core.@interface;

namespace wcs_console_server
{
    public class Program
    {
        public static void Main(string[] args)
        {

            CConsole.WriteLine("Wcs console server start", OutPut.normal);

            Exec.instance.Start();

            Thread thread = new Thread(() =>
            {
                var services = new ServiceCollection();

                services.AddTransient<IWebApi, WebApi>();

                ServiceProvider serviceProvider = services.BuildServiceProvider();

                var test = serviceProvider.GetRequiredService<IWebApi>();

                test.Init();
            });

            thread.Start();


            CConsole.WriteLine("-- server running --", OutPut.normal);

            Exec.instance.All_Equipment.ForEach(equipment =>
            {
                if (equipment.type == IEquipmentObject.DDJ || equipment.type == IEquipmentObject.SSX)
                {
                    equipment.CloseConnect();
                }
            });


            //CConsole.WriteLine("堆垛机1断开链接", OutPut.error, "DDJERROR0001");
            //CConsole.WriteLine("堆垛机2断开链接", OutPut.error, "DDJERROR0001");
            //CConsole.WriteLine("堆垛机1恢复链接", OutPut.normal);
            //CConsole.WriteLine("堆垛机2恢复链接", OutPut.normal);
            //CConsole.WriteLine("输送系统1-1断开链接", OutPut.error, "SSXT00001");
            //CConsole.WriteLine("输送系统1-1恢复链接", OutPut.normal);
            //CConsole.WriteLine("数据库连接断开", OutPut.error);
            //CConsole.WriteLine("数据库连接恢复", OutPut.normal);


            Console.ReadLine();
        }
    }
}
