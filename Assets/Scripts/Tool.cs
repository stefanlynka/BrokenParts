using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tool : object {
    
    public static Vector2 DoubleFromOrigin(Vector2 vector, Vector2 origin) {
        float new_x = (vector.x - origin.x) * 2 + origin.x;
        float new_y = (vector.y - origin.y) * 2 + origin.y;
        return new Vector2(new_x, new_y);
    }

    public static Vector2 HalveFromOrigin(Vector2 vector, Vector2 origin) {
        float new_x = (vector.x - origin.x) / 2 + origin.x;
        float new_y = (vector.y - origin.y) / 2 + origin.y;
        return new Vector2(new_x, new_y);
    }
    
    public static Vector2 FlipXFromOrigin(Vector2 vector, Vector2 origin) {
        float new_x = -(vector.x - origin.x) + origin.x;
        float new_y = (vector.y - origin.y) + origin.y;
        return new Vector2(new_x, new_y);
    }
    public static Vector2 FlipYFromOrigin(Vector2 vector, Vector2 origin) {
        float new_x = (vector.x - origin.x) + origin.x;
        float new_y = -(vector.y - origin.y) + origin.y;
        return new Vector2(new_x, new_y);
    }
}
