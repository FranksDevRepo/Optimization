﻿using Bss.Optimization.Sudoku.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bss.Optimization.Sudoku.Extensions;

namespace Bss.Optimization.Sudoku
{
    class Program
    {
        static void Main(string[] args)
        {
            // Uncomment the model you wish to use
            var model = new Bss.Optimization.Sudoku.GoogleCp.Model();
            // var model = new Bss.Optimization.Sudoku.MSSolverCp.Model();

            // Uncomment the puzzle you want to solve
            var puzzleHints = (null as IEnumerable<GridCell>).CreateDemoPuzzle();
            // var puzzleHints = (null as IEnumerable<GridCell>).CreateEasyPuzzle();
            // var puzzleHints = (null as IEnumerable<GridCell>).CreateHardPuzzle();
            // var puzzleHints = (null as IEnumerable<GridCell>).CreateMultipleSolutionPuzzle();
            // var puzzleHints = (null as IEnumerable<GridCell>).CreateTooManySolutionPuzzle();

            var results = model.Solve(puzzleHints);

            // Display results
            Console.WriteLine($"Solver used: {model.SolverName}");
            Console.WriteLine($"Number of valid solutions: {results.Count()}");
            foreach (var result in results)
            {
                Console.WriteLine(result.GetDisplay());
                Console.WriteLine("");
            }
            Console.WriteLine($"Number of valid solutions: {results.Count()}");
        }
    }
}
