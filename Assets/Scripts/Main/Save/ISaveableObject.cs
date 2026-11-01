public interface ISaveableObject
{
    public Vector3Serialize Position { get; }
    public QuaternionSerialize Rotation { get; }
    public string InstanceId { get; set; }
    public string TypeTag { get; }
    public string ExtraData { get; }

    public void Load(SaveObject data);
}
