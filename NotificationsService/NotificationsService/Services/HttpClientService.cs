using NotificationsService.DTO;

namespace NotificationsService.Services
{
    public class HttpClientService
    {
        HttpClientPool clients;
        ILogger logger;
        public HttpClientService(ILogger<HttpClientService> logger)
        {
            clients = new HttpClientPool(3);
            this.logger = logger;
        }
        public ICollection<Subscription> RequestSubscribers(int userId)
        {
            var usedClient = clients.GetClient;
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), $"http://localhost:5198/Subscriptions/GetSubscribers?userId={userId}");
            HttpResponseMessage response = usedClient.Send(request);
            return response.Content.ReadFromJsonAsync<ICollection<Subscription>>().Result;
        }
        public User? RequestUser(int userId)
        {
            var usedClient = clients.GetClient;
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), $"http://localhost:5035/User/GetProfileById?userId={userId}");
            HttpResponseMessage response = usedClient.Send(request);
            try
            {
                return response.Content.ReadFromJsonAsync<User>().Result;
            }
            catch (Exception e)
            {
                logger.LogError("Получен идентификатор пользователя данные по которому не найдены или иная ошибка");
                return null;
            }
        }
        private class HttpClientPool
        {
            List<HttpClient> clients;
            int currentClientIndex;
            public HttpClientPool(int amount)
            {
                clients = new List<HttpClient>(amount);
                for (int i = 0; i < amount; i++)
                {
                    clients.Add(new HttpClient());
                }
            }
            public HttpClient GetClient
            {
                get
                {
                    var HttpClient = clients[currentClientIndex];
                    if (currentClientIndex == clients.Count - 1)
                        currentClientIndex = 0;
                    else
                        currentClientIndex += 1;
                    return HttpClient;
                }
            }
        }
    }
}
