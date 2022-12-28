﻿using System.Collections.Generic;

namespace ProjectOrigin
{
    public class Poke : BasicMove
    {
        public override string Name { get; } = "Poke";
        public override string Description { get; } = "The user pokes their enemy, lowering their defense by one stage.";
        public override BasicType Type { get; } = new BeastType(true);
        public override bool Contact { get; } = true;
        public override int Power { get; } = 20;
        public override int Accuracy { get; } = 100;
        public override int MaxPP { get; } = 40;
        public override string TargetType { get; } = "SingleEnemy";

        public Poke() : base()
        {

        }

        public Poke(bool newmove) : base(newmove)
        {
            CurrentPP = MaxPP;
        }

        public override List<MoveResult> ApplyMove(CombatInstance inst, BasicMon owner, List<BasicMon> targets)
        {
            ResetResult();

            foreach (BasicMon t in targets)
            {
                int dmg = 0;
                AddResult();

                //Fail logic
                if (DefaultFailLogic(t, owner))
                {
                    Result[TargetNum].Fail = true;
                    Result[TargetNum].Hit = false;
                }
                //Miss Logic
                else if (!ApplyAccuracy(inst, owner, t))
                {
                    Result[TargetNum].Miss = true;
                    Result[TargetNum].Hit = false;
                }
                //Hit logic
                else
                {
                    CurrentPP--;
                    dmg = ApplyPower(inst, owner, t);
                    t.TakeDamage(dmg);
                    (double mod, string mess) = t.ChangeAttStage(-1);
                    //Result.EnemyStatChanges[1] = -1;
                    Result[TargetNum].StatChangeMessages.Add(mess);
                }

            }

            return Result;
        }
    }
}