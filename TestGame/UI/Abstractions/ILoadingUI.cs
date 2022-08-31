namespace TestGame.UI.Abstractions;

public interface ILoadingUI
{
    public void ShowMessage(string text);
    public void Update(float deltaTime);
    public void Draw();
}