using Avalon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    public class ActiveLogService : IService
    {
        IActiveLogRepository activeLogRepository;

        public ActiveLogService(IActiveLogRepository activeLogRepository)
        {
            this.activeLogRepository = activeLogRepository;
        }

        public void Active(long userId, string userName, long activePlat)
        {
            long productCode = 0, terminalCode = 0;


            if (activePlat >= 100000000000)
            {
                productCode = activePlat / 100000000;
                terminalCode = (activePlat - (productCode * 100000000)) / 10000;
            }

            var spec = activeLogRepository.CreateSpecification().Where(
                o => o.UserId == userId &&
                o.TerminalCode == terminalCode &&
                o.ProductCode == productCode);

            var count = activeLogRepository.Count(spec);

            if (count == 0)
            {
                var activeLog = new ActiveLog()
                {
                    ProductCode = (int)productCode,
                    TerminalCode = (int)terminalCode,
                    UserId = userId
                };
                activeLogRepository.Create(activeLog);
            }
        }
    }
}
