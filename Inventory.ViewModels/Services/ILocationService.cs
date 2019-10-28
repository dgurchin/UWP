using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace Inventory.Services
{
    public interface ILocationService
    {
        event EventHandler<Geoposition> PositionChanged;
        Geoposition CurrentPosition { get; }
        Task<bool> InitializeAsync();
        Task<bool> InitializeAsync(uint desiredAccuracyInMeters);
        Task<bool> InitializeAsync(uint desiredAccuracyInMeters, double movementThreshold);
        Task StartListeningAsync();
        void StopListening();
    }
}
