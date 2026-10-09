public class DefaultSelectable : SLMonoBehaviour, ISelectable
{
    protected bool _canSelect = true;

    public virtual void Show()
    {   
    }

    public virtual void Hide()
    {
    }

    public bool CanSelect()
    {
        return _canSelect;
    }
}
