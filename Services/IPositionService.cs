public interface IPositionService {
    IEnumerable<Position> GetAllPositions();
    Position GetPositionByName(string name);
    void AddPosition(Position position);
    IEnumerable<object> CalculateDistances(string name);
}