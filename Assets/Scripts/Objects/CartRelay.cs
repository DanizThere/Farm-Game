using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CartRelay : MonoBehaviour
{
    public CartObject Cart;

    private void OnTriggerEnter(Collider other)
    {
        Cart?.TryRegisterInZone(other);
    }

    private void OnTriggerExit(Collider other)
    {
        Cart?.UnregisterFromZone(other);
    }
}
