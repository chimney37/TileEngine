using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;
using System.Xml.Schema;
using System.Numerics;
using System.Runtime.InteropServices;

namespace ConsoleApplication1
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            /*
            int[] array = {2, 3, 1, 5};
            int[] aray2 = new int[100000];
            for (int i = 0; i < 99999; i++)
                aray2[i] = i + 2;

            aray2[99998] = 1;
            aray2[99999] = 100001;

            Debug.WriteLine(Solution.solution2(aray2));

            int[] array3 = {-1000, 1000};

            Debug.WriteLine(Solution.solution3(array3));

            int X = 1;
            int[] array4 = {1, 3, 1, 4, 2, 3, 5, 4};

            Debug.WriteLine(Solution.solution5(X, array4));

            int N = 5;
            int[] array5 = {3, 4, 4, 6, 1, 4, 4};
            int[] array6 = {3, 4, 4, 6, 1, 4, 6};
            int[] array7 = {6, 4, 4, 6, 1, 4, 6};
            int[] array8 = {5};
            int[] array9 = {1, 1, 1, 6, 1};
            int[] array10 = {3, 4, 4, 6, 4, 4, 4};

            //Solution.solution6(N,array5).ToList().ForEach(p => Debug.Write(p + ","));

            Debug.WriteLine("");

            Solution.solution6(5, array5).ToList().ForEach(p => Debug.Write(p + ","));

            Debug.WriteLine("");

            int[] array11 = {0, 1, 0, 1, 1};
            //Debug.WriteLine(Solution.solution7(array11));

            int[] bigarray = new int[100000];
            for (int i = 0; i < 100000; i++)
            {
                bigarray[i] = i%2;
            }

            Debug.WriteLine(Solution.solution7(bigarray));

            Debug.WriteLine(Solution.solution8(0, 11, 2));

            */

            //int[] array12 = {-3, 1, 2, -2, 5, 6};
            //Debug.WriteLine(Solution.solution9(array12));

            //int[] array13 = {2, 1, 1, 2, 3, 1};
            //Debug.WriteLine(Solution.solution_DisctinctVals(array13));

            /*
            int[] array14 = {10, 2, 5, 1, 8, 20}; //exists triplet
            int[] array15 = { 10, 50, 5, 1 }; //doesn't exist
            int[] array16 = { 2147483647, 1147483648, 1000000000 }; //doesn't exist
            Debug.WriteLine(Solution.solution_ExistsTriangularTriplet(array14));
            Debug.WriteLine(Solution.solution_ExistsTriangularTriplet(array15));
            Debug.WriteLine(Solution.solution_ExistsTriangularTriplet(array16));*/

            /*
            string S1 = ")))(((";
            string S2 = ")))";
            Debug.WriteLine(Solution.solution_ProperlyNested(S1));
            Debug.WriteLine(Solution.solution_ProperlyNested(S2));*/

            /*
            int[] arrayA = { 8, 6, 5, 3, 2, 4, 7 };
            int[] arrayB = { 1, 1, 1, 1, 1, 0, 0 };
            int[] arrayA2 = {4, 3, 2, 1, 5};
            int[] arrayB2 = { 0, 1, 0, 1, 0 };
            //Debug.WriteLine(Solution.solution_FishAlive(arrayA, arrayB));
            Debug.WriteLine(Solution.solution_FishAlive2(arrayA2, arrayB2));
             * */

            //int[] arrayH = { 1, 4, 3, 4, 1, 4, 3, 4, 1 };
            //Debug.WriteLine(Solution.solution_Stonewall(arrayH));

            /*
            int[] arrayA = { 1, 2, 3, 4, 5, -1, -2, -3 };
            int[] arrayA2 = { 1,2,1,2,2,2 };
            Debug.WriteLine(Solution.solution_EquiLeader(arrayA2));
            Debug.WriteLine(Solution.solution_Dominator(arrayA));

            */

            //int[] arrayA3 = { 23171, 21011, 21123, 21366, 21013, 21367 };
            //Debug.WriteLine(Solution.solution_MaxStockProfit(arrayA3));

            //Debug.WriteLine(Solution.solution_CountFactors(2147483647));

            //int[] P = {1,4,16};
            //int[] Q = {26,10,20};
            //Debug.WriteLine(Solution.solution_SemiPrime(26, P, Q));

            //Debug.WriteLine(Solution.solution_ChocolatesCount(10,4));

            //int[] A = {4, 4, 5, 5, 1};
            //int[] B = { 3, 2, 4, 3, 1 };

            //Solution.solution_FibonacciLadder(A, B).ToList().ForEach(p => Debug.Write(p + ","));

            //int[] A = {2, 1, 5, 1, 2, 2, 2};
            //Debug.WriteLine(Solution.solution_MinMaxDivision(3,5,A));

            //int[] A = { -5, -3, -1, 0, 3, 6 };
            //Debug.WriteLine(Solution.solution_DistinctAbs3(A));

            //int[] A = {1, 3, 7, 9, 9};
            //int[] B = {5, 6, 8, 9, 10};

            //Debug.WriteLine(Solution.solution_MaximumOverlappingSegments(A, B));

            int[] A = {1, -2, 0, 9, -1, -2};
            Debug.WriteLine(Solution.solution_NumberSolitaire(A));

        }
    }

    internal class Solution
    {
        /// <summary>
        /// A game for one player is played on a board consisting of N consecutive squares, numbered from 0 to N − 1. 
        /// There is a number written on each square. 
        /// A non-empty zero-indexed array A of N integers contains the numbers written on the squares. 
        /// Moreover, some squares can be marked during the game.
        /// 
        /// At the beginning of the game, there is a pebble on square number 0 and this is the only square on the board which is marked. 
        /// The goal of the game is to move the pebble to square number N − 1.
        /// 
        /// During each turn we throw a six-sided die, with numbers from 1 to 6 on its faces, 
        /// and consider the number K, which shows on the upper face after the die comes to rest. 
        /// Then we move the pebble standing on square number I to square number I + K, providing that square number I + K exists.
        /// If square number I + K does not exist, we throw the die again until we obtain a valid move. 
        /// Finally, we mark square number I + K.
        /// 
        /// After the game finishes (when the pebble is standing on square number N − 1),
        /// we calculate the result. The result of the game is the sum of the numbers written on all marked squares.
        /// 
        /// A[0] = 1
        /// A[1] = -2
        ///  A[2] = 0
        /// A[3] = 9
        /// A[4] = -1
        /// A[5] = -2
        /// one possible game could be as follows:
        /// •the pebble is on square number 0, which is marked;
        /// •we throw 3; the pebble moves from square number 0 to square number 3; we mark square number 3;
        /// •we throw 5; the pebble does not move, since there is no square number 8 on the board;
        /// •we throw 2; the pebble moves to square number 5; we mark this square and the game ends.
        /// 
        /// The marked squares are 0, 3 and 5, so the result of the game is 1 + 9 + (−2) = 8. 
        /// This is the maximal possible result that can be achieved on this board.
        /// 
        /// The marked squares are 0, 3 and 5, so the result of the game is 1 + 9 + (−2) = 8. 
        /// This is the maximal possible result that can be achieved on this board.
        /// 
        /// that, given a non-empty zero-indexed array A of N integers, returns the maximal result that can be achieved on the board represented by array A.
        /// 
        /// •N is an integer within the range [2..100,000];
        /// •each element of array A is an integer within the range [−10,000..10,000].
        /// 
        /// •expected worst-case time complexity is O(N);
        /// •expected worst-case space complexity is O(N), beyond input storage (not counting the storage required for input arguments).
        /// 
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        public static int solution_NumberSolitaire(int[] A)
        {
            //http://www.martinkysel.com/codility-numbersolitaire-solution/
            // dynamic approach


            // write your code in C# 5.0 with .NET 4.5 (Mono)
            int num_possible_rolls = 6;

            //extends the array with the number of possible rolls so we can use the same algorithm for the first index
            int[] subsolutions = Enumerable.Repeat(int.MinValue, A.Length + num_possible_rolls).ToArray();

            //the start before the dice roll is the value in the first cell
            subsolutions[num_possible_rolls] = A[0];

            //the idea is to incrementally solve the problem, by getting the solution progressively.
            //for example, in the first inner loop, we get the solution to get to the 2nd cell is -1, since 1 - 2 = -1
            //in the 2nd inner loop, the maximum is obtained by getting from cell 0 to cell 2 instead of going from cell 0,1 then to 2.
            int max_previous = 0;
            for (int i = num_possible_rolls + 1; i < A.Length + num_possible_rolls;i++)
            {
                max_previous = int.MinValue;

                //TODO: prev_idx beyond where (i -prev_idx - 1) is less than num_possible_rolls is unnecessary, but complicates
                for (int prev_idx = 0; prev_idx < num_possible_rolls; prev_idx++)
                {
                    //get the maximum of previous sums (from rolling the dice to get the result)
                    max_previous = Math.Max(max_previous, subsolutions[i - prev_idx - 1]);

                    //get the best solution to get to the current index given the maximum from previous (since we sum)
                    subsolutions[i] = A[i - num_possible_rolls] + max_previous;
                }
            }
            return subsolutions[A.Length + num_possible_rolls - 1];
        }

        /// <summary>
        /// Located on a line are N segments, numbered from 0 to N − 1, 
        /// whose positions are given in zero-indexed arrays A and B. 
        /// For each I (0 ≤ I < N) the position of segment I is from A[I] to B[I] (inclusive). 
        /// The segments are sorted by their ends, which means that B[K] ≤ B[K + 1] for K such that 0 ≤ K < N − 1.
        /// 
        /// Two segments I and J, such that I ≠ J, are overlapping if they share at least one common point. 
        /// In other words, A[I] ≤ A[J] ≤ B[I] or A[J] ≤ A[I] ≤ B[J].
        /// 
        /// A[0] = 1    B[0] = 5
        ///A[1] = 3    B[1] = 6
        ///A[2] = 7    B[2] = 8
        ///A[3] = 9    B[3] = 9
        ///A[4] = 9    B[4] = 10
        /// 
        /// The size of a non-overlapping set containing a maximal number of segments is 3. 
        /// For example, possible sets are {0, 2, 3}, {0, 2, 4}, {1, 2, 3} or {1, 2, 4}. 
        /// There is no non-overlapping set with four segments.
        /// 
        /// that, given two zero-indexed arrays A and B consisting of N integers, 
        /// returns the size of a non-overlapping set containing a maximal number of segments.
        /// 
        /// For example, given arrays A, B shown above, the function should return 3, as explained above.
        /// 
        /// •N is an integer within the range [0..30,000];
        /// •each element of arrays A, B is an integer within the range [0..1,000,000,000];
        /// •A[I] ≤ B[I], for each I (0 ≤ I < N);
        /// •B[K] ≤ B[K + 1], for each K (0 ≤ K < N − 1).
        /// 
        /// •expected worst-case time complexity is O(N);
        /// •expected worst-case space complexity is O(N), beyond input storage
        ///  (not counting the storage required for input arguments).
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static int solution_MaximumOverlappingSegments(int[] A, int[] B)
        {
            //consider the greedy approach, in this case, we just consider adjacent segments to compare.
            //try finding the maximum number of non-overlapping segments in a single loop

            //if empty, return 0
            if (A.Length == 0)
                return 0;

            //if there is 1 segment only, special case, already found
            if (A.Length == 1)
                return 1;

            //consider the first segment already found (since if you find a segment that is not overlapping with it,
            //you consider finding 2 segments)
            int maximalnonoverlaps = 1;
            int prev_end = B[0];

            //compare each segments end with the next segments start point
            //if next segments start point is more than the previous segments end point, there is no overlap
            //in that case, increment
            for (int cur = 1; cur < A.Length; cur++)
            {
                if (A[cur] > prev_end)
                {
                    maximalnonoverlaps++;
                    prev_end = B[cur];
                }
            }

            return maximalnonoverlaps;
        }



        /// <summary>
        /// A non-empty zero-indexed array A consisting of N numbers is given. 
        /// The array is sorted in non-decreasing order. 
        /// The absolute distinct count of this array is the number of distinct absolute values among the elements of the array.
        /// 
        /// For example, consider array A such that:
        /// 
        /// A[0] = -5
        ///A[1] = -3
        ///A[2] = -1
        ///A[3] =  0
        ///A[4] =  3
        ///A[5] =  6
        /// 
        /// The absolute distinct count of this array is 5, 
        /// because there are 5 distinct absolute values among the elements of this array, namely 0, 1, 3, 5 and 6.
        /// 
        /// •N is an integer within the range [1..100,000];
        /// •each element of array A is an integer within the range [−2,147,483,648..2,147,483,647];
        /// •array A is sorted in non-decreasing order.
        /// 
        /// •expected worst-case time complexity is O(N);
        /// •expected worst-case space complexity is O(N), beyond input storage (not counting the storage required for input arguments).
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        public static int solution_DistinctAbs(int[] A)
        {
            //how about given the fact array is sorted, we try to count the elements, but annihilate the negative numbers 
            //since it is sorted, we can also eliminate repetitive numbers

            //unfortunately, below is not an O(N) solution

            bool[] touched = new bool[A.Length];

            int dupecnt = 0;
            int j = 0;
            for (int i = 0; i < A.Length; i++)
            {
                j = i;
                int n = A[i];

                if (n == -2147483648)
                    continue;

                if (n < 0)
                {
                    //look for dupes in itself
                    while (++j < A.Length && A[j] == n)
                    {
                        if (!touched[j])
                        {
                            touched[j] = true;
                            dupecnt++;
                        }
                    }

                    j = 0;
                    //look for dupes in positive
                    while (n * -1 <= A[A.Length - 1 - j] && 0 < A[A.Length - 1 - j])
                    {
                        if (A[A.Length - 1 - j] == n*-1)
                        {
                            if (!touched[A.Length - 1 - j])
                            {
                                touched[A.Length - 1 - j] = true;
                                dupecnt++;
                            }
                        }
                        if (j++ == A.Length)
                            break;
                    }
                }
                else
                {
                    //look for dupes in itself
                    while (++j < A.Length && A[j] == n)
                    {
                        if (!touched[j])
                        {
                            touched[j] = true;
                            dupecnt++;
                        }
                    }
                }
            }

            return A.Length - dupecnt;
        }

        /// <summary>
        /// An O(N) space solution
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        public static int solution_DistinctAbs2(int[] A)
        {
            Dictionary<int,int> hash = new Dictionary<int, int>(A.Length);

            for (int i = 0; i < A.Length; i++)
            {
                if (A[i] == -2147483648)
                {
                    hash.Add(A[i],0);
                    continue;
                }

                if (!hash.ContainsKey(Math.Abs(A[i])))
                {
                    hash.Add(Math.Abs(A[i]),0);
                }
            }

            return hash.Keys.Count;
        }

        /// <summary>
        /// O(1) space solution
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        public static int solution_DistinctAbs3(int[] A)
        {
            int distinctcnt = 1;

            //set current element to the max value, either on the head or on the tail
            long current = Math.Max(Math.Abs((long)A[0]), Math.Abs((long)A[A.Length - 1]));
            int idx_head = 0;
            int idx_tail = A.Length - 1;

            //travel from greatest abs to smallest abs
            while (idx_head <= idx_tail)
            {
                long former = Math.Abs((long)A[idx_head]);

                //skip the elements on the head side that are the same as current value.
                if (former == current)
                {
                    idx_head++;
                    continue;
                }

                //skip the elments on the tail side that are the same as the current value.
                long latter = Math.Abs((long)A[idx_tail]);
                if (latter == current)
                {
                    idx_tail--;
                    continue;
                }

                //set the current value to the latter, if the head is bigger than or equal to tail value
                if (former >= latter)
                {
                    current = former;
                    idx_head++;
                }
                else
                {
                    current = latter;
                    idx_tail--;
                }

                distinctcnt++;
            }

            return distinctcnt;
        }

        /// <summary>
        /// You are given integers K, M and a non-empty zero-indexed array A consisting of N integers. 
        /// Every element of the array is not greater than M.
        /// You should divide this array into K blocks of consecutive elements. The size of the block is any integer between 0 and N. 
        /// Every element of the array should belong to some block.
        /// The sum of the block from X to Y equals A[X] + A[X + 1] + ... + A[Y]. The sum of empty block equals 0.
        /// The large sum is the maximal sum of any block.
        /// For example, you are given integers K = 3, M = 5 and array A such that:
        /// 
        ///  A[0] = 2
        ///A[1] = 1
        ///A[2] = 5
        ///A[3] = 1
        ///A[4] = 2
        ///A[5] = 2
        ///A[6] = 2
        /// 
        /// The array can be divided, for example, into the following blocks:
        ///[2], [1, 5, 1, 2], [2, 2] with a large sum of 9;
        ///•[2, 1, 5], [], [1, 2, 2, 2] with a large sum of 8;
        ///•[2, 1], [5, 1], [2, 2, 2] with a large sum of 6.
        /// 
        /// The goal is to minimize the large sum. In the above example, 6 is the minimal large sum.
        /// that, given integers K, M and a non-empty zero-indexed array A consisting of N integers, returns the minimal large sum.
        /// 
        /// the function should return 6, as explained above.
        /// 
        /// •N and K are integers within the range [1..100,000];
        /// •M is an integer within the range [0..10,000];
        /// •each element of array A is an integer within the range [0..M].
        /// 
        /// •expected worst-case time complexity is O(N*log(N+M));
        /// •expected worst-case space complexity is O(1), beyond input storage (not counting the storage required for input arguments).
        /// 
        /// </summary>
        /// <param name="K"></param>
        /// <param name="M"></param>
        /// <param name="A"></param>
        /// <returns></returns>
        public static int solution_MinMaxDivision(int K, int M, int[] A)
        {
            // write your code in C# 5.0 with .NET 4.5 (Mono)
            //part of the problem seems to be determining how many combinations of blocks is possible
            // another part if summing up all the individual blocks to get the large sum
            //maybe we can precompute a cumulative sum for each index, 
            // so we can say A[X] + A[X + 1]+...+A[Y] = A[Y] - A[X - 1]
            //but then the space complexity would be O(N)..

            //maybe we can divide and conquer
            //recursively take an element in the array to be a block and generate the remaining blocks
            //but the complexity would be large..

            //a solution from elsewhere uses binary search
            //the idea is very elegant. It searches for a minimal large sum by doing a binary search for 
            //a value between 0, the minimal value of M and the maximum value, given by the maximum theoretical sum in the array
            //(maximum theoretical happens when A[i] = M for all i)
            //with a candidate value, it tries to see if a block sums 

            //maximum sum possible
            int maxSum = M*A.Length;

            int minSum = 0, mid = -1;
            int total = 0;

            //binary search for the minimum sum
            while (minSum <= maxSum)
            {
                mid = (maxSum + minSum)/2;

                if (CheckLargeSumIsPossible(K, mid, A))
                    maxSum = mid - 1;
                else
                    minSum = mid + 1;
            }

            if (CheckLargeSumIsPossible(K, minSum, A))
                minSum--;

            return minSum;
        }

        /// <summary>
        /// Checks if the candidate largesum is possible in the given array
        /// if summing up the entire array doesn't match the candidate, it will return false
        /// it doesn't check all possible cases
        /// 
        /// </summary>
        /// <param name="K"></param>
        /// <param name="candidatelargeSum"></param>
        /// <param name="A"></param>
        /// <returns></returns>
        public static bool CheckLargeSumIsPossible(int K, int candidatelargeSum, int[] A)
        {
            int cur = 0, i = 0;
            while (K > 0 && i < A.Length)
            {
                while (i < A.Length && cur + A[i] < candidatelargeSum)
                    cur += A[i++];

                K--;
                cur = 0;
            }
            return i == A.Length;
        }


        /// <summary>
        /// You have to climb up a ladder. 
        /// The ladder has exactly N rungs, numbered from 1 to N. 
        /// With each step, you can ascend by one or two rungs. 
        /// More precisely:
        /// 
        /// •with your first step you can stand on rung 1 or 2,
        /// •if you are on rung K, you can move to rungs K + 1 or K + 2,
        /// •finally you have to stand on rung N.
        /// 
        /// Your task is to count the number of different ways of climbing to the top of the ladder.
        /// For example, given N = 4, you have five different ways of climbing, ascending by:
        /// 
        /// •1, 1, 1 and 1 rung,
        /// •1, 1 and 2 rungs,
        /// •1, 2 and 1 rung,
        /// •2 and 2 rungs.
        /// 
        /// Given N = 5, you have eight different ways of climbing, ascending by:
        /// 
        /// The number of different ways can be very large, 
        /// so it is sufficient to return the result modulo 2^P, for a given integer P.
        /// 
        /// that, given two non-empty zero-indexed arrays A and B of L integers, 
        /// returns an array consisting of L integers specifying the consecutive answers; 
        /// position I should contain the number of different ways of climbing the ladder with A[I] rungs modulo 2^B[I].
        /// 
        /// For example, given L = 5 and:

        ///  A[0] = 4   B[0] = 3
        ///  A[1] = 4   B[1] = 2
        ///  A[2] = 5   B[2] = 4
        ///  A[3] = 5   B[3] = 3
        ///  A[4] = 1   B[4] = 1
        /// the function should return the sequence [5, 1, 8, 0, 1], as explained above.
        /// 
        /// •L is an integer within the range [1..30,000];
        /// •each element of array A is an integer within the range [1..L];
        /// •each element of array B is an integer within the range [1..30].
        /// 
        /// •expected worst-case time complexity is O(L);
        /// •expected worst-case space complexity is O(L), 
        /// beyond input storage (not counting the storage required for input arguments).
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static int[] solution_FibonacciLadder(int[] A, int[] B)
        {
            // write your code in C# 5.0 with .NET 4.5 (Mono)
            //The ways to calculate the different ways of climbing seems like a fibonacci sequence
            //by using examples, we can deduce:
            //N = 1, we have 1 way
            //N = 2, we have 2 different ways
            //N = 3, we have 3 different ways
            //N = 4, we have 5 different ways
            //N = 5, we have 8 different ways
            //N = 6, we have 13 different ways

            //we can prove for N = m, we have Fib(m+2) ways
            //the problem then becomes, how do we calcuate Fib(A[i] + 2) in O(n) time when there could be L number of i.
            //if we simply loop through, we end up with a O(L*N) time complexity
            //we can observe that if we calculate fibonacci for the max value in the sequence A[i], take an example L.
            //Then, we would have calculated fibonacci for k = L - 1 and so on.
            //If we are able to access Fibonacci[L-1] through an O(1) operation (which we can by FibArray[k + 1])
            //Given that FibArray is output by Fibonacci sequence where FibArray[0] is Fib(1)
            //we can get a Fiboacci sequence array in O(L) time.
            //then we can modulo the result with 2^B[k]

            //edge cases:
            //are there any overflows? Take 2^B[i] where B[i] is 30. 2^30 = 1,073,741,824, which is still within the int limit
            //Fibonacci sequence is capable of overflowing 64bit integers fairly quickly like Fib(40+) or so.
            //System.Numerics.BigInteger class can prevent this and give the correct answer
            //However modulo operation is very slow on BigInteger
            //we can prevent this by precomputing the fibonacci sequence using only some tailing bits

            //we need to find the maximum value of A[i]
            int maxvalue = A.Max();

            //we can reduce the fibonacci sequence to a smaller form by cutting of the upper bits that aren't necessary.
            //we can do this because in the end, we are doing a modulo that would remove the upper bits anyway.
            //for example, B.Max() is 1. then modulo limit is 1 less than 2.
            //this is easier to understand if you think in terms of bits. 16 = 10000, but 15 is 01111
            //modLimit is used in Fibonacci_iterativeModulo which basically takes the bits up to the modLimit.fantastic.
            //now we don't need to use bigint
            int modLimit = (1 << B.Max()) - 1;

            //We need to call Fib(L + 2). Based on the function that returns Fib sequence up to (L - 1) given Fib(L)
            //we need to call func(L + 2 + 1)
            long[] FibSequence = Fibonacci_IterativeModulo(maxvalue + 2 + 1, modLimit);
            int[] result = new int[A.Length];

            for (int i = 0; i < A.Length; i++)
            {
                //get number of different ways for each A[i]
                //Using the binary method to get the modulo is much faster than the % operator
                //we can do this in the special case of the modulo of power of 2 (y mod 2^x) can be represented in the binary AND
                //The binary AND will remove the upper bits that will be lost, in this case would be equivalent to a modulo operation.
                result[i] = (int)(FibSequence[A[i] + 1] & (1 << B[i]) - 1);
            }

            return result;
        }

        /// <summary>
        /// Calculates Fibonacci and returns a fibobacci sequence
        /// len = 1 return 0
        /// len = 2 return 1
        /// len = 3 return 1
        /// len = 4 return 2
        /// len = 5 return 4
        /// generalizing, len = n returns Fib(n - 1)
        /// 
        /// B an array of values that should modulo by 2^B{i]
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static long[] Fibonacci_Iterative(int len)
        {
            long[] fibosequence = new long[len];
            long a = 0, b = 1;
            fibosequence[0] = a;
            fibosequence[1] = b;

            for (int i = 2; i < len; i++)
            {
                long c = a + b;
                fibosequence[i] = c;
                a = b;
                b = c;
            }
            return fibosequence;
        }

        public static long[] Fibonacci_IterativeModulo(int len, int modLimit)
        {
            long[] fibosequence = new long[len];
            long a = 0, b = 1;
            fibosequence[0] = a;
            fibosequence[1] = b;

            for (int i = 2; i < len; i++)
            {
                long c = (a + b) & modLimit;
                fibosequence[i] = c;
                a = b;
                b = c;
            }
            return fibosequence;
        }


        /// <summary>
        /// Calculates Fibonacci sequence using BigInteger
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static BigInteger[] Fibonacci_IterativeBig(int len)
        {
            BigInteger[] fibosequence = new BigInteger[len];
            BigInteger a = 0, b = 1;
            fibosequence[0] = a;
            fibosequence[1] = b;

            for (int i = 2; i < len; i++)
            {
                BigInteger c = a + b;
                fibosequence[i] = c;
                a = b;
                b = c;
            }
            return fibosequence;
        }


        /// <summary>
        /// Two positive integers N and M are given. 
        /// Integer N represents the number of chocolates arranged in a circle, numbered from 0 to N − 1.
        /// 
        /// You start to eat the chocolates. After eating a chocolate you leave only a wrapper.
        /// You begin with eating chocolate number 0. 
        /// Then you omit the next M − 1 chocolates or wrappers on the circle, and eat the following one.
        /// 
        /// More precisely, if you ate chocolate number X, 
        /// then you will next eat the chocolate with number (X + M) modulo N (remainder of division).
        /// 
        /// You stop eating when you encounter an empty wrapper.
        /// For example, given integers N = 10 and M = 4. 
        /// You will eat the following chocolates: 0, 4, 8, 2, 6.
        /// 
        /// The goal is to count the number of chocolates that you will eat, following the above rules.
        /// 
        /// that, given two positive integers N and M, returns the number of chocolates that you will eat.
        /// 
        /// For example, given integers N = 10 and M = 4. the function should return 5, as explained above.
        /// •N and M are integers within the range [1..1,000,000,000].
        /// 
        /// •expected worst-case time complexity is O(log(N+M));
        /// •expected worst-case space complexity is O(log(N+M)).
        /// 
        /// </summary>
        /// <param name="N"></param>
        /// <param name="M"></param>
        /// <returns></returns>
        public static int solution_ChocolatesCount(int N, int M)
        {
            //in principle, we must look for a position that is of an empty wrapper, for the calculations to end.
            //below commented out shows the naive approach.
            //by property of modulo, there is exist a value k such that i * M + k *N = j*M.
            //we can prove that smallest i must be zero. for all i !=0, (i -i)*M + k*N = (j - i)* M.
            //first eaten position would e first position we meet again.
            // j is the number of chocolates we eat.
            // the idea is to then determine j.
            // j = LCM(N,M)/M. 
            // LCM(N,M) = N*M/GCD(N,M)
            //get the LCM Divided by M, so that we don't have to make the extra divisions
            int lcmDividedByM = N/GCD(N, M);

            return lcmDividedByM;

            #region SUBOPTIMAL and NAIVE

            /*
            if (M == 1)
                return N;

            if (N % M == 0)
                return N / M;

            if (N % M == 1)
                return N;

            int cnt = 0;
            Dictionary<int,bool> marker = new Dictionary<int, bool>();

            int i = 0;
            while (!marker.ContainsKey(i))
            {
                marker.Add(i,true);
                i = (i + M) % N;
                cnt++;
            }
            return cnt;
            */

            #endregion



        }

        //Euclidean algorithm : Greatest Common Denominator
        private static int GCD(int a, int b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            if (a == 0)
                return b;
            
            return a;
        }

        /// <summary>
        /// A prime is a positive integer X that has exactly two distinct divisors: 1 and X. The first few prime integers are 2, 3, 5, 7, 11 and 13.
        /// 
        /// A semiprime is a natural number that is the product of two (not necessarily distinct) prime numbers. The first few semiprimes are 4, 6, 9, 10, 14, 15, 21, 22, 25, 26.
        /// 
        /// You are given two non-empty zero-indexed arrays P and Q, each consisting of M integers. 
        /// These arrays represent queries about the number of semiprimes within specified ranges.
        /// 
        /// Query K requires you to find the number of semiprimes within the range (P[K], Q[K]), where 1 ≤ P[K] ≤ Q[K] ≤ N.
        /// 
        /// For example, consider an integer N = 26 and arrays P, Q such that:
        /// P[0] = 1    Q[0] = 26
        ///P[1] = 4    Q[1] = 10
        ///P[2] = 16   Q[2] = 20
        /// 
        /// The number of semiprimes within each of these ranges is as follows:
        /// •(1, 26) is 10,
        /// •(4, 10) is 4,
        /// •(16, 20) is 0.
        /// 
        /// that, given an integer N and two non-empty zero-indexed arrays P and Q consisting of M integers, 
        /// returns an array consisting of M elements specifying the consecutive answers to all the queries.
        /// 
        /// the function should return the values [10, 4, 0], as explained above.
        /// •N is an integer within the range [1..50,000];
        /// •M is an integer within the range [1..30,000];
        /// •each element of arrays P, Q is an integer within the range [1..N];
        /// •P[i] ≤ Q[i].
        /// 
        /// •expected worst-case time complexity is O(N*log(log(N))+M);
        /// •expected worst-case space complexity is O(N+M), beyond input storage (not counting the storage required for input arguments).
        /// </summary>
        /// <param name="N"></param>
        /// <param name="P"></param>
        /// <param name="Q"></param>
        /// <returns></returns>
        public static int[] solution_SemiPrime(int N, int[] P, int[] Q)
        {
            // write your code in C# 5.0 with .NET 4.5 (Mono)
            //first figure out the primes from 1 up to the range N
            //maybe should use defactorization and given 2 factors, we know it's a semi prime

            int[] arrayF = GetF(N);

            bool[] issemiprime = new bool[N+1];
            for(int i =0; i < N + 1 ;i++)
                issemiprime[i] = false;

            //obtain all semiprimes for the entire range
            for (int i = 1; i < N + 1; i++)
            {
                //check for semiprime
                if (GetPrimeFactors(i, arrayF).Length == 2)
                    issemiprime[i] = true;
            }

            //Compute the number of semiprimes until each position
            //Define: semiprimecnt[i] = k => in (0,i) there are k semiprimes.
            int counter = 0;
            int[] semiprimecnt = new int[N + 1];
            for (int i = 0; i < semiprimecnt.Length; i++)
            {
                if (issemiprime[i])
                    semiprimecnt[i] = ++counter;
                else
                    semiprimecnt[i] = counter;
            }

            //number of semiprimes within range P[K], Q[K]
            //is semiprime[Q[K] - semiprime[P[K] -1]
            int[] result = new int[P.Length];
            for (int m = 0; m < P.Length; m++)
            {
                result[m] = semiprimecnt[Q[m]] - semiprimecnt[P[m] - 1];
            }


            #region DISCARD
            /* This is a suboptimal way to find number of semiprimes
             * since O(N*2). 
            for (int m = 0; m < P.Length; m++)
            {
                int semiprimecnt = 0;
                for(int s = P[m]; s <= Q[m] ;s++)
                    if (issemiprime[s])
                        semiprimecnt++;

                result[m] = semiprimecnt;
            }
             */
            #endregion
            return result;
        }

        private static int[] GetF(int N)
        {
            // Make an array to get factors
            int[] F = new int[N + 1];
            for (int n = 0; n < N + 1; n++) F[n] = 0;

            int i = 2;
            while (i*i <= N)
            {
                if (F[i] == 0)
                {
                    int k = i*i;
                    while (k <= N)
                    {
                        if (F[k] == 0)
                            F[k] = i;

                        k+=i;
                    }
                }
                i++;
            }
            return F;
        }
        private static int[] GetPrimeFactors(int x, int[] F)
        {
            List<int> factors = new List<int>();
            while (F[x] > 0)
            {
                factors.Add(F[x]);
                x /= F[x];
            }
            factors.Add(x);
            return factors.ToArray();
        }

        /// <summary>
        /// A positive integer D is a factor of a positive integer N if there exists an integer M such that N = D * M.
        /// For example, 6 is a factor of 24, because M = 4 satisfies the above condition (24 = 6 * 4).
        /// 
        /// that, given a positive integer N, returns the number of its factors.
        /// For example, given N = 24, the function should return 8, because 24 has 8 factors, namely 1, 2, 3, 4, 6, 8, 12, 24. There are no other factors of 24.
        /// 
        /// •N is an integer within the range [1..2,147,483,647].
        /// •expected worst-case time complexity is O(sqrt(N));
        /// •expected worst-case space complexity is O(1).
        /// 
        /// </summary>
        /// <param name="N"></param>
        /// <returns></returns>
        public static int solution_CountFactors(int N)
        {
            // sounds like problem of counting divisors of n
            long i = 1;
            int result = 0;

            //test up to the sqrt(N) is enough to find divisors
            while (i * i < N)
            {
                //if divisible by i, there are 2 divisors possible, i and N/i.
                if (N % i == 0)
                    result += 2;
                i++;
            }

            //check when value is sqrt(N), there is 1 extra divisor possible
            if (i*i == N)
                result += 1;

            return result;
        }

        /// <summary>
        /// An integer N is given, representing the area of some rectangle.
        /// The area of a rectangle whose sides are of length A and B is A * B, and the perimeter is 2 * (A + B).
        /// The goal is to find the minimal perimeter of any rectangle whose area equals N. 
        /// The sides of this rectangle should be only integers.
        /// 
        /// For example, given integer N = 30, rectangles of area 30 are:
        /// •(1, 30), with a perimeter of 62,
        /// •(2, 15), with a perimeter of 34,
        /// •(3, 10), with a perimeter of 26,
        /// •(5, 6), with a perimeter of 22.
        /// 
        /// that, given an integer N, returns the minimal perimeter of any rectangle whose area is exactly equal to N.
        /// For example, given an integer N = 30, the function should return 22, as explained above.
        /// 
        /// •N is an integer within the range  [1..1,000,000,000].
        /// 
        /// •expected worst-case time complexity is O(sqrt(N));
        /// •expected worst-case space complexity is O(1).
        /// 
        /// </summary>
        /// <param name="N"></param>
        /// <returns></returns>
        public static int solution_MinParamRectangle(int N)
        {
            //sounds like a problem of counting divisors of n to get all parameter pairs

            int i = 1;
            int minparameter = int.MaxValue;

            //test up to the sqrt(N) is enough to find divisors
            while (i*i < N)
            {
                //if divisible by i
                if (N%i == 0)
                {
                    int parameter = 2*((N/i) + i);
                    if (parameter < minparameter)
                        minparameter = parameter;
                }
                i++;
            }

            //check when value is sqrt(N)
            if (i*i == N)
            {
                int parameter = 2 * ((N / i) + i);
                if (parameter < minparameter)
                    minparameter = parameter;
            }

            return minparameter;
        }

        /// <summary>
        /// A non-empty zero-indexed array A consisting of N integers is given.
        /// The leader of this array is the value that occurs in more than half of the elements of A.
        /// 
        /// An equi leader is an index S such that 0 ≤ S < N − 1 and two sequences A[0], A[1], ..., A[S] and A[S + 1], A[S + 2], ..., A[N − 1] have leaders of the same value.
        /// 
        /// For example, given array A such that:

        /// A[0] = 4
        /// A[1] = 3
        /// A[2] = 4
        /// A[3] = 4
        /// A[4] = 4
        /// A[5] = 2
        /// we can find two equi leaders:
        /// 
        /// •0, because sequences: (4) and (3, 4, 4, 4, 2) have the same leader, whose value is 4.
        /// •2, because sequences: (4, 3, 4) and (4, 4, 2) have the same leader, whose value is 4.
        /// 
        /// The goal is to count the number of equi leaders.
        /// that, given a non-empty zero-indexed array A consisting of N integers, returns the number of equi leaders.
        /// 
        /// the function should return 2, as explained above.
        /// •N is an integer within the range [1..100,000];
        /// 
        /// •each element of array A is an integer within the range [−1,000,000,000..1,000,000,000].
        /// •expected worst-case time complexity is O(N);
        /// •expected worst-case space complexity is O(N), beyond input storage (not counting the storage required for input arguments).
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        public static int solution_EquiLeader(int[] A)
        {
            // write your code in C# 5.0 with .NET 4.5 (Mono)
            //follow the Leader O(n) algorithm to find leader

            int candidate = -1;
            int leader = -1;
            int count = 0;

            //use a stack and find consecutive elements. If a differing element is encountered, we remove them from stack
            Stack<int> conseElestk = new Stack<int>();
            for (int i = 0; i < A.Length; i++)
            {
                if (conseElestk.Count == 0)
                    conseElestk.Push(A[i]);
                else
                {
                    if (conseElestk.Peek() != A[i])
                        conseElestk.Pop();
                    else
                        conseElestk.Push(A[i]);
                }               
            }

            //find the leader by making sure it occurs in more than half the size of the array
            candidate = -1;
            if (conseElestk.Count > 0)
                candidate = conseElestk.Peek();

            leader = -1;
            count = 0;
            for (int i = 0; i < A.Length; i++)
            {
                if (A[i] == candidate)
                    count++;
            }

            //check for dominance
            if (count > A.Length/2)
                leader = candidate;
            //if not it's not possible to have a leader
            else
                return 0;
            

            //find equileader
            int equileader = 0;
            int ldrCount = 0;
            for (int i = 0; i < A.Length; i++)
            {
                if (A[i] == leader) ldrCount++;

                //get the right subsequence
                int leadersInRightPart = count - ldrCount;

                //if both left and right leader count are dominant, then increase equileader count
                if (ldrCount > (i + 1)/2 && leadersInRightPart > (A.Length - i - 1)/2)
                    equileader++;
            }
            return equileader;
        }

        /// <summary>
        /// A zero-indexed array A consisting of N integers is given. The dominator of array A is the value that occurs in more than half of the elements of A.
        /// 
        /// A[0] = 3    A[1] = 4    A[2] =  3
        ///A[3] = 2    A[4] = 3    A[5] = -1
        ///A[6] = 3    A[7] = 3
        /// 
        /// The dominator of A is 3 because it occurs in 5 out of 8 elements of A (namely in those with indices 0, 2, 4, 6 and 7) and 5 is more than a half of 8.
        /// that, given a zero-indexed array A consisting of N integers, returns index of any element of array A in which the dominator of A occurs. The function should return −1 if array A does not have a dominator.
        /// 
        /// •N is an integer within the range [0..100,000];
        /// •each element of array A is an integer within the range [−2,147,483,648..2,147,483,647].
        ///
        /// the function may return 0, 2, 4, 6 or 7, as explained above.
        /// 
        /// •expected worst-case time complexity is O(N);
        /// •expected worst-case space complexity is O(1), beyond input storage (not counting the storage required for input arguments).
        /// 
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        public static int solution_Dominator(int[] A)
        {
            int candidate = -1;
            long leader = long.MinValue;
            int count = 0;
            int size = 0;
            int value = 0;

            //use a simulated stack(that only keeps size and top value) and find consecutive elements. If a differing element is encountered, we remove them from stack
            for (int i = 0; i < A.Length; i++)
            {
                if (size == 0)
                {
                    size++;
                    value = A[i];
                }
                else
                {
                    if (value != A[i])
                    {
                        size--;
                    }
                    else
                    {
                        value = A[i];
                        size++;
                    }
                }
            }

            //find the leader by making sure it occurs in more than half the size of the array
            candidate = -1;
            if (size > 0)
                candidate = value;

            count = 0;
            for (int i = 0; i < A.Length; i++)
            {
                if (A[i] == candidate)
                    count++;
            }

            //check for dominance
            if (count > A.Length / 2)
                leader = candidate;

            for (int i = 0; i < A.Length; i++)
            {
                if (A[i] == leader)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// A zero-indexed array A consisting of N integers is given. 
        /// It contains daily prices of a stock share for a period of N consecutive days. 
        /// If a single share was bought on day P and sold on day Q, where 0 ≤ P ≤ Q < N, 
        /// then the profit of such transaction is equal to A[Q] − A[P], provided that A[Q] ≥ A[P]. 
        /// Otherwise, the transaction brings loss of A[P] − A[Q].
        /// For example, consider the following array A consisting of six elements such that:
        /// A[0] = 23171
        ///A[1] = 21011
        ///A[2] = 21123
        ///A[3] = 21366
        ///A[4] = 21013
        ///A[5] = 21367
        /// 
        /// If a share was bought on day 0 and sold on day 2, a loss of 2048 would occur 
        /// because A[2] − A[0] = 21123 − 23171 = −2048. If a share was bought on day 4 and sold on day 5, 
        /// a profit of 354 would occur because A[5] − A[4] = 21367 − 21013 = 354. 
        /// Maximum possible profit was 356. It would occur if a share was bought on day 1 and sold on day 5.
        /// 
        /// that, given a zero-indexed array A consisting of N integers containing daily prices of a stock share 
        /// for a period of N consecutive days, returns the maximum possible profit from one transaction during this period. 
        /// The function should return 0 if it was impossible to gain any profit.
        /// 
        /// the function should return 356, as explained above.
        /// 
        /// •N is an integer within the range [0..400,000];
        /// •each element of array A is an integer within the range [0..200,000].
        /// 
        /// •expected worst-case time complexity is O(N);
        /// •expected worst-case space complexity is O(1), beyond input storage (not counting the storage required for input arguments).
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        public static int solution_MaxStockProfit(int[] A)
        {
            //for this maybe we better know the Kadane's algorithm which can determine a maxslice in O(n)
            //http://en.wikipedia.org/wiki/Maximum_subarray_problem

            #region DISCARD
            /*
            //O(N^2) solution..discard
            int max = int.MinValue;
            for (int i = 0; i < A.Length; i++)
            {
                for (int j = i + 1; j < A.Length; j++)
                {
                    if (A[j] - A[i] > max)
                        max = A[j] - A[i];
                }
            }

            return max > 0 ? max : 0; 

            */
            #endregion

            #region DISCARD2
            //the idea is correct, but it is not able to handle all cases (not sure of the test case that fails though). 
            //It is better to use the Kadane's algorithm
            /*
            if (A.Length == 0)
                return 0;

            int min = 200001;
            int earliestminidx = -1;

            //find the earliest minimum value
            for (int i = A.Length - 1; i >= 0; i--)
            {
                if (A[i] < min)
                {
                    min = A[i];
                    earliestminidx = i;
                }
            }

            int max = int.MinValue;
            int premaximaidx = -1;
            int premax = int.MinValue;
            //try to find a post value that is the maximal value
            for (int i = 0; i < A.Length; i++)
            {
                if (A[i] > max && i < earliestminidx)
                {
                    premaximaidx = i;
                    premax = A[i];
                }
                else if (A[i] > max && i > earliestminidx)
                {
                    max = A[i];
                }
            }


            int localmin = 200001;
            int localminidx = -1;
            //a bigger max exists
            if (max < premax)
            {
                //find a local minima
                for (int i = 0; i < premaximaidx; i++)
                {
                    if (A[i] < localmin)
                    {
                        localminidx = i;
                        localmin = A[i];
                    }
                }

                if(localminidx >= 0)
                    return (premax - localmin) > 0 ? premax - localmin : 0;
            }

            return max - min;
             * 
             */
            
            #endregion

            if (A.Length <= 1)
                return 0;

            int maxProfitSoFar = 0;
            int minPrice = A[0];

            for (int i = 1; i < A.Length; i++)
            {
                //calculates the profit here, if bigger than 0
                int profitHere = Math.Max(0, A[i] - minPrice);

                //tracks minimum price : this is the part that is not part of Kadane
                minPrice = Math.Min(minPrice, A[i]);

                //tracks the maximum profit so far
                maxProfitSoFar = Math.Max(profitHere, maxProfitSoFar);
            }

            return maxProfitSoFar;
        }

        /// <summary>
        /// FrogJump
        /// Count minimal number of jumps from position X to Y. 
        /// 
        /// 
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="D"></param>
        /// <returns></returns>
        public static int solution_FrogJump(int X, int Y, int D)
        {
            /// This is a range division problem

            if (X == Y)
                return 0;

            return (Y - X)%D == 0 ? (Y - X)/D : (Y - X)/D + 1;
        }

        /// <summary>
        /// PermMissingElem 
        /// A zero-indexed array A consisting of N different integers is given. The array contains integers in the range [1..(N + 1)], which means that exactly one element is missing.
        ///Your goal is to find that missing element.
        ///Write a function:int solution(int A[], int N); 
        ///that, given a zero-indexed array A, returns the value of the missing element.
        ///
        ///For example, given array A such that:
        ///
        /// A[0] = 2
        /// A[1] = 3
        /// A[2] = 1
        ///  A[3] = 5

        ///the function should return 4, as it is the missing element.

        ///Assume that:

        ///•N is an integer within the range [0..100,000];
        ///•the elements of A are all distinct;
        ///•each element of array A is an integer within the range [1..(N + 1)].

        ///Complexity:

        ///•expected worst-case time complexity is O(N);
        ///•expected worst-case space complexity is O(1), beyond input storage (not counting the storage required for input arguments).
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        public static int solution_PermMissingElement(int[] A)
        {
            //if array length is 0, return 1
            if (A.Length == 0)
                return 1;

            //get the sum of all elements
            long sum = 0;
            for (int i = 0; i < A.Length; i++)
                sum += A[i];

            //use equation by gauss
            return (int) ((long) (A.Length + 1)*(A.Length + 2)/2 - sum);
        }

        /// <summary>
        /// TapeEquilibrium 
        /// A non-empty zero-indexed array A consisting of N integers is given. Array A represents numbers on a tape.

        ///Any integer P, such that 0 < P < N, splits this tape into two non-empty parts: A[0], A[1], ..., A[P − 1] and A[P], A[P + 1], ..., A[N − 1].
        ///The difference between the two parts is the value of: |(A[0] + A[1] + ... + A[P − 1]) − (A[P] + A[P + 1] + ... + A[N − 1])|
        ///In other words, it is the absolute difference between the sum of the first part and the sum of the second part.

        /// We can split this tape in four places:

        ///•P = 1, difference = |3 − 10| = 7 
        ///•P = 2, difference = |4 − 9| = 5 
        ///•P = 3, difference = |6 − 7| = 1 
        ///•P = 4, difference = |10 − 3| = 7 

        /// N is an integer within the range [2..100,000];
        ///each element of array A is an integer within the range [−1,000..1,000].

        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        public static int solution_TapeEquilibrium(int[] A)
        {
            int[] diffarray = new int[A.Length - 1];
            int firstsum = 0;
            int overallsum = 0;

            //get the total sum
            for (int i = 0; i < A.Length; i++)
                overallsum += A[i];

            //create an array that calculates for every i = P.
            //(A[0] + ... + A[P-1]) - (A[P] + ... + A[N-1]).
            //use the property that (A[i+1] + ... + A[N-1]) = sum of all - (A[0] +...+ A[i])
            for (int i = 0; i < A.Length - 1; i++)
            {
                firstsum += A[i];
                diffarray[i] = Math.Abs((overallsum - firstsum) - firstsum);
            }

            //find the minimum value
            int maxsum = int.MaxValue;
            for (int i = 0; i < diffarray.Length; i++)
            {
                if (diffarray[i] < maxsum)
                    maxsum = diffarray[i];
            }

            return maxsum;
        }

        /// <summary>
        /// Determine Permutation or not
        /// A non-empty zero-indexed array A consisting of N integers is given.
        /// A permutation is a sequence containing each element from 1 to N once, and only once.
        ///For example, array A such that:
        ///   A[0] = 4
        ///   A[1] = 1
        ///   A[2] = 3
        ///   A[3] = 2
        ///is a permutation, but array A such that:
        ///    A[0] = 4
        ///   A[1] = 1
        ///   A[2] = 3
        ///is not a permutation, because value 2 is missing.
        ///The goal is to check whether array A is a permutation.
        ///Write a function:
        ///class Solution { public int solution(int[] A); } 
        ///that, given a zero-indexed array A, returns 1 if array A is a permutation and 0 if it is not.
        ///For example, given array A such that:
        ///    A[0] = 4
        ///   A[1] = 1
        ///   A[2] = 3
        ///   A[3] = 2
        ///the function should return 1.
        ///Given array A such that:
        ///    A[0] = 4
        ///    A[1] = 1
        ///   A[2] = 3
        ///the function should return 0.
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        public static int solution_PermCheck(int[] A)
        {
            //idea is to use a dictionary like method to check if there is a missing value or repeating value
            int[] markarray = new int[A.Length + 1];

            for (int i = 0; i < A.Length; i++)
            {
                if (A[i] >= markarray.Length)
                    return 0;

                markarray[A[i]]++;
            }

            for (int i = 1; i < markarray.Length; i++)
            {
                if (markarray[i] == 0 || markarray[i] > 1)
                    return 0;
            }

            return 1;
        }

        /// <summary>
        /// Frog jump
        /// A small frog wants to get to the other side of a river. The frog is currently located at position 0, and wants to get to position X. Leaves fall from a tree onto the surface of the river.
        /// You are given a non-empty zero-indexed array A consisting of N integers representing the falling leaves. 
        /// A[K] represents the position where one leaf falls at time K, measured in minutes.
        /// The goal is to find the earliest time when the frog can jump to the other side of the river. 
        /// The frog can cross only when leaves appear at every position across the river from 1 to X.
        /// 
        ///  A[0] = 1
        ///A[1] = 3
        ///A[2] = 1
        ///A[3] = 4
        ///A[4] = 2
        ///A[5] = 3
        ///A[6] = 5
        ///A[7] = 4
        /// In minute 6, a leaf falls into position 5. This is the earliest time when leaves appear in every position across the river.
        /// that, given a non-empty zero-indexed array A consisting of N integers and integer X, returns the earliest time when the frog can jump to the other side of the river.
        /// 
        /// If the frog is never able to jump to the other side of the river, the function should return −1.
        /// 
        /// </summary>
        /// <param name="X"></param>
        /// <param name="A"></param>
        /// <returns></returns>
        public static int solution_FrogRiverOne(int X, int[] A)
        {
            //define an array that stores the time K for a given position X
            int[] xtimearray = new int[X];

            for (int i = 0; i < xtimearray.Length; i++)
                xtimearray[i] = -1;

            for (int i = 0; i < A.Length; i++)
            {
                //check that the position is only up to and including X
                //check also that the time to fall has already been found earlier
                if (A[i] <= X && xtimearray[A[i] - 1] == -1)
                    xtimearray[A[i] - 1] = i;
            }

            int maxtime = 0;
            for (int i = 0; i < xtimearray.Length; i++)
            {
                //if a leave was never found to drop at position i, which means fron can never get to the position
                if (xtimearray[i] == -1)
                    return -1;

                if (xtimearray[i] > maxtime)
                    maxtime = xtimearray[i];
            }
            return maxtime;
        }

        /// <summary>
        /// You are given N counters, initially set to 0, and you have two possible operations on them:
        /// increase(X) − counter X is increased by 1,
        ///•max counter − all counters are set to the maximum value of any counter.
        ///
        ///A non-empty zero-indexed array A of M integers is given. This array represents consecutive operations:
        /// •if A[K] = X, such that 1 ≤ X ≤ N, then operation K is increase(X),
        ///  •if A[K] = N + 1 then operation K is max counter
        /// 
        /// For example, given integer N = 5 and array A such that:
        /// A[0] = 3
        ///A[1] = 4
        ///A[2] = 4
        /// A[3] = 6
        /// A[4] = 1
        /// A[5] = 4
        ///A[6] = 4
        /// 
        /// the values of the counters after each consecutive operation will be:
        /// 
        /// (0, 0, 1, 0, 0)
        /// (0, 0, 1, 1, 0)
        ///  (0, 0, 1, 2, 0)
        ///  (2, 2, 2, 2, 2)
        ///  (3, 2, 2, 2, 2)
        ///  (3, 2, 2, 3, 2)
        ///  (3, 2, 2, 4, 2)
        /// The goal is to calculate the value of every counter after all operations.
        /// that, given an integer N and a non-empty zero-indexed array A consisting of M integers, returns a sequence of integers representing the values of the counters.
        /// 
        /// •N and M are integers within the range [1..100,000];
        /// •each element of array A is an integer within the range [1..N + 1].
        /// 
        /// •expected worst-case time complexity is O(N+M);
        /// •expected worst-case space complexity is O(N), beyond input storage (not counting the storage required for input arguments).
        /// </summary>
        /// <param name="N"></param>
        /// <param name="A"></param>
        /// <returns></returns>
        public static int[] solution_MaxCounters(int N, int[] A)
        {
            int[] counters = new int[N];
            int maxval = -1; //maxval keeps track of the maximum counter value
            int minval = 0; //minval keeps track of the minimum values that were only bumped up with the max op

            //determine position of max counter operation
            for (int i = 0; i < A.Length; i++)
            {
                //increase counter operation found
                if (1 <= A[i] && A[i] <= N)
                {
                    // if a counter value is less than the min value (when the min value is no longer 0 due to a max op), 
                    //it should be set as the min value
                    if (counters[A[i] - 1] < minval)
                        counters[A[i] - 1] = minval;

                    //a normal increment op
                    counters[A[i] - 1]++;

                    //if a counter value is more than the max value, the max value should be updated
                    if (counters[A[i] - 1] > maxval)
                        maxval = counters[A[i] - 1];
                }
                else
                {
                    //set the minval when N + 1
                    minval = maxval;
                }
            }

            //bump up counters only if they are less than the effective max value
            for (int i = 0; i < counters.Length; i++)
            {
                if (counters[i] < minval)
                    counters[i] = minval;
            }

            return counters;
        }

        /// <summary>
        /// A non-empty zero-indexed array A consisting of N integers is given. The consecutive elements of array A represent consecutive cars on a road.
        /// 
        /// Array A contains only 0s and/or 1s:
        /// •0 represents a car traveling east,
        ///•1 represents a car traveling west.
        ///The goal is to count passing cars. We say that a pair of cars (P, Q), where 0 ≤ P < Q < N, is passing when P is traveling to the east and Q is traveling to the west.
        ///For example, consider array A such that:
        ///  A[0] = 0
        ///  A[1] = 1
        ///  A[2] = 0
        ///  A[3] = 1
        ///  A[4] = 1
        /// 
        /// We have five pairs of passing cars: (0, 1), (0, 3), (0, 4), (2, 3), (2, 4).
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        public static int solution_CountPassingCars(int[] A)
        {
            int eastidx = 0;
            int westidx = 0;
            int eastcnt = 0;
            int paircnt = 0;

            // write your code in C# 5.0 with .NET 4.5 (Mono)
            for (int i = 0; i < A.Length; i++)
            {
                //find a east traveling car
                if (A[i] == 0)
                {
                    eastidx = i;
                    eastcnt++;
                }
                //find a west traveling car
                else
                {
                    westidx = i;
                }

                if (eastidx < westidx)
                {
                    paircnt += eastcnt;

                    //if exceeed 1 billion set to -1
                    if (paircnt > 1000000000)
                    {
                        paircnt = -1;
                        break;
                    }
                }
            }

            return paircnt;
        }


        /// <summary>
        /// given three integers A, B and K, returns the number of integers within the range [A..B] that are divisible by K, i.e.:
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="K"></param>
        /// <returns></returns>
        public static int solution_CountDiv(int A, int B, int K)
        {
            if (A == B)
                return A % K == 0 ? 1 : 0;

            //get total number of divisibles from 1 to B
            int totalDivs = (int)Math.Floor(((decimal) B)/K);

            //negate the lower range numbers
            //if A is 0, it will in fact add a 1 since 0 is divisible by any number
            int exclude = (int)Math.Floor(((decimal)(A-1)) / K);

            return totalDivs - exclude;

            //below seems to work, but I cannot prove it.
            //return ((B % K) < (A % K) || A % K == 0) ? (B - A) / K + 1 : (B - A) / K;
        }

        /// <summary>
        /// Find the maximal triplet of any triplet
        /// A non-empty zero-indexed array A consisting of N integers is given. The product of triplet (P, Q, R) equates to A[P] * A[Q] * A[R] (0 ≤ P < Q < R < N).
        ///
        /// For example, array A such that:

        ///A[0] = -3
        ///A[1] = 1
        ///A[2] = 2
        ///A[3] = -2
        ///A[4] = 5
        ///A[5] = 6
        ///  contains the following example triplets:
        /// •(0, 1, 2), product is −3 * 1 * 2 = −6
        /// •(1, 2, 4), product is 1 * 2 * 5 = 10
        /// •(2, 4, 5), product is 2 * 5 * 6 = 60
        /// 
        /// the function should return 60, as the product of triplet (2, 4, 5) is maximal.
        /// 
        /// •N is an integer within the range [3..100,000];
        /// •each element of array A is an integer within the range [−1,000..1,000].
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        public static int solution_MaximalTriplet(int[] A)
        {
            // Note: unforutnately the solution below, although it seems elegant, is N^3. Basically a naive solution. A better solution in NlogN requires sorting and a observation.
 
            int oneidx = 0;
            int twoidx = 1;
            int threeidx = 2;
            int maxproduct = int.MinValue;

            while(true)
            {
                int product = A[oneidx] * A[twoidx] * A[threeidx];

                Debug.WriteLine("{0},{1},{2},product={3}",oneidx,twoidx,threeidx, product);

                if (product > maxproduct)
                    maxproduct = product;

                threeidx++;

                if (threeidx == A.Length)
                {
                    twoidx++;
                    threeidx = twoidx + 1;

                    if (twoidx == A.Length - 1)
                    {
                        oneidx++;
                        twoidx = oneidx + 1;
                        threeidx = twoidx + 1;
                                  
                        if (oneidx == A.Length - 2)
                            break;
                    }
                }

            }

            return maxproduct;
        }

        /// <summary>
        /// A Better solution of the maximum product
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        public static int solution_MaximalTriplet2(int[] A)
        {
            //a naive solution is in O(N**3), but a sort and a simple observation can make it N Log N
            //important observation: multiplication is commutative, the order doesn't matter when we want a product of 3.
            //e.g. (2,1,0) gives the same value as (0,1,2), therefore, we can actually choose any 3 elements in the array for multiplication
            //this is the reason why we can sort

            //use a sort func, ascending (default)
            Array.Sort(A, (x, y) => x.CompareTo(y));

            //2nd important observation is that a negative*negative will be positive. THerefore,
            //as 1 candidate, we can take the 2 smallest negative numbers, which are assumed to be A[0] and A[1], as well as take the largest            //positive number. given by the last element in the sort.if there is only 1 negative number, this will naturally be a failed     candidate so it doesn't matter.
            //
            //take the case of [5,6,-1000,1,2,1000,-1000], the answer is 1 billion (-1000*1000*-1000)
            //The other candidate is the 3 most largest positive number, assumed to be in the last 3 elements of the sorted array.
            int candidate1 = A[0] * A[1] * A[A.Length - 1];
            int candidate2 = A[A.Length - 1] * A[A.Length - 2] * A[A.Length - 3];

            return (candidate1 > candidate2) ? candidate1 : candidate2;
        }

        /// <summary>
        /// given a zero-indexed array A consisting of N integers, returns the number of distinct values in array A.
        /// 
        /// •N is an integer within the range [0..100,000];
        /// •each element of array A is an integer within the range [−1,000,000..1,000,000].
        /// 
        /// A[0] = 2    A[1] = 1    A[2] = 1
        ///A[3] = 2    A[4] = 3    A[5] = 1
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        public static int solution_DisctinctVals(int[] A)
        {
            //The trick of this question is that null array values are possible
            if (A.Length == 0)
                return 0;

            //sort the array first
            Array.Sort(A, (x, y) => x.CompareTo(y));

            int distinctcnt = 1;
            int currentval = A[0];
            //iterate through counting distinct elements
            for (int i = 0; i < A.Length; i++)
            {
                if (currentval != A[i])
                {
                    distinctcnt++;
                    currentval = A[i];
                }
            }
            return distinctcnt;
        }

        /// <summary>
        /// A zero-indexed array A consisting of N integers is given. A triplet (P, Q, R) is triangular if 0 ≤ P < Q < R < N and:
        /// 
        /// •A[P] + A[Q] > A[R],
        /// •A[Q] + A[R] > A[P],
        /// •A[R] + A[P] > A[Q].
        /// 
        /// For example, consider array A such that:
        ///A[0] = 10    A[1] = 2    A[2] = 5
        ///A[3] = 1     A[4] = 8    A[5] = 20
        /// Triplet (0, 2, 4) is triangular.
        /// 
        /// that, given a zero-indexed array A consisting of N integers, returns 1 if there exists a triangular triplet for this array and returns 0 otherwise.
        /// 
        /// •N is an integer within the range [0..100,000];
        ///•each element of array A is an integer within the range [−2,147,483,648..2,147,483,647].
        /// 
        /// •expected worst-case time complexity is O(N*log(N));
        /// •expected worst-case space complexity is O(N), beyond input storage (not counting the storage required for input arguments).
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        public static int solution_ExistsTriangularTriplet(int[] A)
        {
            // write your code in C# 5.0 with .NET 4.5 (Mono)
            //basically observe that if the sum of 2 edges is bigger than the remaining edge, it is triangular
            //the other observation: we don't need to calculdate the number of triangular triplets. If we find one , we can stop searching
            //Key edge case: N can be 0 => empty array is possible


            if (A.Length == 0)
                return 0;

            //Do a sort first so we can index adjacent elements easily
            Array.Sort(A, (x,y) => x.CompareTo(y));

            //iterate through array to find a case of triangularity
            for (int i = 0; i < A.Length - 2; i++)
            {
                //check triangular property
                if ((long)A[i] + A[i + 1] > A[i + 2] &&
                    (long)A[i + 1] + A[i + 2] > A[i] &&
                    (long)A[i + 2] + A[i] > A[i + 1])
                    return 1;
            }
            return 0;
        }

        /// <summary>
        /// A string S consisting of N characters is considered to be properly nested if any of the following conditions is true:
        /// 
        /// •S is empty;
        /// •S has the form "(U)" or "[U]" or "{U}" where U is a properly nested string;
        /// •S has the form "VW" where V and W are properly nested strings.
        /// 
        /// For example, the string "{[()()]}" is properly nested but "([)()]" is not.
        /// 
        /// that, given a string S consisting of N characters, returns 1 if S is properly nested and 0 otherwise.
        /// 
        /// For example, given S = "{[()()]}", the function should return 1 and given S = "([)()]", the function should return 0, as explained above.
        /// 
        /// •N is an integer within the range [0..200,000];
        /// •string S consists only of the following characters: "(", "{", "[", "]", "}" and/or ")".
        /// 
        /// •expected worst-case time complexity is O(N);
        /// •expected worst-case space complexity is O(N) (not counting the storage required for input arguments).
        /// 
        /// </summary>
        /// <param name="S"></param>
        /// <returns></returns>
        public static int solution_ProperlyNested(string S)
        {
            // write your code in C# 5.0 with .NET 4.5 (Mono)
            // an Empty string is a special case
            // sounds like a stack problem, since if I encounter an opening bra, I should encounter a paired closing cket at the same level

            Stack<char> charStack = new Stack<char>(S.Length/2);
            int invalidKetsCnt = 0;

            foreach (char c in S)
            {
                switch (c)
                {
                    case '(':
                    case '{':
                    case '[':
                        charStack.Push(c);
                        break;
                    case ')':
                    case '}':
                    case ']':
                        //Check stack size first as a peek causes exception on an empty stack
                        if (charStack.Any())
                        {
                            char k = charStack.Peek();
                            if (k == '(' && c == ')' ||
                                k == '[' && c == ']' ||
                                k == '{' && c == '}')
                            {
                                charStack.Pop();
                            }
                        }
                        //edge case. A ket can happen without any bras. in this case this is an invalid nest
                        else
                        {
                            invalidKetsCnt++;
                        }
                        break;
                }
            }

            if (!charStack.Any() && invalidKetsCnt == 0)
                return 1;

            return 0;
        }

        /// <summary>
        /// You are given two non-empty zero-indexed arrays A and B consisting of N integers. Arrays A and B represent N voracious fish in a river, ordered downstream along the flow of the river.
        /// The fish are numbered from 0 to N − 1. If P and Q are two fish and P < Q, then fish P is initially upstream of fish Q. Initially, each fish has a unique position.
        /// 
        /// Fish number P is represented by A[P] and B[P]. Array A contains the sizes of the fish. All its elements are unique. Array B contains the directions of the fish. It contains only 0s and/or 1s, where:
        /// 
        /// •0 represents a fish flowing upstream,
        /// •1 represents a fish flowing downstream.
        /// 
        /// If two fish move in opposite directions and there are no other (living) fish between them, they will eventually meet each other. Then only one fish can stay alive − the larger fish eats the smaller one. More precisely, we say that two fish P and Q meet each other when P < Q, B[P] = 1 and B[Q] = 0, and there are no living fish between them. After they meet:
        /// 
        /// •If A[P] > A[Q] then P eats Q, and P will still be flowing downstream,
        /// •If A[Q] > A[P] then Q eats P, and Q will still be flowing upstream.
        /// We assume that all the fish are flowing at the same speed. That is, fish moving in the same direction never meet. The goal is to calculate the number of fish that will stay alive.
        /// 
        /// For example, consider arrays A and B such that:
        /// A[0] = 4    B[0] = 0
        ///A[1] = 3    B[1] = 1
        ///A[2] = 2    B[2] = 0
        ///A[3] = 1    B[3] = 0
        ///A[4] = 5    B[4] = 0
        /// 
        /// Initially all the fish are alive and all except fish number 1 are moving upstream. Fish number 1 meets fish number 2 and eats it, then it meets fish number 3 and eats it too. Finally, it meets fish number 4 and is eaten by it. The remaining two fish, number 0 and 4, never meet and therefore stay alive.
        /// 
        /// that, given two non-empty zero-indexed arrays A and B consisting of N integers, returns the number of fish that will stay alive.
        /// 
        /// •N is an integer within the range [1..100,000];
        /// •each element of array A is an integer within the range [0..1,000,000,000];
        /// •each element of array B is an integer that can have one of the following values: 0, 1;
        /// •the elements of A are all distinct.
        /// 
        /// •expected worst-case time complexity is O(N);
        /// •expected worst-case space complexity is O(N), beyond input storage (not counting the storage required for input arguments).
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static int solution_FishAlive(int[] A, int[] B)
        {
            // write your code in C# 5.0 with .NET 4.5 (Mono)
            //// sounds like we need a loop or several O(N) loops
            // we can use an additional data strcuture to store values of A or B
            // sounds like a stack problem (0 is met by 1 and either one eaten)
            // we can push when encountering a 0 and pop when 0 is eaten by 1, or not
            // naively, every 1 feels like it should be looped inner, but cannot simply do an inner loop when multiple 1 exists, since we need O(N)

            Stack<int> fishstack = new Stack<int>();

            for (int i = 0; i < A.Length; i++)
            {
                int cursize = A[i];
                int curdir = B[i];

                if (!fishstack.Any())
                    fishstack.Push(i);
                else
                {
                    //condition 1: if there's any fish on the stack
                    //condition 2 : if there is a fish going downstream on the stack and meets the current one that goes upstream
                    //condition 3 : if the downstream fish is smaller than the current fish going upstream
                    while (fishstack.Any() &&
                           curdir - B[fishstack.Peek()] == -1 &&
                           A[fishstack.Peek()] < cursize)
                    {
                        //kill the downstream fish
                        fishstack.Pop();
                    }

                    //if not empty
                    if (fishstack.Any())
                    {
                        //if the fish on the stack is going upstream but is already pass the position of the current fish going downstream
                        //then, keep the fish as being alive by pushing it on stack
                        if (curdir - B[fishstack.Peek()] != -1)
                            fishstack.Push(i);
                    }
                    //else if fishstack is empty, push
                    else
                    {
                        fishstack.Push(i);
                    }
                }
            }
            return fishstack.Count();
         
        }

        /// <summary>
        /// My solution to the fish problem, but more intuitive compared to some other solutions
        /// unfortunately, it fails for 50% of the cases.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static int solution_FishAlive2(int[] A, int[] B)
        {
            Stack<int> stack = new Stack<int>();

            //populate the stack
            for (int i = 0; i < A.Length; i++)
            {
                int curdir = B[i];
                int cursize = A[i];

                if (!stack.Any())
                    stack.Push(i);
                else
                {
                    //condition 1: if there's any fish on the stack
                    //condition 2 : if there is a fish going downstream on the stack and meets the current one that goes upstream
                    //condition 3 : if the downstream fish is smaller than the current fish going upstream
                    while (stack.Any() &&
                           curdir - B[stack.Peek()] == -1 &&
                           A[stack.Peek()] < cursize)
                    {
                        //kill the downstream fish
                        stack.Pop();
                    }

                    //if not empty
                    if (stack.Any())
                    {
                        //if the fish on the stack is going upstream but is already pass the position of the current fish going downstream
                        //then, keep the fish as being alive by pushing it on stack
                        if (curdir - B[stack.Peek()] != -1)
                            stack.Push(i);
                    }
                    //else if fishstack is empty, push
                    else
                    {
                        stack.Push(i);
                    }

                }
            }


            return stack.Count;
        }

        /// <summary>
        /// Solution to this task can be found at our blog.
        /// http://blog.codility.com/2012/06/sigma-2012-codility-programming.html
        /// 
        /// You are going to build a stone wall. 
        /// The wall should be straight and N meters long, and its thickness should be constant; 
        /// however, it should have different heights in different places. 
        /// The height of the wall is specified by a zero-indexed array H of N positive integers. 
        /// H[I] is the height of the wall from I to I+1 meters to the right of its left end. 
        /// In particular, H[0] is the height of the wall's left end and H[N−1] is the height of the wall's right end.
        /// 
        /// The wall should be built of cuboid stone blocks (that is, all sides of such blocks are rectangular). 
        /// Your task is to compute the minimum number of blocks needed to build the wall.
        /// 
        /// that, given a zero-indexed array H of N positive integers specifying the height of the wall, 
        /// returns the minimum number of blocks needed to build it.
        /// 
        /// For example, given array H containing N = 9 integers:
        /// 
        /// H[0] = 8    H[1] = 8    H[2] = 5
        /// H[3] = 7    H[4] = 9    H[5] = 8
        /// H[6] = 7    H[7] = 4    H[8] = 8
        /// 
        /// •N is an integer within the range [1..100,000];
        /// •each element of array H is an integer within the range [1..1,000,000,000].
        /// 
        /// •expected worst-case time complexity is O(N);
        /// •expected worst-case space complexity is O(N), beyond input storage (not counting the storage required for input arguments).
        /// 
        /// </summary>
        /// <param name="H"></param>
        /// <returns></returns>
        public static int solution_Stonewall(int[] H)
        {
            // write your code in C# 5.0 with .NET 4.5 (Mono)
            // observations: 
            // if a same height wall is adjacent, a single block can be used
            // if the height of the wall is unique, a single block must be used to represent that unique height
            // looks like a stack should be used to store the height of each block, then trying to build on it
            // when the stack cannot be used to construct the next wall, should be discarded

            Stack<long> stack = new Stack<long>();
            int blocks = 1;
            stack.Push(H[0]);

            //scan from left to right
            for (int i = 1; i < H.Length; i++)
            {
                long stackheight = stack.Sum();

                if (H[i] < stackheight)
                {
                    if (stack.Count == 1)
                    {
                        blocks++;
                        stack.Pop();
                        stack.Push(H[i]);
                        continue;
                    }

                    bool blockheightexists = false;
                    int sum = 0;
                    foreach (int block in stack.Reverse())
                    {
                        sum += block;
                        
                        //height already exist
                        if (sum == H[i])
                        {
                            blockheightexists = true;
                            while (H[i] < stack.Sum())
                                stack.Pop();

                            break;
                        }
                    }

                    if (!blockheightexists)
                    {
                        blocks++;

                        while(H[i] < stack.Sum())
                            stack.Pop();

                        stack.Push(H[i] - stack.Sum());
                    }

                }
                else if (H[i] > stackheight)
                {
                    blocks++;
                    stack.Push(H[i] - stack.Sum());
                }
            }
            return blocks;
        }


        public static int Partition(int[] numbers, int left, int right)
        {
            int pivot = numbers[left];
            while (true)
            {
                while (numbers[left] < pivot)
                    left++;

                while (numbers[right] > pivot)
                    right--;

                if (left < right)
                {
                    int temp = numbers[right];
                    numbers[right] = numbers[left];
                    numbers[left] = temp;
                }
                else
                {
                    return right;
                }
            }
        }
        static public void QuickSort_Recursive(int[] arr, int left, int right)
        {
            // For Recusrion
            if (left < right)
            {
                int pivot = Partition(arr, left, right);

                if (pivot > 1)
                    QuickSort_Recursive(arr, left, pivot - 1);

                if (pivot + 1 < right)
                    QuickSort_Recursive(arr, pivot + 1, right);
            }
        }
    }
}
