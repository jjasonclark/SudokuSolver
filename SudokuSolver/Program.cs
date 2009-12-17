using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var board = " 5 9 7 1   3   9   71   28   9   1   3 1 5 2  87   69 6   8   9   4 2       6    ";
            //var board = "53  7    6  195    98    6 8   6   34  8 3  17   2   6 6    28    419  5    8  79";
            //var superHardBoard = "              3 85  1 2       5 7     4   1   9       5      73  2 1        4   9";
            Console.WriteLine(Time(Solve, board));
            Console.WriteLine(Time(SolveStrings, board));
            Console.ReadLine();
        }
        public static string Time(Func<string, string> solve, string board)
        {
            var start = DateTime.Now.Ticks;
            System.Console.WriteLine("Solve: {0}", solve(board));
            var end = DateTime.Now.Ticks;
            return String.Format("Took {0}", end - start);
        }

        public static string Solve(string Board)
        {
            string[] leafNodesOfMoves = new string[] { Board };
            while ((leafNodesOfMoves.Length > 0) && (leafNodesOfMoves[0].IndexOf(' ') != -1))
            {
                leafNodesOfMoves = (
                    from partialSolution in leafNodesOfMoves
                    let index = partialSolution.IndexOf(' ')
                    from numberToTry in Enumerable.Range(1, 9)
                    let searchLetter = numberToTry.ToString()[0]
                    let InvalidPositions =
                    from spaceToCheck in Enumerable.Range(0, 9)
                    let IsInRow = partialSolution[(int)Math.Floor(index / 9f) * 9 + spaceToCheck] == searchLetter
                    let IsInColumn = partialSolution[index % 9 + (spaceToCheck * 9)] == searchLetter
                    let IsInGroupBoxOf3x3 = partialSolution[(int)Math.Floor(index % 9f / 3) * 3 +
                                (int)Math.Floor(index / 27f) * 27 +
                                (int)Math.Floor(spaceToCheck / 3f) * 9 + spaceToCheck % 3] == searchLetter
                    where IsInRow || IsInColumn || IsInGroupBoxOf3x3
                    select spaceToCheck
                    where InvalidPositions.Count() == 0
                    select partialSolution.Substring(0, index) + searchLetter + partialSolution.Substring(index + 1)
                        ).ToArray();
            }
            return (leafNodesOfMoves.Length == 0)
                ? "No solution"
                : leafNodesOfMoves[0];
        }

        public static string SolveStrings(string Board)
        {
            string[] leafNodesOfMoves = new string[] { Board };
            while ((leafNodesOfMoves.Length > 0) && (leafNodesOfMoves[0].IndexOf(' ') != -1))
            {
                leafNodesOfMoves = (
                    from partialSolution in leafNodesOfMoves
                    let index = partialSolution.IndexOf(' ')
                    let column = index % 9
                    let groupOf3 = index - (index % 27) + column - (index % 3)
                    from searchLetter in "123456789"
                    let InvalidPositions =
                    from spaceToCheck in Enumerable.Range(0, 9)
                    let IsInRow = partialSolution[index - column + spaceToCheck] == searchLetter
                    let IsInColumn = partialSolution[column + (spaceToCheck * 9)] == searchLetter
                    let IsInGroupBoxOf3x3 = partialSolution[groupOf3 + (spaceToCheck % 3) +
                        (int)Math.Floor(spaceToCheck / 3f) * 9] == searchLetter
                    where IsInRow || IsInColumn || IsInGroupBoxOf3x3
                    select spaceToCheck
                    where InvalidPositions.Count() == 0
                    select partialSolution.Substring(0, index) + searchLetter + partialSolution.Substring(index + 1)
                        ).ToArray();
            }
            return (leafNodesOfMoves.Length == 0)
                ? "No solution"
                : leafNodesOfMoves[0];
        }
    }
}
