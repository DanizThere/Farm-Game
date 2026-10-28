using UnityEngine;

public interface ICartLoadable
{
    public Rigidbody Body { get; }

    public Transform Root { get; }
    public void OnLoaded(CartObject cart);
    public void OnUnloaded(CartObject cart);
}
