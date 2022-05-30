using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScreenTopElement : MonoBehaviour
{
    [SerializeField]
    private float parallaxFactorX;

    private Transform cameraTransform;
    private Vector3 prevCameraPosition;

    private float textureWorldUnitSizeX;
    private float textureWorldUnitSizeY;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        prevCameraPosition = cameraTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureWorldUnitSizeX = (texture.width / sprite.pixelsPerUnit) * transform.localScale.x;
        textureWorldUnitSizeY = (texture.height / sprite.pixelsPerUnit) * transform.localScale.y;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 delta = cameraTransform.position - prevCameraPosition;
        delta.x *= parallaxFactorX;

        transform.position += delta;
        prevCameraPosition = cameraTransform.position;
        GetYPosition();

        if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureWorldUnitSizeX)
        {
            float offsetX = (cameraTransform.position.x - transform.position.x) % textureWorldUnitSizeX;
            transform.position = new Vector3(cameraTransform.position.x + offsetX, transform.position.y);
        }
    }

    void GetYPosition()
    {
        Vector3 cameraSpaceTopMiddleY = new Vector3(Camera.main.pixelWidth / 2.0f, Camera.main.pixelHeight, 0);
        Vector3 worldSpacePoint = Camera.main.ScreenToWorldPoint(cameraSpaceTopMiddleY);

        transform.position = new Vector3(transform.position.x, worldSpacePoint.y - (textureWorldUnitSizeY / 2.0f), transform.position.z);
    }
}
