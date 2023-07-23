namespace Devameet_CSharp.Dtos
{
    public class MeetUpdateRequestDto
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public List<MeetObjectDto> Objects { get; set; }
    }
}
