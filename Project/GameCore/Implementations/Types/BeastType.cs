﻿using System.Collections.Generic;

namespace ProjectOrigin
{
    public class BeastType : BasicType
    {
        public override string Type { get; } = "Beast";
        public override List<BasicType> Advantages { get; }
        public override List<BasicType> Disadvantages { get; }
        public override List<BasicType> Immunities { get; }
        public override string Description { get; } = "";

        public BeastType() : base()
        {

        }

        public BeastType(bool newtype) : base(newtype)
        {
            Advantages = new List<BasicType>()
            {
                new NatureType(),
                new PrimalType()
            };
            Disadvantages = new List<BasicType>()
            {
                new FireType(),
                new FeyType(),
                new PsychicType()
            };
            Immunities = new List<BasicType>()
            {
                new ShadeType()
            };
        }

    }
}