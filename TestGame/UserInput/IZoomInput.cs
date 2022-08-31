namespace TestGame.UserInput;

public interface IZoomInput: IUserInput
{
    public bool IsZooming();
    public float GetZoomValue();
}