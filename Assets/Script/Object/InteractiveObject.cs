using UnityEngine;

public enum InteractiveObjectType
{
    None,
    ItemBox,
    
}
public class InteractiveObject : MonoBehaviour
{
    public InteractiveObjectType objectType;

    


    public void OnSelect()
    {
        print("상호작용 활성화");
    }

    public void OnDeselect()
    {
        print("상호작용 비활성화");
    }
}
