using System.Collections.Generic;
using UnityEngine;

public struct GradingData
{
    public Vector2 start;
    public List<Vector2> positions;
    public Vector2 end;

    public GradingData(Vector2 start, List<Vector2> positions, Vector2 end)
    {
        this.start = start;
        this.positions = new List<Vector2>(positions);
        this.end = end;
    }
}
