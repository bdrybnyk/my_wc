using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace my_wc
{
    public class Program
    {
        public static int Main(string[] args)
        {
            return App.Run(args, Console.In, Console.Out, Console.Error);
        }
    }

    public class App
    {
        public static int Run(string[] args, TextReader stdin, TextWriter stdout, TextWriter stderr)
        {
            bool countLines = args.Contains("-l");
            bool countWords = args.Contains("-w");
            bool countBytes = args.Contains("-c");

            if (!countLines && !countWords && !countBytes)
            {
                countLines = countWords = countBytes = true;
            }

            var files = args.Where(a => !a.StartsWith("-")).ToList();

            if (files.Count == 0)
            {
                string input = stdin.ReadToEnd();
                if (string.IsNullOrEmpty(input)) return 0;
                stdout.WriteLine(FormatOutput(input, countLines, countWords, countBytes, ""));
                return 0;
            }

            int finalExitCode = 0;
            foreach (var file in files)
            {
                if (!File.Exists(file))
                {
                    stderr.WriteLine($"my_wc: {file}: No such file or directory");
                    finalExitCode = 1;
                    continue;
                }

                try {
                    string content = File.ReadAllText(file);
                    long byteCount = new FileInfo(file).Length;
                    stdout.WriteLine(FormatOutput(content, countLines, countWords, countBytes, file, byteCount));
                }
                catch (Exception ex) {
                    stderr.WriteLine($"my_wc: {file}: {ex.Message}");
                    finalExitCode = 1;
                }
            }
            return finalExitCode;
        }

        private static string FormatOutput(string text, bool l, bool w, bool c, string name, long bCount = -1)
        {
            var results = new List<string>();
            if (l) results.Add(text.Split('\n').Length.ToString());
            if (w) results.Add(text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length.ToString());
            if (c) results.Add(bCount >= 0 ? bCount.ToString() : Encoding.UTF8.GetByteCount(text).ToString());
            if (!string.IsNullOrEmpty(name)) results.Add(name);
            return string.Join("\t", results);
        }
    }
}