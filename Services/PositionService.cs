    public class PositionService : IPositionService {
        
        private readonly IPositionRepository _repository;
        private readonly ILogger<PositionService> _logger;
        
        public PositionService(){}
        public PositionService(IPositionRepository repository, ILogger<PositionService> logger) {
            _repository = repository;
            _logger = logger;
        }

        public IEnumerable<Position> GetAllPositions() {
            
            _logger.LogInformation("Fetching all positions from the repository.");
            var positions = _repository.GetAllPositions();
            _logger.LogInformation("Retrieved {Count} positions.", positions.Count()); 
            
            return positions;
        }

        public Position GetPositionByName(string name) {
            _logger.LogInformation("Fetching position by name: {PositionName}.", name);
            var position = _repository.GetPositionByName(name);
        
            if (position == null) {
                _logger.LogWarning("Position with name {PositionName} was not found.", name);
                throw new KeyNotFoundException("Position not found.");
            }

            _logger.LogInformation("Successfully retrieved position: {PositionName}.", name);
        
            return position;
        }

        public void AddPosition(Position position) {
            _logger.LogInformation("Attempting to add a new position: {PositionName}.", position.Name);

            if (string.IsNullOrWhiteSpace(position.Name)) {
                _logger.LogWarning("Failed to add position: Name is required.");
                throw new ArgumentException("Position name is required.");
            }

            if (position.Lat < -90 || position.Lat > 90) {
                _logger.LogWarning("Failed to add position: Invalid latitude {Latitude}.", position.Lat);
                throw new ArgumentException("Latitude must be between -90 and 90.");
            }

            if (position.Lon < -180 || position.Lon > 180) {
                _logger.LogWarning("Failed to add position: Invalid longitude {Longitude}.", position.Lon);
                throw new ArgumentException("Longitude must be between -180 and 180.");
            }

            _repository.AddPosition(position);
            _logger.LogInformation("Position {PositionName} added successfully.", position.Name);
        }

        public virtual IEnumerable<object> CalculateDistances(string name) {
            _logger.LogInformation("Calculating distances from position: {PositionName}.", name);

            var source = GetPositionByName(name);
            var allPositions = _repository.GetAllPositions(); 
            var distances = allPositions
                .Where(pos => pos.Name != name)
                .Select(target => new {
                    name = target.Name,
                    distance_km = DistanceCalculator.Calculate(source.Lat, source.Lon, target.Lat, target.Lon)
                    })
                .ToList();

            _logger.LogInformation("Calculated distances to {Count} positions from {SourcePosition}.", distances.Count, name);
            return distances;
        }
    }
