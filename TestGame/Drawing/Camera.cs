using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.Drawing;

public class Camera : GameDrawer
{
    private const float MinZoom = 0.75f;
    private const float MaxZoom = 1f;

    private float _zoom;
    private Vector2 _position;
    private Point _resolution;

    public Camera()
    {
        _zoom = 1f;
        _resolution = SpriteBatch.GraphicsDevice.Viewport.Bounds.Size;
    }

    public Matrix GetTransformMatrix()
    {
        return Matrix.CreateTranslation(new Vector3(-_position.X, -_position.Y, 0)) *
               Matrix.CreateScale(new Vector3(_zoom, _zoom, 1)) *
               Matrix.CreateTranslation(new Vector3(_resolution.X * 0.5f, _resolution.Y * 0.5f, 0));
    }

    public Rectangle GetViewport()
    {
        var margin = _resolution.ToVector2() * (2 - _zoom);
        return new Rectangle
        (
            (int)(_position.X - margin.X / 2),
            (int)(_position.Y - margin.Y / 2),
            (int)(_resolution.X + margin.X),
            (int)(_resolution.Y + margin.Y)
        );
    }

    public Vector2 GetWorldPosition(Point screenPosition)
    {
        return new Vector2(
            _position.X - _resolution.X / 2 + screenPosition.X,
            _position.Y - _resolution.Y / 2 + screenPosition.Y);
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