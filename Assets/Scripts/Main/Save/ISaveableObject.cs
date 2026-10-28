public interface ISaveableObject
{
    public Vector3Serialize Position { get; }
    public QuaternionSerialize Rotation { get; }
    public string InstanceId { get; }
    public string TypeTag { get; }
}
