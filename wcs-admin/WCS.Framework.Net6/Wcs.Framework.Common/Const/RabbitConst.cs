using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wcs.Framework.Common.Const
{
   public class RabbitConst
    {
        private const string prefix = "Wcs.Framework.";
        public const  string SMS_Exchange = prefix+"SMS.Exchange";
        public const  string SMS_Queue_Send = prefix+ "SMS.Queue.Send";
    }
}
