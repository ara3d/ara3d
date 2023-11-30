namespace SandboxConsole
{
    using System;

    class Program
    {
        public static void Main()
        {
            DrawRoundedBox(20, 10);
            Console.ReadKey();
        }

        static void DrawRoundedBox(int width, int height)
        {
            if (width < 2 || height < 2)
            {
                Console.WriteLine("Width and height must be greater than 2.");
                return;
            }

            // Define box-drawing characters
            char horizontal = '─';
            char vertical = '│';
            char topLeftCorner = '╭';
            char topRightCorner = '╮';
            char bottomLeftCorner = '╰';
            char bottomRightCorner = '╯';

            // Draw top edge
            Console.Write(topLeftCorner);
            for (int i = 0; i < width - 2; i++)
            {
                Console.Write(horizontal);
            }
            Console.WriteLine(topRightCorner);

            // Draw sides
            for (int i = 0; i < height - 2; i++)
            {
                Console.Write(vertical);
                for (int j = 0; j < width - 2; j++)
                {
                    Console.Write(' ');
                }
                Console.WriteLine(vertical);
            }

            // Draw bottom edge
            Console.Write(bottomLeftCorner);
            for (int i = 0; i < width - 2; i++)
            {
                Console.Write(horizontal);
            }
            Console.WriteLine(bottomRightCorner);
        }
    }
}