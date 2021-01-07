using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode
{
    public interface IRunnable
    {
        int DayId { set; }
        int Year { set; }
        Task RunAsync();
    }
    public abstract class Day : Day<string>
    {
        protected override string MapLines(string line) => line;
    }
    public abstract class Day<T> : IRunnable
    {
        public int DayId { get; set; }
        public int Year { get; set; }
        protected abstract bool Part2Done { get; }
        protected virtual int RunPart1 => 1;
        protected virtual int RunPart2 => 1;

        protected abstract T MapLines(string line);
        protected virtual T[] ManipulateLines(T[] lines) => lines;

        protected virtual void Init(T[] lines) { }
        protected abstract void Part1(T[] lines);
        protected abstract void Part2(T[] lines);

        protected TPrint Print<TPrint>(string label, TPrint value, bool result = false)
        {
            Console.WriteLine($"\u001b[1m{label}:\u001b[0m {(result ? "\u001b[1;4;33m" : "")}{value}{(result ? "\u001b[0m" : "")}");
            return value;
        }

        public async Task RunAsync()
        {
            Console.WriteLine($"\u001b[1;44mDay {DayId} - Advent of Code\u001b[0m\n");

            var filename = $"Event{Year}/day{DayId}Input.txt";
            if (!File.Exists(filename))
            {
                Console.WriteLine($"\u001b[1;31mError: No input file found for day {DayId} ({filename}). Make sure to enable \"Copy to output directory\".\u001b[0m");
                return;
            }

            var lines = await File.ReadAllLinesAsync(filename);
            var funcInput = ManipulateLines(lines.Select(l => MapLines(l)).ToArray());

            Init(funcInput);

            Console.WriteLine("\u001b[1;32m------ Part 1 ------\u001b[0m");
            var time = new Stopwatch();
            try
            {
                if (RunPart1 == 1)
                {
                    time.Start();
                    Part1(funcInput);
                    time.Stop();
                    Print("Elapsed time", time.Elapsed + "\n");
                }
                else if (RunPart1 == 0)
                {
                    Console.WriteLine("\u001b[32mDoesn't execute because execution count is set to zero\u001b[0m");
                }
                else
                {
                    for (int i = 0; i < RunPart1; i++)
                    {
                        Console.WriteLine($"\u001b[32mExecution {i + 1}\u001b[0m");
                        time.Restart();
                        Part1(funcInput);
                        time.Stop();
                        Print("Elapsed time", time.Elapsed);
                    }
                    Console.WriteLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\u001b[1;31mError:\u001b[0m");
                Console.WriteLine(e);
                time.Stop();
                return;
            }

            if (Part2Done)
            {
                Console.WriteLine("\u001b[1;32m------ Part 2 ------\u001b[0m");
                try
                {
                    if (RunPart2 == 1)
                    {
                        time.Restart();
                        Part2(funcInput);
                        time.Stop();
                        Print("Elapsed time", time.Elapsed + "\n");
                    }
                    else if (RunPart2 == 0)
                    {
                        Console.WriteLine("\u001b[32mDoesn't execute because execution count is set to zero\u001b[0m");
                    }
                    else
                    {
                        for (int i = 0; i < RunPart2; i++)
                        {
                            Console.WriteLine($"\u001b[32mExecution {i + 1}\u001b[0m");
                            time.Restart();
                            Part2(funcInput);
                            time.Stop();
                            Print("Elapsed time", time.Elapsed);
                        }
                        Console.WriteLine();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"\u001b[1;31mError:\u001b[0m");
                    Console.WriteLine(e);
                    time.Stop();
                    return;
                }
            }
            else
            {
                Console.WriteLine("\u001b[1;32mPart 2 not done\u001b[0m");
            }
        }
    }
    public static class DayExtensions
    {
        public static T Shift<T>(this List<T> list)
        {
            var item = list[0];
            list.RemoveAt(0);
            return item;
        }
        public static List<T> Shift<T>(this List<T> list, int count)
        {
            var item = list.GetRange(0, count);
            list.RemoveRange(0, count);
            return item;
        }
        public static int Multiply(this IEnumerable<int> list) => list.Aggregate((a, b) => a * b);
        public static long Multiply(this IEnumerable<long> list) => list.Aggregate((a, b) => a * b);
        public static float Multiply(this IEnumerable<float> list) => list.Aggregate((a, b) => a * b);
        public static string Reverse(this string text)
        {
            char[] array = text.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }
        public static int CountValue<T>(this IEnumerable<T> list, T value) => list.Count(x => EqualityComparer<T>.Default.Equals(x, value));
    }
}