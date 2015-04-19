using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic
{
    static class GeneticSimulator
    {
        private const int POPULATION_SIZE = 2000;
        private const double MUTATION_RATE = 0.05;
        private const double CROSSOVER_RATE = 0.7;
        private static int target;

        private static Genome[] population;

        static GeneticSimulator()
        {
            population = new Genome[POPULATION_SIZE];
            for (int i = 0; i < POPULATION_SIZE; i++)
            {
                string chromosome = "";
                while (chromosome.Length < Genome.SIZE)
                {
                    char bit;
                    bit = Genome.random.NextDouble() < 0.5 ? '0' : '1';
                    chromosome += bit;
                }
                population[i] = new Genome(chromosome);
            }
        }

        public static void setTarget(int n)
        {
            target = n;
        }

        public static double simulateGeneration(bool printSample)
        {
            if (printSample)
            {
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine(population[i]);
                }
            }

            double[] fitness = mapFitnesses();
            double result = getAverage(fitness);

            sortByFitness(fitness);
            normalizeFitness(fitness);
            accumulateFitness(fitness);

            Genome[] newPop = new Genome[POPULATION_SIZE];
            for (int i = 0; i < POPULATION_SIZE; i += 2)
            {
                Genome a = population[selectMember(fitness)];
                Genome b = population[selectMember(fitness)];
                if (Genome.random.NextDouble() < CROSSOVER_RATE)
                {
                    Genome.crossover(ref a, ref b);
                }
                newPop[i] = a;
                newPop[i + 1] = b;
            }

            for (int i = 0; i < POPULATION_SIZE; i++)
            {
                if (Genome.random.NextDouble() < MUTATION_RATE)
                {
                    newPop[i].mutate();
                }
            }

            population = newPop;
            return result;
        }

        private static double getAverage(double[] fitness)
        {
            double avg = 0;
            for (int i = 0; i < fitness.Length; i++)
            {
                avg += fitness[i];
            }
            return avg / fitness.Length;
        }

        private static int selectMember(double[] fitness)
        {
            double rand = Genome.random.NextDouble();

            for (int i = 0; i < fitness.Length; i++)
            {
                if (fitness[i] > rand)
                    return i;
            }
            return 0;
        }

        private static void printFitnesses(double[] fitness)
        {
            foreach (double d in fitness)
            {
                if(d < 1 && d > 0)
                    Console.WriteLine(d);
            }
            Console.WriteLine();
        }

        private static void accumulateFitness(double[] fitness)
        {
            double total = 0;
            for (int i = 0; i < fitness.Length; i++)
            {
                total += fitness[i];
                fitness[i] = total;
            }
        }

        private static void normalizeFitness(double[] fitness)
        {
            double total = 0;
            foreach (double d in fitness)
            {
                total += d;
            }
            for (int i = 0; i < fitness.Length; i++)
            {
                fitness[i] /= total;
            }
        }

        private static double[] mapFitnesses()
        {
            double[] fitness = new double[population.Length];

            for (int i = 0; i < fitness.Length; i++)
            {
                fitness[i] = population[i].fitness(target);
            }

            return fitness;
        }

        private static void sortByFitness(double[] fitness)
        {
            //Sort in decending order by fitness
            for (int i = 0; i < fitness.Length; i++)
            {
                int pos = i;
                while (pos >= 1 && fitness[pos] > fitness[pos - 1])
                {
                    swap(fitness, pos, pos - 1);
                    swap(population, pos, pos - 1);
                    pos--;
                }
            }
        }

        private static void swap<T>(T[] array, int a, int b)
        {
            T temp = array[a];
            array[a] = array[b];
            array[b] = temp;
        }
    }
}
