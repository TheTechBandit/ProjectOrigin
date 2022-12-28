﻿using System.Collections.Generic;

namespace ProjectOrigin
{
    public class PsychicType : BasicType
    {
        public override string Type { get; } = "Psychic";
        public override List<BasicType> Advantages { get; }
        public override List<BasicType> Disadvantages { get; }
        public override List<BasicType> Immunities { get; }
        public override string Description { get; } = "";

        public PsychicType() : base()
        {

        }

        public PsychicType(bool newtype) : base(newtype)
        {
            Advantages = new List<BasicType>()
            {
                new BeastType(),
                new MetalType(),
                new ShadeType()
            };
            Disadvantages = new List<BasicType>()
            {
                new PsychicType()
            };
            Immunities = new List<BasicType>()
            {

            };
        }

    }
}