using System;
using System.Threading;
using System.Threading.Tasks;

namespace MediaTekDocuments.dal
{
    public class RequestManager
    {
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(3); // Limite à 3 requêtes simultanées
        private const int MaxRetry = 3;
        private const int DelayMs = 1000;
        private static int totalRequests = 0;

        public static async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
        {
#if DEBUG
            int currentRequest = Interlocked.Increment(ref totalRequests);
            Console.WriteLine($"[DEBUG] Nouvelle requête envoyée. Total en cours : {currentRequest}");
#endif
            int retry = 0;
            while (true)
            {
                await semaphore.WaitAsync();
                try
                {
#if DEBUG
                    Console.WriteLine($"[DEBUG] Requête traitée par le thread {Thread.CurrentThread.ManagedThreadId}. Slots disponibles : {semaphore.CurrentCount}");
#endif
                    return await action();
                }
                catch (Exception ex)
                {
#if DEBUG
                    Console.WriteLine($"[DEBUG] Exception : {ex.Message}");
#endif
                    if (retry++ < MaxRetry && ex.Message.Contains("Too many packets"))
                    {
#if DEBUG
                        Console.WriteLine($"[DEBUG] Retry {retry} après erreur 'Too many packets'. Attente {DelayMs * retry} ms.");
#endif
                        await Task.Delay(DelayMs * retry);
                        continue;
                    }
                    throw;
                }
                finally
                {
                    semaphore.Release();
#if DEBUG
                    int afterRelease = Interlocked.Decrement(ref totalRequests);
                    Console.WriteLine($"[DEBUG] Requête terminée. Total en cours : {afterRelease}");
#endif
                }
            }
        }
    }
}