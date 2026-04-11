namespace TextPostsService.Producer
{
    public interface IPostProducedMessagerService
    {
        void SendPostCreatedMessageAsync(string topic, int userId);
    }
}
