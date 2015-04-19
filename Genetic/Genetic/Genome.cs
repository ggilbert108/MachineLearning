using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Genetic
{
    public struct Genome
    {
        private const int ADDITION        = 10;
        private const int SUBTRACTION     = 11;
        private const int MULTIPLICATION  = 12;
        private const int DIVISION        = 13;

        public static Random random = new Random();
        public const int SIZE = 36;

        public string chromosome;

        public Genome(string chromosome)
        {
            this.chromosome = chromosome;
        }

        public double fitness(int target)
        {
            int difference = Math.Abs(total() - target);
            difference += 1;
            double fitness =  1.0 / difference;
            return fitness;
        }

        public override string ToString()
        {
            int[] decoded = decodeChromosome();
            List<int> genes = removeNoise(decoded);

            int stop;
            for (stop = genes.Count - 1; stop >= 0; stop--)
            {
                if (genes[stop] < 10)
                {
                    break;
                }
            }

            List<int> cleanGenes = new List<int>();
            for (int i = 0; i <= stop; i++)
            {
                cleanGenes.Add(genes[i]);
            }

            string result = "";
            foreach (int gene in cleanGenes)
            {
                result += geneChar(gene) + " ";
            }
            result += "= " + total();
            return result;
        }

        public int total()
        {
            int[] decoded = decodeChromosome();
            List<int> genes = removeNoise(decoded);

            if (genes.Count == 0)
            {
                return 0;
            }

            int total = genes[0];
            for (int i = 1; i < genes.Count - 1; i += 2)
            {
                int op = genes[i];
                int operand = genes[i + 1];

                total = performOperation(total, operand, op);
            }
            return total;
        }

        private static string geneChar(int n)
        {
            if (n < 10)
            {
                return "" + n;
            }
            switch (n)
            {
                case 10:
                    return "+";
                case 11:
                    return "-";
                case 12:
                    return "*";
                case 13:
                    return "/";
                default:
                    return "#";
            }
        }

        public void mutate()
        {
            int index = random.Next(SIZE);
            char[] chrome = chromosome.ToCharArray();
            if (chrome[index] == '0')
            {
                chrome[index] = '1';
            }
            else
            {
                chrome[index] = '0';
            }
            chromosome = new string(chrome);
        }

        private List<int> removeNoise(int[] genes)
        {
            List<int> result = new List<int>();
            bool bOperand = true;
            for (int i = 0; i < genes.Length; i++)
            {
                int token = genes[i];
                if (token > 13)
                {
                    continue;
                }
                if (bOperand && token < 10 || !bOperand && token > 10)
                {
                    result.Add(token);
                    bOperand = !bOperand;
                }
            }

            return result;
        }

        private int[] decodeChromosome()
        {
            int[] decoded = new int[9];
            int start = 0;
            for (int i = 0; i < 9; i++)
            {
                string gene = chromosome.Substring(start, 4);
                int token = Convert.ToInt32(gene, 2);
                decoded[i] = token;
                start += 4;
            }
            return decoded;
        }

        private int performOperation(int a, int b, int op)
        {
            if (op == ADDITION)
            {
                return a + b;
            }
            if (op == SUBTRACTION)
            {
                return a - b;
            }
            if (op == MULTIPLICATION)
            {
                return a*b;
            }
            if (op == DIVISION)
            {
                if (b == 0)
                {
                    return a + b;
                }
                return a/b;
            }
            return a;
        }

        public static void crossover(ref Genome a, ref Genome b)
        {
            int crosspoint = random.Next(SIZE);
            string aKeep = a.chromosome.Substring(0, crosspoint);
            string aSwap = a.chromosome.Substring(crosspoint);
            string bKeep = b.chromosome.Substring(0, crosspoint);
            string bSwap = b.chromosome.Substring(crosspoint);
          
            a.chromosome = aKeep + bSwap;
            b.chromosome = bKeep + aSwap;
        }
    }
}