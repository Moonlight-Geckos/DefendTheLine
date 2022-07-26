using UnityEngine;

[CreateAssetMenu(menuName ="Skin Item")]
public class SkinItem : ScriptableObject
{
    public int price;
    public int skinNumber;
    public Sprite skinSprite;
    public GameObject skinObject;
}
