namespace AuthService.DTO
{
    public record RegistryRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Fatname { get; set; }
        public DateTime Birthday { get; set; }
    }
}
