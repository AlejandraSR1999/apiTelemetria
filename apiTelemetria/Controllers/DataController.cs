using apiTelemetria.Models;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiTelemetria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        [HttpPost]
        public async Task<bool> EnviarAsync([FromBody] Data data)
        {

            string connectionString = "Endpoint=sb://queuealejandra.servicebus.windows.net/;SharedAccessKeyName=Enviar;SharedAccessKey=P+OTZO46Jawz+f/ym1JBbW3RCXR0siIchNRoqQzldgE=;EntityPath=colaexamen";
            string queueName = "colaexamen";
            string mensaje = JsonConvert.SerializeObject(data);

            // create a Service Bus client 
            await using (ServiceBusClient client = new ServiceBusClient(connectionString))
            {
                // create a sender for the queue 
                ServiceBusSender sender = client.CreateSender(queueName);

                // create a message that we can send
                ServiceBusMessage message = new ServiceBusMessage(mensaje);

                // send the message
                await sender.SendMessageAsync(message);
                Console.WriteLine($"Sent a single message to the queue: {queueName}");
            }

            return true;
        }
    }
}
