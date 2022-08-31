using System;
using System.Diagnostics.CodeAnalysis;

namespace TestGame.Drawing;

public class StringAnimation
{
    private float _elapsedSeconds;
    private float _interval;
    private Action<string> _onStringChanged;

    private int _index;
    private string[] _animationSequence;
    
    public StringAnimation(
        [NotNull] string[] animationSequence,
        [NotNull] Action<string> onStringChanged,
        float secondsInterval)
    {
        _animationSequence = animationSequence;
        _onStringChanged = onStringChanged;
        _interval = secondsInterval;
    }

    public void Update(float deltaTime)
    {
        _elapsedSeconds += deltaTime;
        if (_elapsedSeconds >= _interval)
        {
            _index++;
        }

        if (_index == _animationSequence.Length)
        {
            _index = 0;
        }

        _onStringChanged(_animationSequence[_index]);
    }
}