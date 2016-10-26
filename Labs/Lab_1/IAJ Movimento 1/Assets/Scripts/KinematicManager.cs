using Assets.Scripts.IAJ.Unity.Movement.KinematicMovement;
using Assets.Scripts.IAJ.Unity.Util;
using UnityEngine;
using UnityEngine.UI;

public class KinematicManager : MonoBehaviour
{
    public const float X_WORLD_SIZE = 55;
    public const float Z_WORLD_SIZE = 32.5f;
    private const float MAX_SPEED = 10.0f;
    private const float TIME_TO_TARGET = 2.0f;
    private const float RADIUS = 1.0f;
    private const float MAX_ROTATION = 8*MathConstants.MATH_PI;

	private KinematicCharacter RedCharacter { get; set; }
	private KinematicCharacter GreenCharacter { get; set; }

    private Text RedMovementText { get; set; }

    private Text GreenMovementText { get; set; }

	// Use this for initialization
	void Start () 
	{
		var textObj = GameObject.Find ("InstructionsText");
		if (textObj != null) 
		{
			textObj.GetComponent<Text>().text = 
				"Instructions\n\n" +
				"Red Character\n" +
				"Q - Stationary\n" +
				"W - Seek\n" + 
				"E - Flee\n" + 
				"R - Arrive\n" + 
				"T - Wander\n\n" +  
				"Green Character\n" + 
				"A - Stationary\n" +
				"S - Seek\n" +
				"D - Flee\n" + 
				"F - Arrive\n" +
				"G - Wander\n"; 
		}

		var redObj = GameObject.Find ("Red");
		if(redObj != null) this.RedCharacter = new KinematicCharacter(redObj);
		var greenObj = GameObject.Find ("Green");
		if (greenObj != null) this.GreenCharacter = new KinematicCharacter(greenObj);

	    this.RedMovementText = GameObject.Find("RedMovement").GetComponent<Text>();
	    this.GreenMovementText = GameObject.Find("GreenMovement").GetComponent<Text>();
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Q)) 
		{
			this.RedCharacter.Movement = null;
		} 
		else if (Input.GetKeyDown (KeyCode.W)) 
		{
            this.RedCharacter.Movement = new KinematicSeek
			{
                Target = this.GreenCharacter.StaticData,
			    MaxSpeed = MAX_SPEED
			};
		}
		else if (Input.GetKeyDown (KeyCode.E)) 
		{
            this.RedCharacter.Movement = new KinematicFlee
			{
                Target = this.GreenCharacter.StaticData,
				MaxSpeed = MAX_SPEED
			};
		}
		else if (Input.GetKeyDown (KeyCode.R)) 
		{
            this.RedCharacter.Movement = new KinematicArrive
			{
                Target = this.GreenCharacter.StaticData,
                MaxSpeed = MAX_SPEED,
                TimeToTarget = TIME_TO_TARGET,
                Radius = RADIUS
			};
		}
		else if (Input.GetKeyDown (KeyCode.T)) 
		{
            this.RedCharacter.Movement = new KinematicWander
			{
				MaxRotation = MAX_ROTATION,
				MaxSpeed = MAX_SPEED
			};
		}
        if (Input.GetKeyDown(KeyCode.A))
        {
            this.GreenCharacter.Movement = null;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            this.GreenCharacter.Movement = new KinematicSeek
            {
                Target = this.RedCharacter.StaticData,
                MaxSpeed = MAX_SPEED
            };
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            this.GreenCharacter.Movement = new KinematicFlee
            {
                Target = this.RedCharacter.StaticData,
                MaxSpeed = MAX_SPEED
            };
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            this.GreenCharacter.Movement = new KinematicArrive
            {
                Target = this.RedCharacter.StaticData,
                MaxSpeed = MAX_SPEED,
                TimeToTarget = TIME_TO_TARGET,
                Radius = RADIUS
            };
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            this.GreenCharacter.Movement = new KinematicWander
            {
                MaxRotation = MAX_ROTATION,
                MaxSpeed = MAX_SPEED
            };
        }

        this.UpdateMovingGameObject(this.RedCharacter);
        this.UpdateMovingGameObject(this.GreenCharacter);

        this.UpdateMovementText();
	}

    private void UpdateMovingGameObject(KinematicCharacter movingCharacter)
    {
        if (movingCharacter.Movement != null)
        {
            movingCharacter.Update();
            movingCharacter.StaticData.ApplyWorldLimit(X_WORLD_SIZE,Z_WORLD_SIZE);
            movingCharacter.GameObject.transform.position = movingCharacter.StaticData.position;
        }
    }

    private void UpdateMovementText()
    {
        if (this.GreenCharacter.Movement == null)
        {
            this.GreenMovementText.text = "Green Movement: Stationary";
        }
        else
        {
            this.GreenMovementText.text = "Green Movement: " + this.GreenCharacter.Movement.Name;
        }

        if (this.RedCharacter.Movement == null)
        {
            this.RedMovementText.text = "Red Movement: Stationary";
        }
        else
        {
            this.RedMovementText.text = "Red Movement: " + this.RedCharacter.Movement.Name;
        }
    }
}
