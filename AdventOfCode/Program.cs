using System;
using System.Linq;
using AdventOfCode;


/* change this to your selection */
/* */
/* */ var year = 2020;
/* */ var day = 1;
/* */
/* ----------------------------- */



Console.Clear();
var instance = (IRunnable)Activator.CreateInstance(
    typeof(Day)
        .Assembly
        .GetTypes()
        .First(t => t.FullName == $"AdventOfCode.Event{year}.Day{day}")
);

instance.DayId = day;
instance.Year = year;
await instance.RunAsync();
