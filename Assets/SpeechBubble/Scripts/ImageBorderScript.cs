using UnityEngine;

public class ImageBorderScript : MonoBehaviour
// ^ This script should be a component of the GameObject which has an Image component 
// which peeks from behind the bubble content image - creating a border.
{

    public GameObject bubble;
    // ^ Has Image component which is the bubble content area.

    private RectTransform bubbleRect;
    // ^ RectTransfrom of bubble.

    private RectTransform shadowRect;
    // RectTransform of this. 

    public float borderWidth;
    // ^ Season to taste.

    // Start is called before the first frame update
    void Start()
    {
        bubbleRect = bubble.GetComponent<RectTransform>();
        shadowRect = GetComponent<RectTransform>();
    }

    void Update()
    // Every frame, set the size of this equal to the size of the content bubble Image plus the borderWidth.
    {
        shadowRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, bubbleRect.rect.width + borderWidth);
        shadowRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, bubbleRect.rect.height + borderWidth);
    }
}
