
namespace ChatWebSocket;

class Program
{
    static void Main(string[] args)
    {
        var client = new Client();

        while (true)
        {
            try
            {
                Console.WriteLine("Введите строку для отправки...");
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input) || input == "exit")
                {
                    return;
                }
                client.Send(input).GetAwaiter().GetResult();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}