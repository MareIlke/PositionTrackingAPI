    
    using Microsoft.Data.SqlClient;


    public class PositionRepository : IPositionRepository {
        private readonly string _connectionString;
        private readonly ILogger<PositionRepository> _logger;

        public PositionRepository(string connectionString, ILogger<PositionRepository> logger) {
        _connectionString = connectionString;
        _logger = logger;
        }

        public IEnumerable<Position> GetAllPositions(){
            _logger.LogInformation("Fetching all positions from the database.");
            var positions = new List<Position>();
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand("SELECT pos_name, pos_lat, pos_lon FROM positions ORDER BY pos_name", connection);
            using var reader = command.ExecuteReader();
            while (reader.Read()) {
                positions.Add(new Position{
                    Name = reader["pos_name"].ToString(),
                    Lat = Convert.ToDouble(reader["pos_lat"]),
                    Lon = Convert.ToDouble(reader["pos_lon"])
                });
            }

            _logger.LogInformation("Fetched {Count} positions from the database.", positions.Count);
            return positions;
        }

        public Position GetPositionByName(string name){
            _logger.LogInformation("Fetching position with name: {PositionName}", name);
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand("SELECT pos_name, pos_lat, pos_lon FROM positions WHERE pos_name = @name", connection);
            command.Parameters.AddWithValue("@name", name);

            using var reader = command.ExecuteReader();
            if (reader.Read()){
                _logger.LogInformation("Position found: {PositionName}", name);
                return new Position{
                    Name = reader["pos_name"].ToString(),
                    Lat = Convert.ToDouble(reader["pos_lat"]),
                    Lon = Convert.ToDouble(reader["pos_lon"])
                };
            }

            _logger.LogWarning("Position not found: {PositionName}", name);
            return null;
        }

        public void AddPosition(Position position) {
            _logger.LogInformation("Adding new position: {PositionName}", position.Name);

            try {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();

            
                if (PositionExists(position.Name)) {
                    _logger.LogWarning("Position with name '{PositionName}' already exists. Skipping insert.", position.Name);
                    throw new ArgumentException($"A position with the name '{position.Name}' already exists.");
                }
                
                using var command = new SqlCommand("INSERT INTO positions (pos_name, pos_lat, pos_lon) VALUES (@name, @lat, @lon)", connection);
                command.Parameters.AddWithValue("@name", position.Name);
                command.Parameters.AddWithValue("@lat", position.Lat);
                command.Parameters.AddWithValue("@lon", position.Lon);
                command.ExecuteNonQuery();

                _logger.LogInformation("Position added: {PositionName}", position.Name);
            }
            
            catch (SqlException ex){
                if (ex.Number == 2627){
                    _logger.LogError(ex, "Unique constraint violation: Position with name '{PositionName}' already exists.", position.Name);
                    throw new ArgumentException($"A position with the name '{position.Name}' already exists.");
                }
                else {
                    _logger.LogError(ex, "An error occurred while adding the position: {PositionName}", position.Name);
                    throw;
                }
            }
        }

    private bool PositionExists(string name)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = new SqlCommand("SELECT COUNT(1) FROM positions WHERE pos_name = @name", connection);
        command.Parameters.AddWithValue("@name", name);
        var result = (int)command.ExecuteScalar();
        return result > 0;
    }
}
