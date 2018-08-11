
/// <summary>
/// The Direction enum is used to identify valid orientations
/// Key User: Robots - Uses Direction to determine turns, rotation, moves, etc
/// </summary>
public enum Direction
{
    NORTH, EAST, SOUTH, WEST
} //End Direction enum

/// <summary>
/// Extension methods for the Direction enum
/// These are used to get one Direction relative to another
/// </summary>
public static class DirectionExtensions
{
    //Get the Direction opposite of the parameter on a compass
    // North <<>> South
    // East <<>> West
    public static Direction Opposite(this Direction direction)
    {
        return (int)direction < 2 ? (direction + 2) : (direction - 2);
    } //End Direction.Opposite()

    //Get the Direction one step counter clockwise to the parameter on a compass
    // North >> West
    // West >> South
    // South >> East
    // East >> North
    public static Direction Previous(this Direction direction)
    {
        return direction == Direction.NORTH ? Direction.WEST : (direction - 1);
    } //End Direction.Previous

    //Get the Direction one step clockwise to the parameter on a compass
    // North >> East
    // East >> South
    // South >> West
    // West >> North
    public static Direction Next(this Direction direction)
    {
        return direction == Direction.WEST ? Direction.NORTH : (direction + 1);
    } //End Direction.Next
} //End DirectionExtensions class
