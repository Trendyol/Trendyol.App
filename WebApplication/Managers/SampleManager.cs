using WebApplication.Models;

namespace WebApplication.Managers
{
    public class SampleManager : ISampleManager
    {
        public bool IsValid(Sample sample)
        {
            return true;
        }
    }
}