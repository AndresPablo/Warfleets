using UnityEngine;

[CreateAssetMenu(menuName = "Warfleets/Generic Module")]
public class ModuleData : BaseModuleData
{ 
    public override void Activate(){
        
    }

}

public abstract class BaseModuleData : ScriptableObject
{ 
    public bool raceMod;
    public ModuleType Type;
    public Sprite icon;
    public Color color = Color.white;
    [Range(1,20f)]public float range = 6f;

    public abstract void Activate();
}