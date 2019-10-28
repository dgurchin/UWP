using System.Threading.Tasks;

using Inventory.Common;
using Inventory.Services;

using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;

namespace Inventory.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        private readonly ILocationService _locationService;
        private MapControl _map;

        // TODO WTS: Set your preferred default zoom level
        private const double DefaultZoomLevel = 16;

        // TODO WTS: Set your preferred default location if a geolock can't be found.
        private readonly BasicGeoposition _defaultPosition = new BasicGeoposition()
        {
            Latitude = 50.493286,
            Longitude = 30.488978
        };

        private double _zoomLevel;
        private Geopoint _center;

        public double ZoomLevel
        {
            get { return _zoomLevel; }
            set { Set(ref _zoomLevel, value); }
        }

        public Geopoint Center
        {
            get { return _center; }
            set { Set(ref _center, value); }
        }

        public RelayCommand<MapIcon> AddMapIconCommand { get; }

        public MapViewModel(ILocationService locationService, ICommonServices commonServices) : base(commonServices)
        {
            _locationService = locationService;

            Center = new Geopoint(_defaultPosition);
            ZoomLevel = DefaultZoomLevel;

            AddMapIconCommand = new RelayCommand<MapIcon>(AddMapIcon, CanAddMapIcon);
        }

        public async Task InitializeAsync(MapControl map)
        {
            _map = map;
            _locationService.PositionChanged += LocationService_PositionChanged;

            var initializationSuccessful = await _locationService.InitializeAsync();
            if (initializationSuccessful)
            {
                await _locationService.StartListeningAsync();
            }

            if (initializationSuccessful && _locationService.CurrentPosition != null)
            {
                Center = _locationService.CurrentPosition.Coordinate.Point;
            }
            else
            {
                Center = new Geopoint(_defaultPosition);
            }
        }

        public void Cleanup()
        {
            if (_locationService != null)
            {
                _locationService.PositionChanged -= LocationService_PositionChanged;
                _locationService.StopListening();
            }
        }

        private void LocationService_PositionChanged(object sender, Geoposition geoposition)
        {
            if (geoposition != null)
            {
                Center = geoposition.Coordinate.Point;
            }
        }

        private void AddMapIcon(MapIcon mapIcon)
        {
            _map.MapElements.Add(mapIcon);
        }

        private bool CanAddMapIcon(MapIcon mapIcon)
        {
            if (_map == null)
            {
                return false;
            }

            return true;
        }
    }
}
