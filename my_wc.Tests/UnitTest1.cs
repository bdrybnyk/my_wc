using NUnit.Framework;
using System.IO;
using my_wc;

namespace my_wc.Tests
{
    public class Tests
    {
        [Test]
        public void Test_Success_ExitCode()
        {
            var output = new StringWriter();
            var error = new StringWriter();
            var input = new StringReader("hello world");
            
            int result = App.Run(new[] { "-w" }, input, output, error);

            Assert.That(result, Is.EqualTo(0));
            Assert.That(output.ToString().Trim(), Is.EqualTo("2"));
        }

        [Test]
        public void Test_FileNotFound_Stderr()
        {
            var output = new StringWriter();
            var error = new StringWriter();
            
            int result = App.Run(new[] { "missing_file.txt" }, Console.In, output, error);

            Assert.That(result, Is.EqualTo(1));
            Assert.That(error.ToString(), Does.Contain("No such file or directory")); // Помилка в stderr [cite: 92]
        }
    }
}