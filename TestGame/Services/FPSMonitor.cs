using System.Collections.Generic;

namespace TestGame.Services;

public class FPSMonitor
{
    private const float UpdateSeconds = 0.8f;
    public float FramesPerSecond { get; private set; }
    private float _timeSkipped;

    public void CountFrame(float deltaTime)
    {
        if (_timeSkipped > UpdateSeconds)
        {
            FramesPerSecond = 1f / deltaTime;
            _timeSkipped = 0;
        }

        _timeSkipped += deltaTime;
    }
}