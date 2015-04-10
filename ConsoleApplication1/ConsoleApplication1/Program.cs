using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Xml.Schema;

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

            int[] arrayA = { 1, 2, 3, 4, 5, -1, -2, -3 };
            int[] arrayA2 = { 1,2,1,2,2,2 };
            Debug.WriteLine(Solution.solution_EquiLeader(arrayA2));
            Debug.WriteLine(Solution.solution_Dominator(arrayA));
        }
    }

    internal class Solution
    {
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
