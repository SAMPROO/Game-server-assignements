using System;

namespace dotnetKole
{
    class Program
    {
        static void Main(string[] args)
        {
            FileRepository fr = new FileRepository();

            NewPlayer newPlayer = new NewPlayer();
            newPlayer.Name = "Arnold Schwarzenegger";
            fr.Create(newPlayer);

            fr.Delete(Guid.NewGuid());
        }
    }
}
