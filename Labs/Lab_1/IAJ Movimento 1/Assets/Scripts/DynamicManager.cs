using Assets.Scripts.IAJ.Unity.Movement;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;
using UnityEngine;
using UnityEngine.UI;

public class DynamicManager : MonoBehaviour {

    private const float X_WORLD_SIZE = 55;
    private const float Z_WORLD_SIZE = 32.5f;
    private const float MAX_ACCELERATION = 20.0f;
    private const float MAX_SPEED = 20.0f;
    private const float DRAG = 0.9f;

	public DynamicCharacter RedCharacter { get; set; }
	public DynamicCharacter GreenCharacter { get; set; }

    private Text RedMovementText { get; set; }
    private Text GreenMovementText { get; set; }

    private DynamicWander RedDynamicWander { get; set; }
    private DynamicWander GreenDynamicWander { get; set; }
    private DynamicSeek RedDynamicSeek { get; set; }
    private DynamicSeek GreenDynamicSeek { get; set; }
    private DynamicFlee RedDynamicFlee { get; set; }
    private DynamicFlee GreenDynamicFlee { get; set; }
    private DynamicArrive RedDynamicArrive { get; set; }
    private DynamicArrive GreenDynamicArrive { get; set; }
    

    public GameObject redDebugTarget;
    public GameObject greenDebugTarget;


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
		if(redObj != null) this.RedCharacter = new DynamicCharacter(redObj)
		{
		    Drag = DRAG,
            MaxSpeed = MAX_SPEED
		};
		var greenObj = GameObject.Find ("Green");
		if (greenObj != null) this.GreenCharacter = new DynamicCharacter(greenObj)
		{
		    Drag = DRAG,
            MaxSpeed = MAX_SPEED
		};

	    this.RedMovementText = GameObject.Find("RedMovement").GetComponent<Text>();
	    this.GreenMovementText = GameObject.Find("GreenMovement").GetComponent<Text>();

        #region movement initialization

	    var redKinematicData = new KinematicData(new StaticData(this.RedCharacter.GameObject.transform.position));
	    var greenKinematicData = new KinematicData(new StaticData(this.GreenCharacter.GameObject.transform.position));

        this.RedDynamicSeek = new DynamicSeek
        {
            Character = redKinematicData,
            Target = this.GreenCharacter.KinematicData,
            MaxAcceleration = MAX_ACCELERATION
        };

        this.RedDynamicFlee = new DynamicFlee
		{
            Character = redKinematicData,
			Target = this.GreenCharacter.KinematicData,
			MaxAcceleration = MAX_ACCELERATION
		};

        this.RedDynamicArrive = new DynamicArrive
        {
            Character = redKinematicData,
            Target = this.GreenCharacter.KinematicData,
            MovingTarget = this.GreenCharacter.KinematicData,
            MaxAcceleration = MAX_ACCELERATION
        };

	    this.RedDynamicWander = new DynamicWander();

        this.GreenDynamicSeek = new DynamicSeek
        {
            Character = greenKinematicData,
            Target = this.RedCharacter.KinematicData,
            MaxAcceleration = MAX_ACCELERATION
        };

        this.GreenDynamicFlee = new DynamicFlee
        {
            Character = greenKinematicData,
            Target = this.RedCharacter.KinematicData,
            MaxAcceleration = MAX_ACCELERATION
        };

        this.GreenDynamicArrive = new DynamicArrive
        {
            Character = greenKinematicData,
            Target = this.RedCharacter.KinematicData,
            MovingTarget = this.RedCharacter.KinematicData,
            MaxAcceleration = MAX_ACCELERATION
        };

        this.GreenDynamicWander = new DynamicWander();
        
        #endregion
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Q)) 
		{
			this.RedCharacter.Movement = null;
		} 
		else if (Input.GetKeyDown (KeyCode.W))
		{
		    this.RedCharacter.Movement = this.RedDynamicSeek;
		}
		else if (Input.GetKeyDown (KeyCode.E))
		{
		    this.RedCharacter.Movement = this.RedDynamicFlee;
		}
		
		else if (Input.GetKeyDown (KeyCode.T))
		{
		    this.RedCharacter.Movement = this.RedDynamicWander;
		}

        else if(Input.GetKeyDown(KeyCode.R))
        {
            this.RedCharacter.Movement = this.RedDynamicArrive;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            this.GreenCharacter.Movement = null;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            this.GreenCharacter.Movement = this.GreenDynamicSeek;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            this.GreenCharacter.Movement = this.GreenDynamicFlee;
        }

        else if (Input.GetKeyDown(KeyCode.G))
        {
            this.GreenCharacter.Movement = this.GreenDynamicWander;
        }

        else if (Input.GetKeyDown(KeyCode.F))
            this.GreenCharacter.Movement = this.GreenDynamicArrive;

        this.UpdateMovingGameObject(this.RedCharacter);
        this.UpdateMovingGameObject(this.GreenCharacter);

	    if (this.redDebugTarget != null && this.RedCharacter.Movement != null)
	    {
	        this.redDebugTarget.transform.position = this.RedCharacter.Movement.Target.position;
	    }

	    if (this.greenDebugTarget != null && this.GreenCharacter.Movement != null)
	    {
	        this.greenDebugTarget.transform.position = this.GreenCharacter.Movement.Target.position;
	    }

	    this.UpdateMovementText();
	}

    private void UpdateMovingGameObject(DynamicCharacter movingCharacter)
    {
        if (movingCharacter.Movement != null)
        {
            movingCharacter.Update();
            movingCharacter.KinematicData.ApplyWorldLimit(X_WORLD_SIZE,Z_WORLD_SIZE);
            movingCharacter.GameObject.transform.position = movingCharacter.KinematicData.position;
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
