using UnityEngine;

public abstract class CollectableSOBase : ScriptableObject
{
    public abstract void Collect(GameObject objectThatCollected);

}
