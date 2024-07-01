using System;
using System.Collections.Generic;

[Serializable]
public class PlayerModel
{
    public int player_id;
    public string player_username;
    public string player_nickname;
    public PlayerAttributes attributes;
}

[Serializable]
public class PlayerAttributes
{
    public int element_stone;
    public int skill_summon_tickets;
    public int money;
    public int attack_power;
    public int max_health;
    public float critical_chance;
    public float critical_damage;
    public string current_stage;
    public int level;
    public int awakening;
    public int? guild_id;
    public int? equipped_skill1_id;
    public int? equipped_skill2_id;
    public int? equipped_skill3_id;
    public int combat_power;
    public int rank;

    public void CalculateCombatPower()
    {
        double baseStats = attack_power + critical_chance + max_health + critical_damage;
        double awakeningMultiplier = awakening == 0 ? 1 : awakening * 10;
        double levelMultiplier = level * 0.1;

        combat_power = (int)(baseStats * awakeningMultiplier * levelMultiplier);
    }
}

[Serializable]
public class MailModel
{
    public int id;
    public int user_id;
    public string type;
    public string reward;
    public string created_at;
    public string expires_at;
    public bool is_read;
}

[Serializable]
public class GuildModel
{
    public int guild_id;
    public string guild_name;
    public int guild_leader;
}

[Serializable]
public class FriendModel
{
    public int player_id;
    public int friend_id;
}

[Serializable]
public class PlayerWeaponModel
{
    public int player_weapon_id;
    public int player_id;
    public int weapon_id;
    public int level;
    public int count;
    public int attack_power;
    public float critical_chance;
    public float critical_damage;
}

[Serializable]
public class SkillModel
{
    public int id;
    public string name;
    public string description;
    public int damage_percentage;
    public string image;
    public int cooldown;
}

[Serializable]
public class PlayerSkillModel
{
    public int player_skill_id;
    public int player_id;
    public int skill_id;
    public int level;
    public SkillModel skill;
}

[Serializable]
public class MissionProgressModel
{
    public int player_id;
    public int last_level_check;
    public int last_combat_power_check;
    public int last_awakening_check;
    public string last_online_time_check;
}

[Serializable]
public class RewardModel
{
    public int id;
    public string name;
    public string description;
    public int reward;
}

[Serializable]
public class MonsterModel
{
    public int id;
    public string Stage;
    public string Type;
    public string Name;
    public int Health;
    public int Attack;
    public int DropMoney;
    public int DropElementStone;
    public float DropElementStoneChance;
}


[Serializable]
public class WeaponModel
{
    public int weapon_id;
    public int weapon_grade;
    public int attack_power;
    public float crit_rate;
    public float crit_damage;
    public long weapon_exp;
    public string prefab_name;
}

[Serializable]
public class SkillSlotModel
{
    public PlayerSkillModel playerSkill; 
    public bool is_empty;

    public SkillSlotModel(PlayerSkillModel playerSkill)
    {
        this.playerSkill = playerSkill;
        is_empty = playerSkill == null;
    }

    // 편의를 위한 프로퍼티들
    public int SkillId => playerSkill?.skill_id ?? -1;
    public string SkillName => playerSkill?.skill.name ?? "Empty";
    public string IconFileName => playerSkill?.skill.image ?? "empty_slot";
}

[Serializable]
public class AttendanceRewardResponse
{
    public string message;
    public int dayCount;
}