using Microsoft.Xna.Framework;

namespace TestGame.Extensions;

public static class PointExtensions
{
    public static Point Multiply(this Point point, int value) => new (point.X * value, point.Y * value);
    public static Point Divide(this Point point, int value) => new (point.X / value, point.Y / value);
    public static Point LeftNeighbour(this Point point) => new(point.X - 1, point.Y);
    public static Point RightNeighbour(this Point point) => new(point.X + 1, point.Y);
    public static Point TopNeighbour(this Point point) => new(point.X, point.Y - 1);
    public static Point BottomNeighbour(this Point point) => new(point.X, point.Y + 1);
    public static Point TopLeftNeighbour(this Point point) => new(point.X - 1, point.Y - 1);
    public static Point TopRightNeighbour(this Point point) => new(point.X + 1, point.Y - 1);
    public static Point BottomLeftNeighbour(this Point point) => new(point.X - 1, point.Y + 1);
    public static Point BottomRightNeighbour(this Point point) => new(point.X + 1, point.Y + 1);

}