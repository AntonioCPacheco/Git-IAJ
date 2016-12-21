using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.IAJ.Unity.Movement.Arbitration;
using Assets.Scripts.IAJ.Unity.Movement;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;
using Assets.Scripts.IAJ.Unity.Util;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PriorityManager : MonoBehaviour
{
    public const float X_WORLD_SIZE = 55;
    public const float Z_WORLD_SIZE = 32.5f;
    public const float AVOID_MARGIN = 30.0f;
    public const float MAX_SPEED = 40.0f;
    public const float MAX_ACCELERATION = 40.0f;
    public const float MAX_LOOK_AHEAD = 20.0f;
    public const float DRAG = 0.1f;

    private DynamicCharacter RedCharacter { get; set; }

    //private Text RedMovementText { get; set; }

    private BlendedMovement Blended { get; set; }

    private PriorityMovement Priority { get; set; }

    private List<DynamicCharacter> Characters { get; set; }

    // Use this for initialization
    void Start()
    {
        Application.targetFrameRate = 60;
        var redObj = GameObject.Find("Red");

        this.RedCharacter = new DynamicCharacter(redObj)
        {
            Drag = DRAG,
            MaxSpeed = MAX_SPEED
        };

        var obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        //this.Characters = this.CloneSecondaryCharacters(redObj, Random.Range(20, 30), obstacles);
        //this.Characters.Add(this.RedCharacter);

        this.InitializeMainCharacter(obstacles);

        //initialize all but the last character (because it was already initialized as the main character)
        //foreach (var character in this.Characters.Take(this.Characters.Count - 1))
        //{
        //    this.InitializeSecondaryCharacter(character, obstacles);
        //}
    }

    private void InitializeMainCharacter(GameObject[] obstacles)
    {

        this.Priority = new PriorityMovement
        {
            Character = this.RedCharacter.KinematicData
        };

        this.Blended = new BlendedMovement
        {
            Character = this.RedCharacter.KinematicData
        };
        /*
        foreach (var obstacle in obstacles)
        {
            //TODO: add your AvoidObstacle movement here
            var avoidObstacleMovement = new DynamicAvoidObstacle(obstacle)
            {

                MaxAcceleration = MAX_ACCELERATION,
                avoidDistance = 5f,
                lookAhead = 10f,
                Character = this.RedCharacter.KinematicData,
                MovementDebugColor = Color.magenta
            };
            this.Blended.Movements.Add(new MovementWithWeight(avoidObstacleMovement, 8f));
            //this.Priority.Movements.Add(avoidObstacleMovement);
        }

        var wander = new DynamicWander
        {
            //....
            TurnAngle = 0.2f,
            WanderRadius = 10f,
            WanderOffset = 10f,
            MaxAcceleration = MAX_ACCELERATION,
            Character = this.RedCharacter.KinematicData,
            MovementDebugColor = Color.yellow
        };

        //this.Priority.Movements.Add(wander);
        this.Blended.Movements.Add(new MovementWithWeight(wander, 1f));
        
        var separation = new Separation
        {
            MaxAcceleration = MAX_ACCELERATION,
            separationFactor = 3f,
            radius = 5f,
            flock = Characters
        };

        var flockVelocityMatch = new FlockVelocityMatching
        {
            flock = Characters,
            radius = 5.0f,
            fanAngle = 60.0f,
            Target = this.RedCharacter.KinematicData,
            Character = this.RedCharacter.KinematicData,
            //Target = new KinematicData(new StaticData(character.KinematicData.position)),
            //MovingTarget = this.RedCharacter.KinematicData,
            TimeToTargetSpeed = .1f
        };

        var cohesion = new Cohesion
        {
            flock = Characters,
            radius = 5.0f,
            fanAngle = 60.0f,
            //stopRadius = 0.5f,
            maxSpeed = MAX_SPEED,
            slowRadius = 1f,
        };

        var fleeFromClick = new FleeFromClick
        {
            camera = Camera.main,
            fleeRadius = 10f,
            MaxAcceleration = MAX_ACCELERATION

        };

        this.Blended.Movements.Add(new MovementWithWeight(fleeFromClick, 4f));
        this.Blended.Movements.Add(new MovementWithWeight(separation, 1f));
        this.Blended.Movements.Add(new MovementWithWeight(flockVelocityMatch, 2f));
        this.Blended.Movements.Add(new MovementWithWeight(cohesion, 2f));
        
        */
        var VO = new VelocityObstacle(obstacles)
        {
            Character = this.RedCharacter.KinematicData
        };
        this.Blended.Movements.Add(new MovementWithWeight(VO, 4f));
        this.RedCharacter.Movement = this.Blended;
    }

    private void InitializeSecondaryCharacter(DynamicCharacter character, GameObject[] obstacles)
    {
        bool avoidingObstacle = false;
        this.Priority = new PriorityMovement
        {
            Character = character.KinematicData
        };

        this.Blended = new BlendedMovement
        {
            Character = character.KinematicData
        };

        foreach (var obstacle in obstacles)
        {

            //TODO: add your AvoidObstacle movement here
            var avoidObstacleMovement = new DynamicAvoidObstacle(obstacle)
            {
                MaxAcceleration = 100f,
                avoidDistance = 10f,
                lookAhead = 10f
            };

            //Target = character.KinematicData
            //MovementDebugColor = Color.magenta
            if (!avoidingObstacle)
            {
                avoidingObstacle = avoidObstacleMovement._avoiding;
            }
            this.Blended.Movements.Add(new MovementWithWeight(avoidObstacleMovement, 8f));
            //this.Priority.Movements.Add(avoidObstacleMovement);

        }

        var straight = new DynamicStraightAhead
        {
            MaxAcceleration = MAX_ACCELERATION,
            Target = character.KinematicData
        };


        var separation = new Separation
        {
            MaxAcceleration = MAX_ACCELERATION,
            separationFactor = 3f,
            radius = 5f,
            flock = Characters
        };

        var flockVelocityMatch = new FlockVelocityMatching
        {
            flock = Characters,
            radius = 5.0f,
            fanAngle = 60.0f,
            Target = character.KinematicData,
            Character = character.KinematicData,
            TimeToTargetSpeed = .1f
        };

        var cohesion = new Cohesion
        {
            flock = Characters,
            radius = 5.0f,
            fanAngle = 60.0f,
            maxSpeed = MAX_SPEED,
            slowRadius = 1f,
        };

        var fleeFromClick = new FleeFromClick
        {
            camera = Camera.main,
            fleeRadius = 10f,
            MaxAcceleration = MAX_ACCELERATION

        };

        this.Blended.Movements.Add(new MovementWithWeight(straight, 3f));
        this.Blended.Movements.Add(new MovementWithWeight(fleeFromClick, 4f));
        this.Blended.Movements.Add(new MovementWithWeight(separation, 1f));
        this.Blended.Movements.Add(new MovementWithWeight(flockVelocityMatch, 3f));
        this.Blended.Movements.Add(new MovementWithWeight(cohesion, 2f));
        character.Movement = this.Blended;

    }

    private List<DynamicCharacter> CloneSecondaryCharacters(GameObject objectToClone, int numberOfCharacters, GameObject[] obstacles)
    {
        var characters = new List<DynamicCharacter>();
        for (int i = 0; i < numberOfCharacters; i++)
        {
            var clone = GameObject.Instantiate(objectToClone);
            //clone.transform.position = new Vector3(30,0,i*20);
            clone.transform.position = this.GenerateRandomClearPosition(obstacles);
            var character = new DynamicCharacter(clone)
            {
                MaxSpeed = MAX_SPEED,
                Drag = DRAG
            };
            //character.KinematicData.orientation = (float)Math.PI*i;
            characters.Add(character);
        }

        return characters;
    }


    private Vector3 GenerateRandomClearPosition(GameObject[] obstacles)
    {
        Vector3 position = new Vector3();
        var ok = false;
        while (!ok)
        {
            ok = true;

            position = new Vector3(Random.Range(-X_WORLD_SIZE, X_WORLD_SIZE), 0, Random.Range(-Z_WORLD_SIZE, Z_WORLD_SIZE));

            foreach (var obstacle in obstacles)
            {
                var distance = (position - obstacle.transform.position).magnitude;

                //assuming obstacle is a sphere just to simplify the point selection
                if (distance < obstacle.transform.localScale.x + AVOID_MARGIN)
                {
                    ok = false;
                    break;
                }
            }
        }

        return position;
    }
    
    void Update()
    {
        /*foreach (var character in this.Characters)
        {
            this.UpdateMovingGameObject(character);
        }*/
        this.UpdateMovingGameObject(this.RedCharacter);

    }

    private void UpdateMovingGameObject(DynamicCharacter movingCharacter)
    {
        if (movingCharacter.Movement != null)
        {
            movingCharacter.Update();
            movingCharacter.KinematicData.ApplyWorldLimit(X_WORLD_SIZE, Z_WORLD_SIZE);
            movingCharacter.GameObject.transform.position = movingCharacter.Movement.Character.position;
        }
    }

}
