using Confluent.Kafka;

namespace TextPostsService.Producer
{
    public class PostProducedMessagerService : IPostProducedMessagerService
    {
        IProducer<string, int> _producer;
        ILogger logger;
        public PostProducedMessagerService(ILogger<PostProducedMessagerService>logger)
        {
            this.logger = logger;
            
        }
        public void SendPostCreatedMessageAsync(string topic, int userId)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "kafka:9092",

            };
            _producer = new ProducerBuilder<string, int>(config).Build();
            try
            {
                _producer.ProduceAsync(topic, new Message<string, int> { Value = userId, Key = "userId" }).Wait();
            }
            catch (ProduceException<string, int> e)
            {
                logger.LogInformation(e.Message);
                logger.LogError("Что-то произошло при отправке в Kafka сообщения");
            }
            catch (ArgumentException e)
            {
                logger.LogInformation(e.Message);
                logger.LogError("Что-то произошло при отправке в Kafka сообщения");
            }
            catch (KafkaException e)
            {
                logger.LogInformation(e.Message);
                logger.LogError("Что-то произошло при отправке в Kafka сообщения");
            }
            finally
            {
                _producer.Dispose();
            }
        }
    }
}
