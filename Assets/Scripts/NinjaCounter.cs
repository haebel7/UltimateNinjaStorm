using UnityEngine;

[CreateAssetMenu(fileName = "NinjaCountData", menuName = "ScriptableObjects/NinjaCountData", order = 1)]
public class NinjaCounter : ScriptableObject
{
    public int ninjaCount;
    public int maxNinjasOnScreen;

    public void increaseNinjaAmount(int amount)
    {
        ninjaCount += amount;
    }

}
