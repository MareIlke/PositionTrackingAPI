public interface IPositionRepository {
    IEnumerable<Position> GetAllPositions();
    Position GetPositionByName(string name);
    void AddPosition(Position position);
}