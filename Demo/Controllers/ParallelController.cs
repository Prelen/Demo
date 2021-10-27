using Demo.Data;
using Demo.Data.BaseEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParallelController : ControllerBase
    {
        private readonly IRepository<Table_1> _repo1;
        private readonly IRepository<Table_2> _repo2;
        public ParallelController(IRepository<Table_1> repo1, IRepository<Table_2> repo2)
        {
            _repo1 = repo1; 
            _repo2 = repo2; 
        }

        [HttpGet("simplethread")]
        public IActionResult SimpleThread()
        {
            Thread tr1 = new(RunProcess);
            tr1.Start();
            //Some Processing - 2 threads
            tr1.Join();
            return Ok("Thread Complete");
        }

        [HttpGet("mutex")]
        public IActionResult GetMutex()
        {
            Mutex mutex = new();
            mutex.WaitOne(); //Works as a lock
            RunProcess(); // Shared Resource
            mutex.ReleaseMutex();
            return Ok();
        }

        [HttpGet("semaphore")]
        public IActionResult GetSemaphore()
        {
            Semaphore semaphore = new(5, 5); // (Initial count, max count)
            semaphore.WaitOne();
            RunProcess(); // 5 threads can access here
            semaphore.Release();

            return Ok();
        }
        [HttpGet("serialtest")]
        public IActionResult SerialTest()
        {
            System.Diagnostics.Stopwatch stopwatch = new();
            stopwatch.Start();
            var allRecords = _repo2.GetAll();
            foreach (var record in allRecords)
            {
                record.address = "1 New Place";
                record.city = "New City";
                record.company = "New Company";
                System.Threading.Thread.Sleep(25); // Some processing
                _repo2.Update(record);
            }
            stopwatch.Stop();
            return Ok($"Processed - Serial (Executed time {stopwatch.ElapsedMilliseconds})");
        }

        [HttpGet("paralleltest")]
        public IActionResult ParallelTest()
        {
            System.Diagnostics.Stopwatch stopwatch = new();
            stopwatch.Start();
            var allRecords = _repo1.GetAll();
            Parallel.ForEach(allRecords, record => {
                record.address = "1 New Place";
                record.city = "New City";
                record.company = "New Company";
                System.Threading.Thread.Sleep(25); // Some processing
                _repo1.Update(record);
            });

            stopwatch.Stop();
            return Ok($"Processed - Parallel (Executed time {stopwatch.ElapsedMilliseconds})");
        }

        [HttpGet("simpleprocess")]
        public IActionResult SimpleProcess()
        {
            System.Diagnostics.Stopwatch stopwatch = new();
            stopwatch.Start();
            for (int i = 0; i <= 100; i++)
            {

            }
            stopwatch.Stop();
            string serial = stopwatch.Elapsed.TotalMilliseconds.ToString();
            stopwatch.Reset();
            stopwatch.Start();
            Parallel.For(0, 100, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }, i=> { 
            
            });
            stopwatch.Stop();
            string parallel = stopwatch.Elapsed.TotalMilliseconds.ToString();
            return Ok($"Serial Time to count: {serial} milliseconds. Parallel Time to count: {parallel} milliseconds");
        }

        [HttpGet("AsyncFull")]
        public async  Task<IActionResult> AsyncFull()
        {
            var result = await _repo1.FindAsync(r => r.city.ToUpper().Contains("VER")); //Synchronising because of failure
            return Ok(result);
        }


        [HttpGet("asyncvoid")]
        public async void AsyncVoid()
        {
            var result = await SomeTaskAsync();
            await Task.Delay(2000);
            //throw new Exception("Error occurred");
            Console.WriteLine(result);

        }
        [HttpGet("asyncvoid1")]
        public async Task<string> AsyncVoid1()
        {
            var result = await SomeTaskAsync();
            await Task.Delay(2000);
            throw new Exception("Error occurred");
            return result;

        }


        [HttpGet("nocanceltoken")]
        public async Task<IActionResult> NoCancelToken(CancellationToken cancellationToken)
        {
            string str = await ProcessTextAsync(cancellationToken);
            return Ok(str);
        }

       

        private async Task<string> ProcessTextAsync(CancellationToken cancellationToken)
        {
            return await Task.Run(async () => {
                await Task.Delay(5000);
                return "Processing Text";
            });
        }


        [HttpGet("canceltoken")]
        public async Task<IActionResult> CancelToken(CancellationToken cancellationToken)
        {
            string str = await ProcessTextCancelAsync(cancellationToken);
            return Ok(str);
        }
        private async Task<string> ProcessTextCancelAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await Task.Run(async () => {
                    await Task.Delay(5000, cancellationToken);
                    return "Processing Text";
                }, cancellationToken);
            }
            catch (TaskCanceledException) {
                return "Task cancelled";
            }
              

            return "Default Text";
        }


        private async Task<string> SomeTaskAsync()
        {
            return await Task.FromResult("SomeText");
        }
        private static void RunProcess()
        {
            for (int i = 0; i < 1000; i++)
            {

            }
        }
    }
}
