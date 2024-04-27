var a = int.Parse(ReadLine());
var bc = ReadLine().Split().Select(int.Parse).ToArray();
var (b, c) = (bc[0], bc[1]);
var s = ReadLine();

WriteLine($"{a + b + c} {s}");

