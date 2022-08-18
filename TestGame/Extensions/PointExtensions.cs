using Microsoft.Xna.Framework;

namespace TestGame.Extensions;

public static class PointExtensions
{
    public static Point Multiply(this Point point, int value) => new (point.X * value, point.Y * value);
    public static Point Divide(this Point point, int value) => new (point.X / value, point.Y / value);

}