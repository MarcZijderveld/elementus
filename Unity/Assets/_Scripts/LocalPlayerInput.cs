using UnityEngine;
using System.Collections;

public class LocalPlayerInput : MonoBehaviour 
{
	public float movementSpeed = 0f,
				 jumpHeight	   = 0f;
	
	private float distanceToGround = 0f;
	public float groundedOffset = 1f;
	
	public bool inverseMovement = false;
	
	private PhotonView photonView;
	
	private void Start()
	{
		distanceToGround = this.GetComponentInChildren<BoxCollider>().bounds.extents.y;
		
		if(inverseMovement)
			movementSpeed *= -1;
		
		photonView = GetComponent<PhotonView>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += transform.forward * movementSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position -= transform.forward * movementSpeed * Time.deltaTime;
		}

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
			rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpHeight, rigidbody.velocity.z);
        }
	    Debug.Log(IsGrounded());
    }
	
	private bool IsGrounded()
    {
//		Debug.Log(distanceToGround);
        return Physics.Raycast(transform.position, -transform.up, distanceToGround + groundedOffset);
    }

    private void OnDrawGizmos()
    {
		Gizmos.color = Color.magenta;	
        Gizmos.DrawRay(transform.position, -transform.up + new Vector3(0, distanceToGround + groundedOffset, 0));
    }
}
