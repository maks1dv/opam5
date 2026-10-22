using System;
using System.Collections.Generic;

namespace Lab05_StackQueue
{
    public class Request
    {
        public int Id { get; set; }
        public int Weight { get; set; }
        public int ServerId { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
    }

    public static class Task3_QueueService
    {
        // c = 3: два обслуговувачі беруть заявки з однієї спільної черги
        public static void RunSimulation(int studentN, int studentK)
        {
            int requestCount = 8 + (studentK % 5); // 10 заявок
            Random rnd = new Random(studentN);     // Random(8)

            Queue<Request> sharedQueue = new Queue<Request>();

            for (int i = 1; i <= requestCount; i++)
            {
                int weight = rnd.Next(1, 10);
                sharedQueue.Enqueue(new Request { Id = i, Weight = weight });
            }

            int maxQueueLength = sharedQueue.Count;
            int server1FreeTime = 0;
            int server2FreeTime = 0;

            List<Request> processedRequests = new List<Request>();

            while (sharedQueue.Count > 0)
            {
                if (sharedQueue.Count > maxQueueLength)
                {
                    maxQueueLength = sharedQueue.Count;
                }

                Request currentReq = sharedQueue.Dequeue();

                if (server1FreeTime <= server2FreeTime)
                {
                    currentReq.ServerId = 1;
                    currentReq.StartTime = server1FreeTime;
                    server1FreeTime += currentReq.Weight;
                    currentReq.EndTime = server1FreeTime;
                }
                else
                {
                    currentReq.ServerId = 2;
                    currentReq.StartTime = server2FreeTime;
                    server2FreeTime += currentReq.Weight;
                    currentReq.EndTime = server2FreeTime;
                }

                processedRequests.Add(currentReq);
            }

            Console.WriteLine($"\n--- Завдання 3: Модель обслуговування (Схема c = 3) ---");
            Console.WriteLine($"Всього заявок: {requestCount}, Максимальна довжина черги: {maxQueueLength}\n");
            Console.WriteLine($"{"№ Заявки",-10} | {"Вага (хв)",-10} | {"Обслуговувач",-15} | {"Час початку",-12} | {"Час завершення",-15}");
            Console.WriteLine(new string('-', 72));

            foreach (var req in processedRequests)
            {
                Console.WriteLine($"{req.Id,-10} | {req.Weight,-10} | {$"Сервер {req.ServerId}",-15} | {req.StartTime,-12} | {req.EndTime,-15}");
            }
        }
    }
}