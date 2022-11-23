using Humanizer;
using ImageMagick;
using ImageMagick.Formats;

string[] files = Environment.GetCommandLineArgs().Skip(1).ToArray();

long totalBytes = 0;
long totalSaved = 0;

WebPWriteDefines wwd = new() { Lossless = true, ThreadLevel = true, };
Console.WriteLine("Enter choice:");

int choice = int.Parse(Console.ReadLine() ?? "0");

switch (choice)
{
	case 0:
		wwd = new WebPWriteDefines { Lossless = true, ThreadLevel = true, };
		break;

	case 1:
		wwd = new WebPWriteDefines { Lossless = false, ThreadLevel = true };
		break;

	case 2:
		wwd = new WebPWriteDefines { Lossless = false, ThreadLevel = true, Method = 6 };
		break;
}

foreach (var file in files)
{
	if (!File.Exists(file))
		continue;

	string newFile = file.Replace(Path.GetExtension(file), ".webp");
	long original = new FileInfo(file).Length;
	totalBytes += original;

	Console.Write($"Processing {Path.GetFileName(file)}...");

	using MagickImage mm = new(file);
	mm.Write(newFile, wwd);

	long saved = new FileInfo(newFile).Length;
	totalSaved += saved;

	Console.ForegroundColor = ConsoleColor.Green;
	Console.Write("done! ");
	Console.ForegroundColor = ConsoleColor.DarkCyan;
	Console.Write($"{original.Bytes()}");
	Console.ForegroundColor = ConsoleColor.Cyan;
	Console.Write($" --> {saved.Bytes()}");
	Console.ForegroundColor = ConsoleColor.White;
	Console.WriteLine($" ({(1f - ((float)saved / original)):P} saved)");
	Console.ForegroundColor = ConsoleColor.Gray;
}

Console.WriteLine();

Console.ForegroundColor = ConsoleColor.Green;
Console.Write("OVERALL: ");
Console.ForegroundColor = ConsoleColor.DarkCyan;
Console.Write($"{totalBytes.Bytes()}");
Console.ForegroundColor = ConsoleColor.Cyan;
Console.Write($" --> {totalSaved.Bytes()}");
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine($" ({1f - (((float)totalSaved / totalBytes)):P} saved)");
Console.ForegroundColor = ConsoleColor.Gray;