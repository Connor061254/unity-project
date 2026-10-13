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

    public static bool TryGetComponentsInChildren<T>(this GameObject gameObject, out T[] components, bool includeInactive = false) where T : Component
    {
        components = gameObject.GetComponentsInChildren<T>(includeInactive);
        return components != null && components.Length > 0;
    }

    public static bool TryGetComponentsInChildren<T>(this Component callingComponent, out T[] components, bool includeInactive = false) where T : Component
    {
        components = callingComponent.GetComponentsInChildren<T>(includeInactive);
        return components != null && components.Length > 0;
    }
}
