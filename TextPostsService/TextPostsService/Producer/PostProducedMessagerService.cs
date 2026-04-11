using Confluent.Kafka;

namespace TextPostsService.Producer
{
    public class PostProducedMessagerService : IPostProducedMessagerService
    {
        IProducer<string, int> _producer;
        public PostProducedMessagerService()
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };
            _producer = new ProducerBuilder<string, int>(config).Build();
        }
        public void SendPostCreatedMessageAsync(string topic, int userId)
        {
            _producer.ProduceAsync(topic, new Message<string, int> { Value = userId,Key="userId" });
        }
    }
}
