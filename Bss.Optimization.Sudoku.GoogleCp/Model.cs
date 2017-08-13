﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bss.Optimization.Sudoku.Entities;
using Bss.Optimization.Sudoku.Interfaces;
using Google.OrTools.ConstraintSolver;

namespace Bss.Optimization.Sudoku.GoogleCp
{
    public class Model : ISudokuSolver
    {

        public IEnumerable<GridCell[]> Solve(IEnumerable<GridCell> hints)
        {
            var model = new Solver("CPSolver");

            // Create variables
            var x = new IntVar[81];
            for (int i = 0; i < 81; i++)
                x[i] = model.MakeIntVar(1, 9, $"x[{i}]");

            // Create constraints
            for (int group = 0; group < 9; group++)
            {
                // Create row constraints
                IntVarVector row = new IntVarVector();
                for (int i = 0; i < 9; i++)
                {
                    int index = (group * 9) + i;
                    row.Add(x[index]);
                }
                var rowConstraint = model.MakeAllDifferent(row);
                model.Add(rowConstraint);

                // Create column constraints
                IntVarVector col = new IntVarVector();
                for (int i = 0; i < 9; i++)
                {
                    int index = (i * 9) + group;
                    col.Add(x[index]);
                }
                var colConstraint = model.MakeAllDifferent(col);
                model.Add(colConstraint);

                // Create region constraints
                int regionStartX = (group % 3) * 3;
                int regionStartY = (group / 3) * 3;
                IntVarVector region = new IntVarVector();
                for (int i = 0; i < 9; i++)
                {
                    int deltaX = (i % 3);
                    int deltaY = (i / 3);
                    int xLoc = regionStartX + deltaX;
                    int yLoc = regionStartY + deltaY;
                    int index = (yLoc * 9) + xLoc;
                    region.Add(x[index]);
                }
                var regionConstraint = model.MakeAllDifferent(region);
                model.Add(regionConstraint);
            }

            // Add hints
            if (hints != null)
                foreach (var hint in hints)
                {
                    int index = (hint.Y * 9) + hint.X;
                    model.Add(x[index] == hint.Value);
                }

            DecisionBuilder decisionBuilder = model.MakePhase(x, Solver.INT_VAR_DEFAULT, Solver.INT_VALUE_DEFAULT);
            var optimizationStatus = model.Solve(decisionBuilder);

            if (!optimizationStatus)
                throw new InvalidOperationException("Solution not found");

            var results = new List<GridCell[]>();
            while (model.NextSolution())
            {
                var solution = new GridCell[81];

                for (int i = 0; i < 81; i++)
                {
                    byte xLoc = Convert.ToByte(i % 9);
                    byte yLoc = Convert.ToByte(i / 9);
                    solution[i] = GridCell.Create(xLoc, yLoc, Convert.ToByte(x[i].Value()));
                }

                results.Add(solution);
            }

            return results;
        }

    }
}
