using Godot;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using static WaveManager;
using static Level;
using System.Reflection.Emit;
using System;
using System.Diagnostics;

public partial class GameManager : Node2D
{
    public Level currentLevel;
    public int score;

    public TimeSpan time = TimeSpan.Zero;

    public Player player;
    public UI UI { get; set; }
    [Export]
    public PackedScene damageNumber;

    public bool roundStarted;

    [Export]
    public Json characterJSON, enemyJSON, dropsJSON, levelsJSON, wavesJSON;
    public List<Player.Character> characterData;
    public List<Enemy.EnemyData> enemyData;
    public List<Drop.Data> dropData;
    public List<LevelData> levelData;
    public List<Wave> waveData;
    public bool paused;



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        roundStarted = false;
        ReadJSON();
        LevelData chosenLevel = levelData.First(level => level.id == 0);
        StartRound(chosenLevel);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (roundStarted)
        {
            // Add the time since the last frame
            time += TimeSpan.FromSeconds(delta);

            // Format the time as hh:mm:ss
            string timeStr = string.Format("{0:D2}:{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);

            // Update UI timer
            UI.UpdateRoundTimer(timeStr);
 
            // Display the time (replace this with your own display code)
            GetInput();
        }
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
        //Level Data Parse
        JObject levels = JObject.Parse(levelsJSON.Data.ToString());
        List<JToken> jLevels = levels["level"].Children().ToList();
        levelData = jLevels.Select(level => level.ToObject<LevelData>()).ToList();
        //Wave Data Parse
        JObject waves = JObject.Parse(wavesJSON.Data.ToString());
        List<JToken> jWaves = waves["wave"].Children().ToList();
        waveData = jWaves.Select(wave => wave.ToObject<Wave>()).ToList();
    }

    public void StartRound(LevelData level)
    {
        paused = false;
        time = TimeSpan.Zero;
        score = 0;
        currentLevel = (Level)GetNode("/root/Level");
        currentLevel.data = level;
        player = (Player)GetNode("/root/Level/Player");
        UI = (UI)GetNode("/root/Level/UI");
        UI.CallDeferred("UISetup");

        player.data = characterData.First(character => character.name == "Pestilas");
        UI.abilities = player.data.abilityIDs;
        player.CallDeferred("PlayerSetup");
        roundStarted = true;
    }

    public void GetInput()
    {
        if (Input.IsActionJustPressed("1"))
        {
            player.abilityManager.UseAbility(player.data.abilityIDs[0]);
        }
    }

    public void ScoreUpdate(int scoreToAdd)
    {
        score += scoreToAdd;
        UI.UpdateRoundScore(score);
    }

    public void CreateDamageNumber(int damage, Vector2 position)
	{
        DamageNumber number = (DamageNumber)damageNumber.Instantiate();
        Node2D level = GetNode<Node2D>("/root/Level");
        level.AddChild(number);
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

    //private void SpawnWave()
    //{

    //}
}
