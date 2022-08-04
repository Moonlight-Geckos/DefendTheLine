using System;
using System.Collections;
using UnityEngine;

public class ColorAssigner : MonoBehaviour
{
    [Serializable]
    protected class Assignment
    {
        public string name = "_Color";
        [Range(0, 5)]
        public int indexInPalette;
    }
    [SerializeField]
    protected Assignment[] assignments;

    protected ColorGenerator.ColorPalette _palette;
    protected Renderer _renderer;

    IEnumerator OnBecameVisible()
    {
        yield return null;
        _palette = ColorGenerator.Instance.ActivePalette;
        _renderer = GetComponent<Renderer>();
        AssignColor();
    }
    protected virtual void AssignColor()
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
                _renderer.materials[i].SetColor(assignments[i].name, _palette.colors[assignments[i].indexInPalette]);
            }
        }
    }
    protected virtual bool CheckValidity()
    {
        return _renderer != null && _renderer.materials.Length == assignments.Length;
    }
}