using UnityEngine;

public class GridColorAssigner : ColorAssigner
{
    protected override void AssignColor()
    {
        for (int i = 0; i < assignments.Length; i++)
        {
            _renderer.material.SetColor(assignments[i].name, _palette.colors[assignments[i].indexInPalette]);
        }
    }
}