using FluentScheduler;

namespace NseOpenInterestWatch.Code
{
    public class NseScheduler : Registry
    {
        public NseScheduler()
        {
            // Schedule an IJob to run at an interval
//            Schedule<NseDailyJob>().ToRunNow().AndEvery(1).Days().At(18, 0).WeekdaysOnly();
            Schedule<NseDailyJob>().ToRunNow().AndEvery(30).Seconds().Between(9,10,15,45);
            //Schedule<NseDailyJob>().ToRunNow().AndEvery(1).Minutes();//.At(5, 0);

            // Schedule an IJob to run once, delayed by a specific time interval
            //Schedule<NseDailyJob>().ToRunOnceIn(5).Seconds();

            //// Schedule a simple job to run at a specific time
            //Schedule(() => Console.WriteLine("It's 9:15 PM now.")).ToRunEvery(1).Days().At(21, 15);

            //// Schedule a more complex action to run immediately and on an monthly interval
            //Schedule<MyComplexJob>().ToRunNow().AndEvery(1).Months().OnTheFirst(DayOfWeek.Monday).At(3, 0);

            //// Schedule a job using a factory method and pass parameters to the constructor.
            //Schedule(() => new MyComplexJob("Foo", DateTime.Now)).ToRunNow().AndEvery(2).Seconds();

            //// Schedule multiple jobs to be run in a single schedule
            //Schedule<NseDailyJob>().AndThen<MyOtherJob>().ToRunNow().AndEvery(5).Minutes();
        }
    }

    internal class NseDailyJob : IJob
    {
        public void Execute()
        {
            var nseDataScraper = new NseOiDataScraper();

            nseDataScraper.ScrapNseBankNiftyData();
        }
    }
}
