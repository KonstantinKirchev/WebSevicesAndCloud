namespace SOAClient
{
    using System;
    using DistanceCalculatorService;

    class SoaClient
    {
        static void Main()
        {
            var service = new CalculatorClient();
            
            var responce = service.CalculateDistance(
                new Point { X = 4, Y = 11 },
                new Point { X = -1, Y = 0 }
            );

            Console.WriteLine(responce);
        }
    }
}
