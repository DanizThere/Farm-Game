using System;

[Serializable]
public class KeyItemObjectValue
{
    public Type Type => Handler.GetItemObject().GetType();
    public ItemObjectHandler Handler;
}