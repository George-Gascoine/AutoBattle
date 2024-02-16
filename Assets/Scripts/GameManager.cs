using Godot;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

public partial class GameManager : Node2D
{
    public Player player;
    public UI UI { get; set; }
    [Export]
    public PackedScene damageNumber;

    [Export]
    public Json characterJSON, enemyJSON, dropsJSON;
    public List<Player.Character> characterData;
    public List<Enemy.EnemyData> enemyData;
    public List<Drop.Data> dropData;
    public bool paused;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		paused = false;
        player = (Player)GetNode("/root/World/Player");
        UI = (UI)GetNode("/root/World/UI");
        UI.CallDeferred("UISetup");
        

        ReadJSON();
        player.data = characterData.First(character => character.name == "Pestilas");
        UI.abilities = player.data.abilityIDs;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        GetInput();
	}

    public void ReadJSON()
    {
        //Character Data Parse
        JObject characters = JObject.Parse(characterJSON.Data.ToString());
        List<JToken> jCharacters = characters["character"].Children().ToList();
        characterData = jCharacters.Select(character => character.ToObject<Player.Character>()).ToList();
        //Enemy Data Parse
        JObject enemies = JObject.Parse(enemyJSON.Data.ToString());
        List<JToken> jEnemies = enemies["enemy"].Children().ToList();
        enemyData = jEnemies.Select(enemy => enemy.ToObject<Enemy.EnemyData>()).ToList();
        //Drop Data Parse
        JObject drops = JObject.Parse(dropsJSON.Data.ToString());
        List<JToken> jDrops = drops["drop"].Children().ToList();
        dropData = jDrops.Select(drop => drop.ToObject<Drop.Data>()).ToList();
    }

    public void GetInput()
    {
        if (Input.IsActionJustPressed("1"))
        {
            player.abilityManager.UseAbility(player.data.abilityIDs[0]);
        }
    }
    public void CreateDamageNumber(int damage, Vector2 position)
	{
        DamageNumber number = (DamageNumber)damageNumber.Instantiate();
        Node2D world = GetNode<Node2D>("/root/World");
        world.AddChild(number);
		number.Position = position - new Vector2(6,20);
		number.damageTaken = damage;
		number.DisplayDamage();
    }

    public void GetSprite(Sprite2D sprite, int spriteID, int columns, int width, int height, string texturePath)
    {
        int spriteColumns = columns;
        sprite.RegionEnabled = true;
        Texture2D texture = (Texture2D)ResourceLoader.Load(texturePath);
        sprite.Texture = texture;
        int spriteX = Mathf.FloorToInt(spriteID % spriteColumns);
        int spriteY = (spriteID / spriteColumns);
        sprite.RegionRect = new Rect2((spriteX * width), spriteY * height, new Vector2(width, height));
    }
}
