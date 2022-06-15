using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmasiCase.Service.RabbitMQ
{
    public static class GenericActionMethod
    {
        public static async Task SendMessageViaRabbitMQ(string message, string exchangeName)
        {
            var @event = new GenericActionMessageEvent()
            {
                Message = message
            };

            QueueFactory.SendMessageToExchange(exchangeName, "direct", "AccountExchange", @event);
        }
    }
}
