namespace MovieFinder.DtoModels
{
    public class RegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsSuccess { get; set; }
        public string Error { get; set; }
    }
}
 