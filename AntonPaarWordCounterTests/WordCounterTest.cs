using AntonPaarWordCounter.Implementation;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AntonPaarWordCounterTests
{
    public class WordCounterTest
    {
        Progress<int> progress = null;
        DefaultAntonPaarWordCounter counter = null;
        CancellationTokenSource cancellationToken = null;
        string fileName = "sample.txt";

        [SetUp]
        public void Setup()
        {
            progress = new Progress<int>();
            counter = new DefaultAntonPaarWordCounter();
            cancellationToken = new CancellationTokenSource();
        }

        [Test]
        public void FileDoesNotExistVerify()
        {
            Assert.ThrowsAsync<FileNotFoundException>(async () => await counter.CountAsync("FileDoesNotExists", progress, cancellationToken.Token));
        }

        [Test]
        public async Task SingleLineVerify()
        {
            using (FileStream fs = File.Create(fileName))
            {  
                Byte[] title = new UTF8Encoding(true).GetBytes("Test1 Test2 Test1\nTest2");
                fs.Write(title, 0, title.Length);
            }

            var result = await counter.CountAsync(fileName, progress, cancellationToken.Token);
            Assert.AreEqual(2, result.Data.Single(x => x.Word.Equals("Test1", StringComparison.OrdinalIgnoreCase)).Count);
            Assert.AreEqual(2, result.Data.Single(x => x.Word.Equals("Test2", StringComparison.OrdinalIgnoreCase)).Count);
        }

        [Test]
        public async Task MultipleLinesVerify()
        {
            using (FileStream fs = File.Create(fileName))
            {
                Byte[] title = new UTF8Encoding(true).GetBytes("\r\n\r\n\tTest1 Test2 Test1\nTest2\r\nTest2");
                fs.Write(title, 0, title.Length);
            }
            var result = await counter.CountAsync(fileName, progress, cancellationToken.Token);

            Assert.AreEqual(2, result.Data.Single(x => x.Word.Equals("Test1", StringComparison.OrdinalIgnoreCase)).Count);
            Assert.AreEqual(3, result.Data.Single(x => x.Word.Equals("Test2", StringComparison.OrdinalIgnoreCase)).Count);
        }

        [Test]
        public async Task DescedingOrderVerify()
        {
            using (FileStream fs = File.Create(fileName))
            {
                Byte[] title = new UTF8Encoding(true).GetBytes("\r\n\r\n\tTest1 Test2 Test1\nTest2\r\nTest2");
                fs.Write(title, 0, title.Length);
            }
            var result = await counter.CountAsync(fileName, progress, cancellationToken.Token);

            Assert.IsTrue(result.Data.First().Word.Equals("Test2", StringComparison.OrdinalIgnoreCase));
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
    }
}