using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyColorAssigner : ColorAssigner
{
    [SerializeField]
    private Assignment[] bossAssignments;

    public bool BossMode
    {
        get;
        set;
    }

    protected override void AssignColor()
    {
        base.AssignColor();
        if (!CheckValidity())
        {
            Debug.LogWarning("Color Assigner is used incorrectly!!");
            return;
        }
        else
        {
            if (!BossMode)
            {
                for (int i = 0; i < assignments.Length; i++)
                {
                    _renderer.materials[i].SetColor(assignments[i].name, _palette.colors[assignments[i].indexInPalette]);
                }
            }
            else
            {
                for (int i = 0; i < assignments.Length; i++)
                {
                    _renderer.materials[i].SetColor(assignments[i].name, _palette.colors[bossAssignments[i].indexInPalette]);
                }
            }
        }
    }
}
