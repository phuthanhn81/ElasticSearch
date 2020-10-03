using Model;
using System;

namespace TRANSFERRING_DATA
{
    class Program
    {
        static void Main(string[] args)
        {
            // Repo repo = new Repo();
            // repo.Save();

            int options;
            do
            {
                Console.WriteLine("Your option ?");
                options = Convert.ToInt32(Console.ReadLine());

                CRUD crud = new CRUD();
                crud._CRUD(options);

                Console.WriteLine("Your input: {0}", options);
            }
            while (options != 0);

            Console.WriteLine("End");
            Console.ReadLine();
        }
    }
}
