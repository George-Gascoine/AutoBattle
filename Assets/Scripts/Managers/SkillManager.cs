using Godot;
using System.Collections.Generic;
using System;


public partial class SkillManager : Control
{
    public Player player;
    public int availableSkillPoints;
    public class Skill
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public List<string> requirements { get; set; }
        public List<int> connections { get; set; }
        public int levels { get; set; }
        public SkillEffect effect { get; set; }
        
        public Action<Player> GetEffectAction()
        {
            switch (effect.type)
            {
                case "increase":
                    switch (effect.attribute)
                    {
                        case "MovementSpeed":
                            return player => player.Speed *= 1 + effect.amount;
                        case "Damage":
                            return player => player.damage *= 1 + effect.amount;
                        case "Health":
                            return player => player.Speed *= 1 + effect.amount;
                            // Other attributes...
                    }
                    break;
                    // Other effect types...
            }

            return null;
        }
    }
    public class SkillEffect
    {
        public string type { get; set; }
        public string attribute { get; set; }
        public float amount { get; set; }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        player = GetNode<Player>("/root/Level/Player"); 
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

    public void ApplySkillPoint()
    {
        if(availableSkillPoints > 0)
        {
            availableSkillPoints--;
        }
    }
}
