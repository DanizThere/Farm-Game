using System;

[Serializable]
public class KeyItemObjectValue
{
    public object Type => Handler.GetItemObject();
    public ItemObjectHandler Handler;
}