using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    bool pres;
    int w;
    int h;
    Vector3 limxy, limXY;
    public float divPix;
    private BoxCollider2D box;
    private float bleft, bright, bup, bdown;
    private bool grabbed;
    // Use this for initialization
    void Start() {
        box = GetComponent<BoxCollider2D>();
        bleft = transform.position.x + box.offset.x - (box.size.x / 2);
        bright = transform.position.x + box.offset.x + (box.size.x / 2);
        bup = transform.position.y + box.offset.y + (box.size.y / 2);
        bdown = transform.position.y + box.offset.y - (box.size.y / 2);
    }
	
	// Update is called once per frame
	void Update () {
        mover();
	}

    void mover()
    {
        w = Screen.width;
        h = Screen.height;
        limxy = Camera.main.ScreenToWorldPoint(new Vector3(0, 0));
        limXY = Camera.main.ScreenToWorldPoint(new Vector3(w, h));

        /*if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            transform.position = t.position;
        }*/
        if (Input.GetMouseButtonDown(0))
        {
            pres = true;
            if (box.bounds.Contains(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                                 Input.mousePosition.y, 10))))
            {
                grabbed = true;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            pres = false;
            grabbed = false;
        }
        if (pres&&grabbed)
        {
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                                                Input.mousePosition.y, 10));
        }
        if (transform.position.x < limxy.x) transform.position = new Vector3(limxy.x, transform.position.y);
        else if (transform.position.x > limXY.x) transform.position = new Vector3(limXY.x, transform.position.y);
        if (transform.position.y < limxy.y) transform.position = new Vector3(transform.position.x, limxy.y);
        else if (transform.position.y > limXY.y) transform.position = new Vector3(transform.position.x, limXY.y);
    }
}
