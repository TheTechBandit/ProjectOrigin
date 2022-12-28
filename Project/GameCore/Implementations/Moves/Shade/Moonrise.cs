﻿using System.Collections.Generic;

namespace ProjectOrigin
{
    public class Moonrise : BasicMove
    {
        public override string Name { get; } = "Moonrise";
        public override string Description { get; } = "The user performs a ritual, raising the moon and darkening the area.";
        public override BasicType Type { get; } = new ShadeType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = -1;
        public override int MaxPP { get; } = 5;
        public override string TargetType { get; } = "None";

        public Moonrise() : base()
        {

        }

        public Moonrise(bool newmove) : base(newmove)
        {
            CurrentPP = MaxPP;
        }

        public override List<MoveResult> ApplyMove(CombatInstance inst, BasicMon owner, List<BasicMon> targets)
        {
            ResetResult();
            AddResult();

            //Fail logic
            if (SelfMoveFailLogic(owner))
            {
                Result[TargetNum].Fail = true;
                Result[TargetNum].Hit = false;
            }
            //Hit logic
            else
            {
                CurrentPP--;
                Result[TargetNum].Messages.Add(inst.Environment.AttemptMoonrise());
            }

            return Result;
        }
    }
}