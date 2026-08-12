public interface IView
{
    public int Order { get; set; }
    public bool IsActive { get; set; }

    public void Show();
    public void Hide();
}