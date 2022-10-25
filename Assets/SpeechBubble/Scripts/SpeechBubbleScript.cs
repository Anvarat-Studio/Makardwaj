using UnityEngine;

public class SpeechBubbleScript : MonoBehaviour
{
    public GameObject source;
    // ^ Object which is "speaking" - Tail will alway point towards it.

    public GameObject bubbleBorder;
    // ^ Larger version which peaks to create border - acts as bounds for SpeechBubble.

    public GameObject tail;
    // Image which points to source.

    private float tailAngle;
    // ^ Current angle between bubble center and source.

    public Canvas canvas;
    // ^ Canvass which only contains our speechbubble components.

    // Update is called once per frame
    void Update()
    {
        GetTailAngle();
        RotateTail();
        MoveTail();
    }

    private void GetTailAngle()
    // ^ Some trigonometry.
    {
        if (!tail)
        {
            return;
        }
        Vector3 sourceP = source.transform.position;
        Vector3 canvasP = canvas.transform.position;
        float deltaX = sourceP.x - canvasP.x;
        float deltaY = sourceP.y - canvasP.y;
        tailAngle = Mathf.Atan2(deltaY, deltaX);
        tailAngle = Mathf.Rad2Deg * tailAngle;
    }

    private void RotateTail()
    // ^ What is says on the tin.
    {
        if (!tail)
        {
            return;
        }
        RectTransform tailRect = (RectTransform)tail.transform;
        tailRect.rotation = Quaternion.Euler(0, 0, tailAngle);
    }

    private void MoveTail()
    // ^ Sets tail position on the line between the cneter of the bubble and source.
    {
        if (!tail)
        {
            return;
        }
        RectTransform bubbleRect = bubbleBorder.GetComponent<RectTransform>();
        RectTransform tailRect = tail.GetComponent<RectTransform>();
        float bubbleHeight = bubbleRect.rect.height;
        float bubbleWidth = bubbleRect.rect.width;
        Vector2 intersection = PointOnEllipse(bubbleWidth, bubbleHeight, Mathf.Deg2Rad * tailAngle);
        float scaleFactor = 1.2f;
        // ^ Compensates for the fact that positioning an object changes the location of its *center* not an endpoint.
        float half = 0.5f;
        float xScale = scaleFactor * half * bubbleRect.transform.localScale.x;
        float yScale = scaleFactor * half * bubbleRect.transform.localScale.y;
        tail.GetComponent<RectTransform>().localPosition = new Vector3(xScale * intersection.x, yScale * intersection.y, 0f);
    }

    private Vector2 PointOnEllipse(float aHorzAxix, float aVertAxis, float aRadians)
    // ^ The speech bubble is not actually an ellipse, but close enough.
    {
        bool isCosTooSmall = Mathf.Abs(Mathf.Cos(aRadians)) < 0.00001f;
        // ^ We will be putting Cos(aRadians) in the denominator (by evaluating Tan(aRadians)) so it better not be zero.
        float bigNumber = 100000;
        // ^ Our standin for infinity.
        float m = isCosTooSmall ? bigNumber : Mathf.Tan(aRadians);
        // ^ m is slope as in y = mx + b.
        float numerator = aHorzAxix * aVertAxis;
        float denominator = Sqrt(Square(aVertAxis) + Square(aHorzAxix * m));
        float newX = numerator / denominator;
        newX = newX * Mathf.Sign(Mathf.Cos(aRadians));
        numerator = aVertAxis * Sqrt(Square(aHorzAxix) - Square(newX));
        denominator = aHorzAxix;
        float newY = numerator / denominator;
        newY = newY * Mathf.Sign(Mathf.Sin(aRadians));
        return new Vector2(newX, newY);
    }

    private float Sqrt(float aFloat)
    // ^ Shorthand.
    {
        return Mathf.Sqrt(aFloat);
    }

    private float Square(float aFloat)
    // ^ Shorthand.
    {
        return Mathf.Pow(aFloat, 2f);
    }

    public void SetBorderWidth(float aWidth)
    // ^ You can set the border width directly on the <ImageBorderScript> component
    // of the bubbleBorder GameObject - this feature is just for convenience. 
    {
        ImageBorderScript iScript = bubbleBorder.GetComponent<ImageBorderScript>();
        iScript.borderWidth = aWidth;
    }

}
