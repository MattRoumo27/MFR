using System;

[Serializable]
public class CheckpointFlagData
{
    public float[] position;

    public CheckpointFlagData(CheckpointFlag flag)
    {
        position = new float[2];
        position[0] = flag.transform.position.x;
        position[1] = flag.transform.position.y;
    }
}
