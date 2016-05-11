using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class WebcamTexture : MonoBehaviour 
{
	private DataSync _data = null;
	public DataSync data
	{
		get
		{
			if (_data == null)
			{
				_data = Hierarchy.GetComponentWithTag<DataSync>("DataSync");
			}
			return _data;
		}
	} 

	public bool send, receive;

	private byte[] lastImage;
	private List<byte> buffer;

	WebCamTexture webcamTexture;

	public GameObject target;

	public Vector2 resolution = new Vector2(640,480);

	// Use this for initialization
	void Start () 
	{
		if(send)
		{
	        webcamTexture = new WebCamTexture((int)resolution.x, (int)resolution.y);
	        renderer.material.mainTexture = webcamTexture;
	        webcamTexture.Play();
			view = target.GetComponent<PhotonView>();

		}
		lastImage = new byte[1];
		imageTex = new Texture2D((int)resolution.x, (int)resolution.y, TextureFormat.RGB24, false);
	}

	private Texture2D imageTex;

	private PhotonView view;
	
	public bool test;

	// Update is called once per frame
	void FixedUpdate () 
	{
		if(data.initConnected || test)
		{
			if(send)
			{
				Color32[] pixel = webcamTexture.GetPixels32();
				byte[] b = new byte[pixel.Length * 3];
				int j = 0;
				for(int i = 0; i < b.Length; i += 3)
				{
					b[i] = (pixel[j].r);
					b[i + 1] = (pixel[j].g);
					b[i + 2] = (pixel[j].b);
					j++;
				}

				if(webcamTexture != null)
				{
					if(!ByteArraysEqual(b, lastImage))
					{
						byte[] compressed = CLZF2.Compress(b);
						byte[] package1 = new byte[compressed.Length / 2];
						byte[] package2 = new byte[compressed.Length / 2];

						for(int i = 0; i < compressed.Length / 2; i++)
						{
							package1[i] = compressed[i];
						}

						for(int i = 0; i < compressed.Length / 2; i++)
						{
							package2[i] = compressed[(compressed.Length / 2) + i];
						}

						view.RPC("SendWebCamData", PhotonTargets.Others, package1, false);
						view.RPC("SendWebCamData", PhotonTargets.Others, package2, true);
					}
					if(test && !ByteArraysEqual(b, lastImage))
					{
						byte[] compressed = CLZF2.Compress(b);

						target.GetComponent<WebcamTexture>().SendWebCamData(compressed, true);
					}
					lastImage = b;
				}
			}
		}
	}

	[RPC]
	public void SendWebCamData(byte[] image, bool last)
	{
		List<byte> temp = new List<byte>(image);
		buffer.Concat(temp);

		if(last)
		{
			lastImage = buffer.ToArray();
			lastImage = CLZF2.Decompress(lastImage);
			Color32[] colorArray = new Color32[lastImage.Length/3];
			int j = 0;

			for(int i = 0; i < lastImage.Length; i+=3)
			{
				Color32 color = new Color32(lastImage[i + 0], lastImage[i + 1], lastImage[i + 2], 255);
				colorArray[j] = color;
				j++;
			}
			imageTex.SetPixels32(colorArray);
			imageTex.Apply();
			renderer.material.mainTexture = imageTex;
			buffer.Clear();
		}
	}

	public bool ByteArraysEqual(byte[] b1, byte[] b2)
	{
		if (b1 == b2) return true;
		if (b1 == null || b2 == null) return false;
		if (b1.Length != b2.Length) return false;
		for (int i=0; i < b1.Length; i++)
		{
			if (b1[i] != b2[i]) return false;
		}
		return true;
	}
}
