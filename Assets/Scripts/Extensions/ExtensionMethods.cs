using UnityEngine;

public static class ExtensionMethods
{
   public static bool TryGetComponentInChildren<T>(this GameObject gameObject, out T component, bool includeInactive = false) where T : Component
    {
        component = gameObject.GetComponentInChildren<T>(includeInactive);
        return component != null;
    }

    public static bool TryGetComponentInChildren<T>(this Component callingComponent, out T component, bool includeInactive = false) where T : Component
    {
        component = callingComponent.GetComponentInChildren<T>(includeInactive);
        return component != null;
    }
}
