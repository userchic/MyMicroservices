namespace AuthService.DTO
{
    public record RegistryRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Fatname { get; set; }
        public string Email { get; set; }
        public DateOnly Birthday { get; set; }
    }
}
