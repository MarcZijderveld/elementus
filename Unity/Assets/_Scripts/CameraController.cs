using UnityEngine;
using System.Collections;
 
public class CameraController : MonoBehaviour 
{   
	private	Grid 	_grid	= null; 
	public	Grid 	grid
	{
		get
		{
			if (_grid == null)
			{
				_grid = Hierarchy.GetComponentWithTag<Grid>("Grid");
			}
			return _grid;
		}		
	} 

	public float zoomSpeed,
				 panSpeed,
				 scrollEdge,
				 rotationSensetivityX,
				 rotationSensetivityY,
				 minimumYRotation,
				 maximumYRotation,
				 minFov,
				 maxFov,
				 sensitivity;
	
	private Transform camTarget;
    bool animating;
	private float rotationX,
				  rotationY,
				  fov;  

	private Transform sphere,
					  mover;

    private Vector3 startPosCamara;
	private Vector3 startPosSphere;
    private float xDist, yDist;
    private float widht, height;
	private bool edgeScrolling,
				 movement = true;

	private PlayerDataHandler _playerDataHandler = null;
	public PlayerDataHandler playerDataHandler
	{
		get
		{
			if (_playerDataHandler == null)
			{
				_playerDataHandler = Hierarchy.GetComponentWithTag<PlayerDataHandler>("PlayerDataHandler");
			}
			return _playerDataHandler;
		}
	}

	private void Start()
	{
		PlayerPrefs.SetInt("camera_EdgeScrolling", 1);
        
        Vector2 distances = grid.getDistanceBetweenTiles();
        widht = grid.gridWidth;
        height = grid.gridHeight;
        xDist = distances.x * widht;
        yDist = distances.y * height;

        animating = false;
		sphere = transform.parent;
		mover = sphere.parent;
//		sphere.position = grid.GetMiddle();
		startPosSphere = sphere.position;

		if(playerDataHandler.GetID() == 1)
		{
			Debug.Log("rotated");
			mover.transform.localEulerAngles = new Vector3(0, 180, 0f);	
			rotationX = 180;

		}
	}
	
    private void Update () 
	{		
		if(movement)
		{
			edgeScrolling = (PlayerPrefs.GetInt("camera_EdgeScrolling") == 0) ? false : true;

			if(Input.GetButton("Rotate Camera"))
			{
				rotationX += Input.GetAxis("Mouse X") * rotationSensetivityX;
				rotationY += Input.GetAxis("Mouse Y") * rotationSensetivityY;
				rotationY = Mathf.Clamp(rotationY, minimumYRotation, maximumYRotation);
			}
			else
			{
				if(edgeScrolling)
				{

					if ( Input.GetKey("d") /*|| Input.mousePosition.x >= Screen.width * (1 - scrollEdge)*/)
					{
						mover.transform.Translate(mover.right * Time.deltaTime * panSpeed, Space.World);

					}
					else if (Input.GetKey("a")/* || Input.mousePosition.x <= Screen.width * scrollEdge*/)
					{
						mover.transform.Translate(mover.right * Time.deltaTime * -panSpeed, Space.World);
                       
					}
					if ( Input.GetKey("w") /*|| Input.mousePosition.y >= Screen.height * (1 - scrollEdge)*/)
					{
						mover.transform.Translate(mover.forward * Time.deltaTime * panSpeed, Space.World);
                      
					}
					else if ( Input.GetKey("s") /*|| Input.mousePosition.y <= Screen.height * scrollEdge*/)
					{
						mover.transform.Translate(mover.forward * Time.deltaTime * -panSpeed, Space.World);
					}

                    if (mover.transform.position.x > widht)
                    {
                        mover.transform.position = new Vector3(widht, mover.position.y, mover.position.z);
                    }
                    if (mover.transform.position.x < 0)
                    {
                        mover.transform.position = new Vector3(0, mover.position.y, mover.position.z);
                    }
                    if (mover.transform.position.z > height)
                    {
                        mover.transform.position = new Vector3(mover.position.x, mover.position.y, height);
                    }
                    if (mover.transform.position.z < 0)
                    {
                        mover.transform.position = new Vector3(mover.position.x, mover.position.y, 0);
                    }
				}
			}
            if (!animating)
            {
                fov += Input.GetAxis("Mouse ScrollWheel") * sensitivity - (fov / 5);
                Mathf.Clamp(fov, -4, 4);
				//transform.position = new Vector3(transform.position.x, transform.position.y, fov);
                if (Vector3.Distance(transform.position +transform.forward * Time.deltaTime * (fov * 3) , sphere.position) < 5)
                    transform.position = sphere.position + (Vector3.Normalize(transform.position - sphere.position) * 5);
                else if(  Vector3.Distance(transform.position +transform.forward * Time.deltaTime * (fov * 3) , sphere.position) > 20)
                    transform.position = sphere.position + (Vector3.Normalize(transform.position - sphere.position) * 20);
                else
                    transform.position += transform.forward * Time.deltaTime * (fov * 3);				                 
                   
			}

			sphere.transform.localEulerAngles = new Vector3(-rotationY, 0, 0f);	
			mover.transform.localEulerAngles = new Vector3(0, rotationX, 0f);	
		}
    }

	public IEnumerator FocusOn(Vector3 position)
	{
        animating = true;
        startPosSphere = sphere.position;
        startPosCamara = transform.localPosition;
		Timer t = new Timer(4f);

		while(t)
		{
            sphere.position = Vector3.Lerp(startPosSphere, position, t.progress);
            transform.localPosition = Vector3.Lerp(startPosCamara, Vector3.Normalize( startPosCamara) * 8 ,t.progress);
			yield return null;
		}
	}

	public IEnumerator Reset()
	{
        Vector3 resetSpherePos = sphere.position;
        Vector3 resetCameraPos = transform.localPosition;
	
        float startFOV = camera.fieldOfView;
        Vector3 startPostion = transform.localPosition;

        Timer t = new Timer(4f);

        while (t)
        {
            sphere.position = Vector3.Lerp(resetSpherePos, startPosSphere, t.progress);
            transform.localPosition = Vector3.Lerp(resetCameraPos, startPosCamara, t.progress);
            yield return null;
        }
        animating = false;
	}
}