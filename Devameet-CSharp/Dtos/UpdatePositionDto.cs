namespace Devameet_CSharp.Dtos
{
    public class UpdatePositionDto
    {
        //posicao e orientaçao do usuario na sala de video chamada
        public int X { get; set; }
        public int Y { get; set; }
        public string Orientation { get; set; } = null!;
    }
}
