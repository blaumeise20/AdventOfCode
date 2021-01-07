# Advent of Code solutions

These are my solutions for the [Advent of Code](https://adventofcode.com) puzzles.
Currently they are not documented really good, just look into the code.
Warning: [Day 17 - 2020](AdventOfCode/Event2020/Day17.cs) is currently a mess, but atleast it works.

## Running the code

Make sure to select the right day in [Program.cs](AdventOfCode/Program.cs). You also have to create a file called `day{x}Input.txt` ({x} is the day the input is for) in the correct year folder. Open the file properties and select "Copy to output directory". You should be ready to go.

## Day API

### Class `Day<T>`
This is the ultimate base class for all solutions. You have to implement it correctly for it to work.

**Class `Day` (without `T`)**

This class is just derrived from `Day<string>`. Use this base class if your input line type is `string`.

### `T List<T>.Shift()`
Extension Method. Removes the first element from a list and returns it.

### `List<T> List<T>.Shift(int count)`
Extension Method. Removes `count` elements from the start of the list and returns them.

### `int|long|float IEnumerable<int|long|float>.Multiply()`
Extension Method. Multiplies the values. Like `.Aggregate((a, b) => a * b)

### `string string.Reverse()`
Extension Method. Reverses a string.

### `int IEnumerable<T>.CountValue(T value)`
Extension Method. Counts the occurrences of `value`.


*More documentation comming soon.*
