using System;
using Microsoft.Azure.Devices;
using System.Text;
using System.Threading.Tasks;
using Message = Microsoft.Azure.Devices.Message;

namespace project
{
    class Program
    {
        private static ServiceClient serviceClient;
        private static string cnString = Keys.connectionString;
        private static string targetDevice = "Blazor";

        static void Main(string[] args)
        {
            Console.WriteLine("Starting Cloud-to-Device message service\n");
            serviceClient = ServiceClient.CreateFromConnectionString(cnString);

            while (true)
            {
                SendCloudToDeviceMessageAsync().Wait();
                // Random wait time between 3 and 7 seconds
                Task.Delay(new Random().Next(3, 8) * 1000).Wait();
            }
        }

        private async static Task SendCloudToDeviceMessageAsync()
        {
            string messageString = GenerateMessage();
            Console.WriteLine(messageString);
            Message message = new Message(Encoding.ASCII.GetBytes(messageString));
            await serviceClient.SendAsync(targetDevice, message);
        }

        private static string GenerateMessage()
        {
            CloudMessage message = new CloudMessage();
            message.PumpFlow = Math.Round(new Random().Next(0, 13) + new Random().NextDouble(), 2);
            message.PumpHead = Math.Round(new Random().Next(0, 20) + new Random().NextDouble(), 2);

            return message.ToString();
        }
    }

    class CloudMessage
    {
        public double PumpFlow { get; set; }
        public double PumpHead { get; set; }

        public CloudMessage()
        {
            PumpFlow = 0;
            PumpHead = 0;
        }

        public override string ToString()
        {
            return "{\"PumpFlow\":\"" + PumpFlow + "\",\"PumpHead\":\"" + PumpHead + "\"}";
        }
    }
}
