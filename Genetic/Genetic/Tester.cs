using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic
{
    class Tester
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("GENETIC ALGORITHM EXPRESSION FINDER");
            Console.WriteLine("Enter any number between 1 and 6561");
            Console.WriteLine("The program will try to find expressions" +
                              " that equate to the number you enter. \n" +
                              "If you want try to stump the program, enter" +
                              " a large prime like 643.");
            string line = Console.ReadLine();
            int n = Convert.ToInt32(line);
            while (n < 1 || n > 6561)
            {
                Console.WriteLine("Nice try... but that number is out of bounds.");
                Console.WriteLine("Enter any number between 1 and 6561");
                line = Console.ReadLine();
                n = Convert.ToInt32(line);
            }

            GeneticSimulator.setTarget(n);
            for (int i = 0; i < 100; i++)
            {
                double average = GeneticSimulator.simulateGeneration(false);
                Console.WriteLine("Generation " + i + " average fitness: " + average);
            }
            Console.WriteLine("\n\n");
            Console.WriteLine("Here's a sample of expressions " +
                              "the genetic algorthim found that approximate " + n + ".");
            Console.WriteLine("Note: for our purposes, the order of operations is ALWAYS left to right.");
            GeneticSimulator.simulateGeneration(true);
            Console.ReadLine();
            Console.ReadLine();
        }
    }
}
