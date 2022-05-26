using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackgroundElement : MonoBehaviour
{
    [SerializeField]
    private bool infiniteVertical;

    [SerializeField]
    private bool infiniteHorizontal;

    [SerializeField]
    private float parallaxFactorX;

    [SerializeField]
    private float parallaxFactorY;

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
        delta.y *= parallaxFactorY;

        transform.position += delta;
        prevCameraPosition = cameraTransform.position;


        if(infiniteHorizontal && Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureWorldUnitSizeX)
        {
            float offsetX = (cameraTransform.position.x - transform.position.x) % textureWorldUnitSizeX;
            transform.position = new Vector3(cameraTransform.position.x + offsetX, transform.position.y);
        }

        if (infiniteVertical && Mathf.Abs(cameraTransform.position.y - transform.position.y) >= textureWorldUnitSizeY)
        {
            float offsetY = (cameraTransform.position.y - transform.position.y) % textureWorldUnitSizeY;
            transform.position = new Vector3(transform.position.x, cameraTransform.position.y + offsetY);
        }
    }
}
