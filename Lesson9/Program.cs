using System.Text;

namespace Lesson9
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            string result = await WaitAndReturnAsync();
            Console.WriteLine(result);

            string filePath = "C:\\Users\\hmeln\\source\\repos\\Lesson9\\Lesson9\\example.txt";
            string textToWrite = "Hello, Async World!";

            await WriteTextAsync(filePath, textToWrite);    
            string readText = await File.ReadAllTextAsync(filePath);
            Console.WriteLine(readText);

            try
            {
                await TaskThatFails();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception caused: { ex.Message}");
            }

            Task<string> task1 = WaitAndReturnStringAsync("Task 1", 1000);
            Task<string> task2 = WaitAndReturnStringAsync("Task 2", 2000);
            Task<string> task3 = WaitAndReturnStringAsync("Task 3", 3000);

            string[] results = await Task.WhenAll(task1, task2, task3);
            foreach (string res in results)
            {
                Console.WriteLine(res);
            }

            await foreach (var number in GenerateSequenceAsync())
            {
                Console.WriteLine(number);
            }

            int num = await GetValuesAsync();
            Console.WriteLine(num);

            Console.ReadKey();
        }

        public static async Task<string> WaitAndReturnAsync()
        {
            await Task.Delay(1000);
            return "Completed";
        }

        public static async Task WriteTextAsync(string filePath, string text)
        {
            byte[] encodedText = Encoding.Unicode.GetBytes(text);
            using (FileStream sourceStream = new FileStream(filePath,
                FileMode.Create, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true))
            {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            }
        }

        public static async Task TaskThatFails()
        {
            await Task.Delay(1000);
            throw new InvalidOperationException("Failed!");
        }

        public static async Task<string> WaitAndReturnStringAsync(string name, int delay)
        {
            await Task.Delay(delay);
            return $"{name} completed";
        }

        public static async IAsyncEnumerable<int> GenerateSequenceAsync()
        {
            for (int i = 0; i < 5; i++)
            {
                await Task.Delay(1000);
                yield return i;
            }
        }

        public static async ValueTask<int> GetValuesAsync()
        {
            await Task.Delay(1000);
            return 42;
        }
    }
}