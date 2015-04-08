using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrefixStringProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            int product = 0;
            string prefix = ChoosePrefixWithLargestProduct("abababa", out product);
            Console.WriteLine("Prefix with largest product={0} is {1}", product, prefix);
        }

        public static string ChoosePrefixWithLargestProduct(string S, out int product)
        {
            product = 0;
            StringBuilder sb = new StringBuilder();

            int maxproductidx = 0;
            int occurence = 0;
            //Generate all prefixes
            for (int i = 0; i < S.Length; i++)
            {
                sb.Append(S[i]);

                //find number of occurences of prefix
                occurence = 0;
                for (int j = 0; j < S.Length - sb.Length + 1; j++)
                {
                    if (S.Substring(j, sb.Length) == sb.ToString())
                        occurence++;
                }

                if (occurence*sb.Length > product)
                {
                    maxproductidx = i;
                    product = occurence*sb.Length;
                }

                Debug.WriteLine("Prefix={0}, occurence={1}, product={2}", sb, occurence, occurence*sb.Length);
            }

            return S.Substring(0, maxproductidx);
        }
    }
}
