

public class BulletSkillJam : SkillJam
{
    public BulletSkillJam()
    {
        this.skillNum = 0;
        this.skillPower = 0.9f;
        this.skillDistance = 12f;
        this.missileSpeed = 1200f;
        this.skillCool = 0.5f;
        this.skillCost = 10f;
        this.skillRank = 1;
        this.skillElement = (int)Element.None;
        InitiateProperty(skillRank);
    }

    new void InitiateProperty(int skillRank)
    {
        this.skillRank = skillRank;
        skillPower += (skillRank-1) * 0.1f;
        skillCost += (skillRank-1) * 1f;
    }

    new void SetElement(int elementIdx)
    {
        this.skillElement = elementIdx;
    }

    new public object[] SendSkillInfo()
    {
        object[] skillInfo = { skillNum, skillPower, skillDistance, missileSpeed, skillCool, skillCost, skillRank, skillElement };
        return skillInfo;
    }

    enum Element
    {
        None,
        Arcane,
        Fire,
        Ice,
        Poison,
        Lightning
    }
}
