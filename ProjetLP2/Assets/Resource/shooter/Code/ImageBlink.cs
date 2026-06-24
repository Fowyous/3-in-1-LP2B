using UnityEngine;
using UnityEngine.UI;

///<summary>
///this class makes the image in the same game object as this script blink
///</summary>
public class ImageBlink : MonoBehaviour
{

  private Image image;
  [SerializeField] float blinkSpeed = 1f; // Duration of one complete blink cycle    
  private float blinkTimer = 0f;

  private void Start() { image = GetComponent<Image>(); }
  private void Update()
  {
    blinkTimer += Time.deltaTime;
    // Use sine wave for smooth fading 
    float alpha = Mathf.Abs(Mathf.Sin(blinkTimer * Mathf.PI / blinkSpeed));
    Color color = image.color;
    color.a = alpha;
    image.color = color;
  }
}

