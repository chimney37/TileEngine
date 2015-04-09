using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class StoneWall
    {
        public static int solution_Stonewall(int[] H)
        {
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

                        while (H[i] < stack.Sum())
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
    }
}
