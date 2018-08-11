using UnityEngine;

/// <summary>
/// CellMetrics stores all values needed to render cells
/// </summary>
public static class CellMetrics
{

    //outerSide and innerSide provide dimensions for Cells
    //outerSide is the length of the Cell's outer sides
    //innerSide is the length of the Cell's inner sides
    //These will potentially be used together to create cells with a center and border
    public const float outerSide = 10f;
    public const float innerSide = outerSide * 0.8f; //Currently unused

    //Quad corners starting at the top right and moving clockwise
    public static Vector3[] outerCorners =
    {
    new Vector3(outerSide, 0f, outerSide),   // Top Right
    new Vector3(outerSide, 0f, -outerSide),  // Bottom Right
    new Vector3(-outerSide, 0f, -outerSide),  // Bottom Left
    new Vector3(-outerSide, 0f, outerSide)  // Top Left
}; //End corners array

    public static Vector3[] innerCorners =
    {
    new Vector3(innerSide, 0.1f, innerSide),   // Top Right
    new Vector3(innerSide, 0.1f, -innerSide),  // Bottom Right
    new Vector3(-innerSide, 0.1f, -innerSide),  // Bottom Left
    new Vector3(-innerSide, 0.1f, innerSide)  // Top Left
}; //End corners array

    //Cell color options
    public static Color[] colors;
}