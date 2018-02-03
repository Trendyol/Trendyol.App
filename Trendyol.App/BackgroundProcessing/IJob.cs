using System.Threading.Tasks;

namespace Trendyol.App.BackgroundProcessing
{
    public interface IJob
    {
        Task Run();
    }
}