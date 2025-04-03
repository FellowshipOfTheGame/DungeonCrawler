using UnityEngine;

public abstract class TileFeature
{
    public abstract void Activate(PlayerSave playerSave);
    public abstract void Deactivate(PlayerSave playerSave);

    public abstract void OnCreate(GameObject tileGameObject);
    public abstract void OnDestroy(GameObject tileGameObject);
}