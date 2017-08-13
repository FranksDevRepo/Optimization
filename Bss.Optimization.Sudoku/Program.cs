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
            var model = new Bss.Optimization.Sudoku.GoogleCp.Model();

            var puzzleHints = (null as IEnumerable<GridCell>).CreateDemoPuzzle();
            var results = model.Solve(puzzleHints);

            // Display results
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
