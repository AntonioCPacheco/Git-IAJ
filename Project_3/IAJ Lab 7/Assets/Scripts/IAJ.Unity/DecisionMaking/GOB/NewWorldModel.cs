using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.GOB
{
    public class NewWorldModel : WorldModel
    {
        private object[] Properties { get; set; }
        private bool[] Enemies { get; set; }
        private bool[] Collectibles { get; set; }
        private List<Action> Actions { get; set; }

        public NewWorldModel(List<Action> actions) : base(actions)
        {
            this.Actions = actions;
            this.ActionEnumerator = actions.GetEnumerator();
            initialize();
        }

        public NewWorldModel(NewWorldModel parent) : base(parent)
        {
            
            this.Actions = parent.Actions;
            this.Parent = parent;
            this.ActionEnumerator = this.Actions.GetEnumerator();
            initialize();
        }

        public void initialize()
        {

            int i = 0, j = 0,n;
            
            foreach (var c in GameObject.FindGameObjectsWithTag("Chest"))
                i++;
            foreach (var c in GameObject.FindGameObjectsWithTag("ManaPotion"))
                i++;
            foreach (var c in GameObject.FindGameObjectsWithTag("HealthPotion"))
                i++;
            foreach (var c in GameObject.FindGameObjectsWithTag("Skeleton"))
                j++;
            foreach (var c in GameObject.FindGameObjectsWithTag("Orc"))
                j++;
            foreach (var c in GameObject.FindGameObjectsWithTag("Dragon"))
                j++;
            this.Enemies = new bool[j];
            for (n = 0; n < j; n++)
                this.Enemies[n] = true;
            this.Collectibles = new bool[i];
            for (n = 0; n < i; n++)
                this.Collectibles[n] = true;

            this.Properties = new object[8];
            SetProperty("Mana", 0);
            SetProperty("HP", 10);
            SetProperty("MAXHP", 10);
            SetProperty("XP", 0);
            SetProperty("Time", 0f);
            SetProperty("Level", 1);
            SetProperty("Money", 0);
            SetProperty("Position", new Vector3(0f,0f,0f));
        }

        public override object GetProperty(string propertyName)
        {
            object property = null;
            switch(propertyName)
            {
                case "Mana":
                    property = this.Properties[0];
                    break;
                case "HP":
                    property = this.Properties[1];
                    break;
                case "MAXHP":
                    property = this.Properties[2];
                    break;
                case "XP":
                    property = this.Properties[3];
                    break;
                case "Time":
                    property = this.Properties[4];
                    break;
                case "Money":
                    property = this.Properties[5];
                    break;
                case "Level":
                    property = this.Properties[6];
                    break;
                case "Position":
                    property = this.Properties[7];
                    break;
                case "Chest":
                    property = this.Collectibles[0];
                    break;
                case "Chest (1)":
                    property = this.Collectibles[1];
                    break;
                case "Chest (2)":
                    property = this.Collectibles[2];
                    break;
                case "Chest (3)":
                    property = this.Collectibles[3];
                    break;
                case "Chest (4)":
                    property = this.Collectibles[4];
                    break;
                case "ManaPotion":
                    property = this.Collectibles[5];
                    break;
                case "ManaPotion (1)":
                    property = this.Collectibles[6];
                    break;
                case "HealthPotion":
                    property = this.Collectibles[7];
                    break;
                case "HealthPotion (1)":
                    property = this.Collectibles[8];
                    break;
                case "Skeleton (2)":
                    property = this.Enemies[0];
                    break;
                case "Skeleton (3)":
                    property = this.Enemies[1];
                    break;
                case "Orc":
                    property = this.Enemies[2];
                    break;
                case "Orc (1)":
                    property = this.Enemies[3];
                    break;
                case "Dragon":
                    property = this.Enemies[4];
                    break;
            }
            return property;
        }

        public override void SetProperty(string propertyName, object value)
        {
            switch (propertyName)
            {
                case "Mana":
                    this.Properties[0] = value;
                    break;
                case "HP":
                    this.Properties[1] = value;
                    break;
                case "MAXHP":
                    this.Properties[2] = value;
                    break;
                case "XP":
                    this.Properties[3] = value;
                    break;
                case "Time":
                    this.Properties[4] = value;
                    break;
                case "Money":
                    this.Properties[5] = value;
                    break;
                case "Level":
                    this.Properties[6] = value;
                    break;
                case "Position":
                    this.Properties[7] = value;
                    break;
                case "Chest":
                    this.Collectibles[0] = (bool)value;
                    break;
                case "Chest (1)":
                    this.Collectibles[1] = (bool)value;
                    break;
                case "Chest (2)":
                    this.Collectibles[2] = (bool)value;
                    break;
                case "Chest (3)":
                    this.Collectibles[3] = (bool)value;
                    break;
                case "Chest (4)":
                    this.Collectibles[4] = (bool)value;
                    break;
                case "ManaPotion":
                    this.Collectibles[5] = (bool)value;
                    break;
                case "ManaPotion (1)":
                    this.Collectibles[6] = (bool)value;
                    break;
                case "HealthPotion":
                    this.Collectibles[7] = (bool)value;
                    break;
                case "HealthPotion (1)":
                    this.Collectibles[8] = (bool)value;
                    break;
                case "Skeleton (2)":
                    this.Enemies[0] = (bool)value;
                    break;
                case "Skeleton (3)":
                    this.Enemies[1] = (bool)value;
                    break;
                case "Orc":
                    this.Enemies[2] = (bool)value;
                    break;
                case "Orc (1)":
                    this.Enemies[3] = (bool)value;
                    break;
                case "Dragon":
                    this.Enemies[4] = (bool)value;
                    break;
            }
        }

        public override WorldModel GenerateChildWorldModel()
        {
            return new NewWorldModel(this);
        }
    }
}
