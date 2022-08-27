namespace TestGame.UserInput;

public class Control
{
    private ControlState _state;
    
    public void UpdateState(bool isDown)
    {
        if (isDown)
        {
            if (_state == ControlState.None)
                _state = ControlState.PressedFirst;
            else if (_state == ControlState.PressedFirst)
                _state = ControlState.Pressed;
        }
        else if (_state is ControlState.Pressed or ControlState.PressedFirst)
             _state = ControlState.Released;

    }
}

public enum ControlState
{
    None,
    PressedFirst,
    Pressed,
    Released
}