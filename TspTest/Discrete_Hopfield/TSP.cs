﻿using System;
using System.Collections.Generic;
using System.Drawing;
using MathNet.Numerics.LinearAlgebra;

namespace TspTest.Discrete_Hopfield
{
    struct City
    {
        public string Name { set; get; }
        public PointF Position { set; get; }
    }

    class TSP
    {
        private List<City> _cities { set; get; }
        public int CitiesNumber { get; }
        public Matrix<double> D { get; }

        public TSP(List<City> cities)
        {
            _cities = cities;
            CitiesNumber = cities.Count;
            D = Matrix<double>.Build.Dense(cities.Count, cities.Count);
            cities.ForEach(
                e1 =>
                    cities.ForEach(
                        e2 =>
                            D[cities.IndexOf(e1), cities.IndexOf(e2)] =
                                (int)
                                    Math.Sqrt(Math.Pow(e2.Position.X - e1.Position.X, 2) +
                                              Math.Pow(e2.Position.Y - e1.Position.Y, 2))));
        }

        public void Solve()
        {
            DiscreteHopfield nn = new DiscreteHopfield(this);
            _generateSolution(nn.Iterate());
        }

        private void _generateSolution(Tuple<Matrix<double>, List<double>> sol)
        {
            Console.WriteLine("Energy Values:");
            for (int i = 0; i < sol.Item2.Count; i++)
            {
                Console.WriteLine("Epoch: {0} => Energy = {1}", i+1, sol.Item2[i]);
            }
            Console.WriteLine("******************\nTour:");
            for (int i = 0; i < CitiesNumber; i++)
            {
                for (int j = 0; j < CitiesNumber; j++)
                {
                    if(sol.Item1[i*CitiesNumber+j, 0] == 1)
                        Console.WriteLine("Step: {0} => City: {1}", j, _cities[i].Name);
                }
            }
        }


    }
}
