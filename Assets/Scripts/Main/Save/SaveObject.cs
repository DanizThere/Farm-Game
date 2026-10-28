using System;
using System.Collections.Generic;
using System.Text;

[Serializable]
public class SaveObject
{
    public Vector3Serialize Position;
    public QuaternionSerialize Rotation;
    public string InstanceId;
    public string TypeTag;
}
