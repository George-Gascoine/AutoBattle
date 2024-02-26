using Godot;
using System;
using System.Linq;

public partial class SkillButton : TextureButton
{
    public GameManager gameManager;
    public Player player;
    [Export]
    public int skillID;
    public SkillManager skillManager;
    public SkillManager.Skill skill;
    public Label label, tooltipText;
    public Panel tooltip;
    public Line2D line { get; set; }
    public int level;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        gameManager = (GameManager)GetNode("/root/GameManager");
        //skillManager
        player = GetNode<Player>("/root/Level/Player");
        skill = gameManager.skillData.First(skill => skill.id == skillID);
        tooltip = GetNode<Panel>("Tooltip");
        tooltipText = tooltip.GetChild(0).GetNode<Label>("TooltipText");
        label = GetNode<Label>("Label");
        line = GetNode<Line2D>("Line2D");
        label.Text = level.ToString() + "/" + skill.levels.ToString();
        DrawSkillLine();
        SetTooltipText();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (IsHovered())
        {
            DisplayTooltip();
        }
        else
        {
            tooltip.Visible = false;
        }
    }

    public void SetTooltipText()
    {
        tooltipText.Text = skill.description;
        GD.Print(skill.description);
    }

    public void OnPressed()
    {
        if(level < skill.levels)
        {
            level += 1;
            label.Text = level.ToString() + "/" + skill.levels.ToString();
            //skillManager.ApplySkillPoint();
            SelfModulate = new Color(1, 1, 1, 1);
            line.DefaultColor = new Color(0.6f, 0.8f, 0.2f, 1);

            Action<Player> effectAction = skill.GetEffectAction();
            effectAction(player);

            var derivedSkills = GetChildren();
            foreach (var skillButton in derivedSkills)
            {
                if (skillButton is SkillButton)
                {
                    if (level == skill.levels)
                    {
                        SkillButton enableThis = skillButton as SkillButton;
                        enableThis.Disabled = false;
                    }
                }
            }
        }
    }

    public void DisplayTooltip()
    {
        tooltip.Visible = true;
        tooltip.GlobalPosition = GetGlobalMousePosition();
    }

    public void DrawSkillLine()
    {
        if (GetParent() is SkillButton)
        {
            line.AddPoint(GlobalPosition + Size / 2);
            line.AddPoint(GetParent<SkillButton>().GlobalPosition + Size / 2);
        }
    }
}
