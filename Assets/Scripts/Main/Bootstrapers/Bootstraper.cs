using System.Collections;

[System.Serializable]
public abstract class Bootstraper
{
    public abstract void Inject(ServiceLocator serviceLocator);
}
