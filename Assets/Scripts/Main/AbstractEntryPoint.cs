using System.Collections;
using UnityEngine;

public abstract class AbstractEntryPoint : MonoBehaviour
{
    public abstract IEnumerator Initialize();
}