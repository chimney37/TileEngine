using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

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

            int[] array12 = {-3, 1, 2, -2, 5, 6};
            Debug.WriteLine(Solution.solution9(array12));
        }
    }

    internal class Solution
    {
        /// <summary>
        /// FrogJump
        /// Count minimal number of jumps from position X to Y. 
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="D"></param>
        /// <returns></returns>
        public static int solution(int X, int Y, int D)
        {
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
        public static int solution2(int[] A)
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
        public static int solution3(int[] A)
        {
            int[] diffarray = new int[A.Length - 1];
            int firstsum = 0;
            int overallsum = 0;

            for (int i = 0; i < A.Length; i++)
                overallsum += A[i];

            for (int i = 0; i < A.Length - 1; i++)
            {
                firstsum += A[i];
                diffarray[i] = Math.Abs((overallsum - firstsum) - firstsum);
            }

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
        public static int solution4(int[] A)
        {
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
        public static int solution5(int X, int[] A)
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
        public static int[] solution6(int N, int[] A)
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
        public static int solution7(int[] A)
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
        public static int solution8(int A, int B, int K)
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
        public static int solution9(int[] A)
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

        public static int solution10(int[] A)
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
