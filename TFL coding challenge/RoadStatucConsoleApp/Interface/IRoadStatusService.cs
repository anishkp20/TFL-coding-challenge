using RoadStatucConsoleApp.Models;

namespace RoadStatucConsoleApp.Interface
{
    public interface IRoadStatusService
    {
        Task<RoadStatus> GetRoadStatusAsync(string roadId);
    }
}
