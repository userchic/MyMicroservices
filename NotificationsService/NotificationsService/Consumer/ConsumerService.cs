using Confluent.Kafka;
using CSharpFunctionalExtensions;
using NotificationsService.Abstractions;
using System;
using System.Threading.Tasks;
namespace NotificationsService.Consumer
{
    public class ConsumerService
    {
        IConsumer<string, int> _consumer;
        INotificationsService notificationsService;

        CancellationTokenSource cancellationSource;
        bool isConsuming = false;
        public ConsumerService(INotificationsService notifService)
        {
            notificationsService = notifService;

        }
        public Result<string, string> StartConsuming(int partition)
        {
            if (isConsuming)
                return ((string)null).ToResult("Чтение сообщений уже происходит.");
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "my-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            cancellationSource = new();
            _consumer = new ConsumerBuilder<string, int>(config).Build();
            _consumer.Subscribe("postCreated");
            _consumer.Assign(new TopicPartition("postCreated", new Partition(partition)));
            CancellationToken token = cancellationSource.Token;
            Task task = new Task(() => ConsumingCycle(token), token);
            task.Start();
            return "Успешно начато потребление сообщений".ToResult("Все норм должно быть");
        }
        public Result<string,string> StopConsuming()
        {
            if (!isConsuming)
                return ((string)null).ToResult("В данный момент consumer не работает");
            cancellationSource.Cancel();
            return "Успешно остановлено потребление сообщений".ToResult("Все норм должно быть, ты этого не видел ");
        }
        private void ConsumingCycle(CancellationToken token)
        {
            isConsuming = true;
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var consumeResult = _consumer.Consume(TimeSpan.FromSeconds(10));
                    if (consumeResult is not null)
                        notificationsService.NotifySubscribers(consumeResult.Message.Value);
                }
            }
            catch (ConsumeException e)
            {
                Console.WriteLine($"Error consuming message: {e.Error.Reason}");
            }
            _consumer.Dispose();
            isConsuming = false;
        }
    }
}
