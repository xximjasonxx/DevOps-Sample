
namespace AuthApi.Services
{
    public interface ITelemetryService
    {
        void TrackEvent(string eventName);
    }
}