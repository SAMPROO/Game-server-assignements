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
            //fr.Create(newPlayer);
            fr.Delete(new Guid("5fdff6f8-2fd0-4b71-a70f-1ac51e60568c"));
        }
    }
}
