using System;
using System.Collections.Generic;
using UnityEngine;

public class DataHolder : MonoBehaviour
{
    #region Serialized

    [SerializeField]
    private List<SkinItem> allSkins;

    [SerializeField]
    private Material[] blobsMaterials;

    #endregion Public

    private static DataHolder _instance;

    public static DataHolder Instance
    {
        get { return _instance; }
    }
    public List<SkinItem> AllSkins
    {
        get { return allSkins; }
    }
    public Material[] BlobsMaterials
    {
        get { return blobsMaterials; }
    }

    #region Methods
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
            allSkins?.Sort((x, y) => x.skinNumber.CompareTo(y.skinNumber));
        }

    }
    #endregion
}
