namespace Paralel_4
{
    class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            Random rand = new Random();
            int forks = 5;
            int steps = 5;
            int philosophers = 5;
            Console.WriteLine("Forks: {0}\nSteps: {1}\nPhilosophers: {2}", forks, steps, philosophers);
            program.Starter(forks, steps, philosophers);
        }

        private void Starter(int forks, int steps, int philosophers)
        {
            for (int i = 0; i < forks; i++)
            {
                storage.Add("fork ");
                Console.WriteLine(" + Added fork to storage");
            }

            Access = new Semaphore(2, 2);
            Forks = new Semaphore(forks, forks);//3, 3
            Empty = new Semaphore(1, 1);//0, 3

            for (int i = 0; i < philosophers; i++)
            {
                Thread threadPhilosopher = new Thread(Philosopher);
                threadPhilosopher.Name = "Philosopher " + (i + 1);
                Args args = new Args(steps, threadPhilosopher);
                threadPhilosopher.Start(args);
            }
        }
        class Args
        {
            public int Steps { get; set; }
            public Thread Thread { get; set; }

            public Args(int steps, Thread thread)
            {
                Steps = steps;
                Thread = thread;
            }
        }
        private Semaphore Access;
        private Semaphore Forks;
        private Semaphore Empty;

        private readonly List<string> storage = new List<string>();

        private void Philosopher(Object input)
        {
            Args args = (Args)input;

            for (int i = 0; i < args.Steps; i++)
            {
                Empty.WaitOne();
                Forks.WaitOne();
                storage.RemoveAt(0);
                Console.WriteLine(" - " + args.Thread.Name + " took left fork.   Forks left: " + storage.Count);
                Access.WaitOne();
                Forks.WaitOne();
                storage.RemoveAt(0);
                Console.WriteLine(" - " + args.Thread.Name + " took right fork.  Forks left: " + storage.Count);
                Console.WriteLine("   " + args.Thread.Name + " is eating...      Forks left: " + storage.Count);
                Access.Release();
                Empty.Release();
                Thread.Sleep(1000);

                Access.WaitOne();
                storage.Add("fork");
                storage.Add("fork");
                Forks.Release(2);
                Console.WriteLine(" + " + args.Thread.Name + " returned forks.   Forks left: " + storage.Count);
                Console.WriteLine("   " + args.Thread.Name + " is thinking...    Forks left: " + storage.Count);
                Access.Release();
                Thread.Sleep(1000);
            }
        }
    }
}