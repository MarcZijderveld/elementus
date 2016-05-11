using UnityEngine;
using System.Collections;

public class Elemental_Change : MonoBehaviour 
{
    public Transform particle;
    public Element_Enum.Types element;
    private SoundSettings _soundSettings = null;
    public SoundSettings soundSettings
    {
        get
        {
            if (_soundSettings == null)
            {
                _soundSettings = Hierarchy.GetComponentWithTag<SoundSettings>("SoundManager");
            }
            return _soundSettings;
        }
    } 
    public Transform fireP1;
    public Transform fireP2;
    public Transform waterP1;
    public Transform waterP2;
    public Transform grassP1;
    public Transform grassP2;
    public Vector3 beginPosition;
    public Vector2 beginGridPosition;
    bool fisrt = true;
    Elemental_Movement elementMovement;
	AnimationStates states;
    Transform[] models;
    int value;

    //set the defult values for the type of element.
	public void Init() 
	{
		states = GetComponent<AnimationStates>();
        value = 0;
		models = new Transform[6]{fireP1, fireP2, waterP1, waterP2, grassP1, grassP2};
        element = Element_Enum.Types.fire;
        
        //models[0 + value].gameObject.SetActive(true);
		states.SetAnimationRoot(models[0 + value].gameObject);
		elementMovement = states.movement;
	}
    bool first = false;
    void Update()
    {
        if (!first)
        {
            if (gameObject.GetComponent<Elemental_Properties>().isMine == false)
            {
                value++;
            }
			if(value == 1)
            	StartCoroutine("ChangeElementPlay", Element_Enum.Types.fire);
			else
				StartCoroutine("ChangeElementPlay", Element_Enum.Types.grass);

			first = true;
        }
    }
    public IEnumerator ChangeElementPlay(Element_Enum.Types type)
    {
        if (!fisrt)
        {
            Transform emit = Instantiate(particle, transform.position - new Vector3(0, 0.2f, 0), particle.rotation) as Transform;
            emit.gameObject.AddComponent<DeleteParticle>();
        }
        fisrt = false;

        yield return new WaitForSeconds(0.5f);
        
        for (int i = 0; i < 6; i++)
        {
            foreach (Renderer ren in models[i].gameObject.GetComponentsInChildren<Renderer>())
            {
                ren.enabled = false;
            }
            //.SetActive(false);
        }
        if (element == Element_Enum.Types.fire)
        {
            foreach (Renderer ren in models[0 + value].gameObject.GetComponentsInChildren<Renderer>())
            {
                ren.enabled = true;
            }
            //models[0 + value].gameObject.SetActive(true);
            soundSettings.Play("Swap_Fire");
            states.SetAnimationRoot(models[0 + value].gameObject);
        }
        if (element == Element_Enum.Types.water)
        {
            foreach (Renderer ren in models[2 + value].gameObject.GetComponentsInChildren<Renderer>())
            {
                ren.enabled = true;
            }
            //models[2 + value].gameObject.SetActive(true);
            soundSettings.Play("Swap_Water");
            states.SetAnimationRoot(models[2 + value].gameObject);
        }
        if (element == Element_Enum.Types.grass)
        {
            foreach (Renderer ren in models[4 + value].gameObject.GetComponentsInChildren<Renderer>())
            {
                ren.enabled = true;
            }
            //models[4 + value].gameObject.GetComponent<(true);
            soundSettings.Play("Swap_Grass");
            states.SetAnimationRoot(models[4 + value].gameObject);
        }
        yield return null;
    }

    //change the element of the unit and change the look
	public IEnumerator ChangeElement(Element_Enum.Types type)
	{
        if (type != element)
        {
            element = type;
            yield return StartCoroutine("ChangeElementPlay", type);
        }
	}
}
