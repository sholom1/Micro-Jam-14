using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    [SerializeField]
    private Texture2D cursor;

    public void setCursor()
    {
        Cursor.SetCursor(cursor, new Vector2(0.5f, 0.5f), CursorMode.ForceSoftware);
    }
}
