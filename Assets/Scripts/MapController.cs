using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour
{
    private float width, height;
    private Vector3 startPos;
    private Transform cam;
    Transform mapTransform;

    void Start()
    {
        startPos = transform.position;
        width = GetComponent<Tilemap>().localBounds.size.x;
        height = GetComponent<Tilemap>().localBounds.size.y;

        mapTransform = transform;

        cam = Camera.main.gameObject.transform;
    }

    void Update()
    {
        if (cam.position.x > startPos.x + width)
            startPos.x += width * 2;
        else if (cam.position.x < startPos.x - width)
            startPos.x -= width * 2;
        if (cam.position.y > startPos.y + height)
            startPos.y += height * 2;
        else if (cam.position.y < startPos.y - height)
            startPos.y -= height * 2;
        
        mapTransform.position = startPos;
    }
}
