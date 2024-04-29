using System;
namespace wcs.core.common
{
    public enum OutPut
    {
        normal,
        warning,
        error
    }

    public class CConsole
    {
        public static void WriteLine(string message, OutPut type, string alarmCode = "", string taskId = "")
        {
            switch (type)
            {
                case OutPut.normal:
                    {
                        Console.ForegroundColor = ConsoleColor.Green;

                        MessageContainer.instance.Info(message, taskId);
                    }
                    break;
                case OutPut.warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case OutPut.error:
                    {
                        Console.ForegroundColor = ConsoleColor.Red;

                        MessageContainer.instance.Error(message, taskId, alarmCode);
                    }
                    break;
                default:
                    break;
            }
            Console.WriteLine(message);

            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}

