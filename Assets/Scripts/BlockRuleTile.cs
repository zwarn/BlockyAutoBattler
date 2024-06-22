using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockRuleTile : RuleTile
{
    public enum BlockType
    {
        Red
    }

    public BlockType type;

    public override bool RuleMatch(int neighbor, TileBase other)
    {
        if (other is BlockRuleTile otherRuleTile)
            switch (neighbor)
            {
                case TilingRuleOutput.Neighbor.This: return otherRuleTile.type == type;
                case TilingRuleOutput.Neighbor.NotThis: return otherRuleTile.type != type;
            }

        return base.RuleMatch(neighbor, other);
    }
}