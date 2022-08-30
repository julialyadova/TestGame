using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TestGame.Extensions;

namespace TestGame.Drawing;

public class Camera
{
    private const float MinZoom = 0.75f;
    private const float MaxZoom = 1f;

    private float _zoom;
    private Vector2 _position;

    public Camera()
    {
        _zoom = 1f;
    }

    public Matrix GetTransformMatrix(GraphicsDevice graphicsDevice)
    {
        return Matrix.CreateTranslation(new Vector3(-_position.X, -_position.Y, 0)) *
               Matrix.CreateScale(new Vector3(_zoom, _zoom, 1)) *
               Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width * 0.5f, graphicsDevice.Viewport.Height * 0.5f, 0));
    }

    public Rectangle GetViewport(GraphicsDevice graphicsDevice)
    {
        var screenSize = graphicsDevice.Viewport.Bounds.Size;
        var margin = screenSize.ToVector2() * (2 - _zoom);
        return new Rectangle
        (
            (int)(_position.X - margin.X / 2),
            (int)(_position.Y - margin.Y / 2),
            (int)(screenSize.X + margin.X),
            (int)(screenSize.Y + margin.Y)
        );
        return new Rectangle((_position.ToPoint() - graphicsDevice.Viewport.Bounds.Size.Divide(2)), graphicsDevice.Viewport.Bounds.Size);
    }

    public void Move(Vector2 direction)
    {
        _position += direction;
    }

    public void LookAt(Vector2 position)
    {
        _position = position;
    }

    public void Zoom(float zoomValue)
    {
        _zoom = Math.Clamp(_zoom + zoomValue, MinZoom, MaxZoom);
    }
}