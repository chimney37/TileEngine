using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class FishAlive
    {
        public static int solution_FishAlive2(int[] A, int[] B)
        {
            //implementation of codility solution in C# based on http://rafal.io/posts/codility-fish.html
            Stack<int> stack = new Stack<int>();

            //populate the stack
            for (int i = 0; i < A.Length; i++)
            {
                int curfishdir = B[i];
                int curfishsize = A[i];

                if (!stack.Any())
                    stack.Push(i);
                else
                {
                    //condition 1: if there's downstream fish on the stack
                    //condition 2 : if there is a fish going downstream on the stack and meets the current one that goes upstream
                    //condition 3 : if the downstream fish is smaller than the current fish going upstream
                    while (stack.Any() &&
                           curfishdir - B[stack.Peek()] == -1 &&
                           A[stack.Peek()] < curfishsize)
                    {
                        //kill the downstream fish
                        stack.Pop();
                    }

                    //if not empty
                    if (stack.Any())
                    {
                        //if the fish on the stack is going upstream but is already pass the position of the current fish, if going downstream
                        //then, keep the fish as being alive by pushing it on stack
                        if (curfishdir - B[stack.Peek()] != -1)
                            stack.Push(i);
                    }
                    //else if fishstack is empty, push the fish which is still alive
                    else
                    {
                        stack.Push(i);
                    }

                }
            }


            return stack.Count;
        }

    }
}
