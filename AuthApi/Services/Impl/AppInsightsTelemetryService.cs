using Microsoft.ApplicationInsights;

namespace AuthApi.Services.Impl
{
    public class AppInsightsTelemetryService : ITelemetryService
    {
        private readonly TelemetryClient _telemetryClient;

        public AppInsightsTelemetryService(TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient;
        }

        public void TrackEvent(string eventName)
        {
            _telemetryClient.TrackEvent(eventName);
        }
    }
}