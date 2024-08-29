using RabbitMQ.Client;
using System.Text;

public class MessageSender
{
	public void SendMessage(string message)
	{
		var factory = new ConnectionFactory() { HostName = "localhost" };
		using (var connection = factory.CreateConnection())
		using (var channel = connection.CreateModel())
		{
			channel.QueueDeclare(queue: "task_queue",
								 durable: false,
								 exclusive: false,
								 autoDelete: false,
								 arguments: null);

			var body = Encoding.UTF8.GetBytes(message);
			channel.BasicPublish(exchange: "",
								 routingKey: "task_queue",
								 basicProperties: null,
								 body: body);
		}
	}
}
