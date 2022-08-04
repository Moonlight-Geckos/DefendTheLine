using UnityEngine;

public class GrassColorAssigner : ColorAssigner
{
    protected override void AssignColor()
    {
        if (!CheckValidity())
        {
            Debug.LogWarning("Color Assigner is used incorrectly!!");
            return;
        }
        else
        {
            for (int i = 0; i < assignments.Length; i++)
            {
                var color = _palette.colors[assignments[i].indexInPalette];
                color *= Random.Range(0.5f, 1f);
                _renderer.materials[i].SetColor(assignments[i].name, color);
            }
        }
    }
}
