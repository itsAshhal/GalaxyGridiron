using UnityEngine;

namespace GalaxyGridiron
{
    public static class Accessories
    {
        public static Vector2 GetWorldPosition(Vector2 screenPos) => Camera.main.ScreenToWorldPoint(screenPos);
    }
}