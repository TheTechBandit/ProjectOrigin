﻿using System.Collections.Generic;

namespace ProjectOrigin
{
    public class ColdType : BasicType
    {
        public override string Type { get; } = "Cold";
        public override List<BasicType> Advantages { get; }
        public override List<BasicType> Disadvantages { get; }
        public override List<BasicType> Immunities { get; }
        public override string Description { get; } = "";

        public ColdType() : base()
        {

        }

        public ColdType(bool newtype) : base(newtype)
        {
            Advantages = new List<BasicType>()
            {
                new NatureType(),
                new AirType(),
                new RockType(),
                new PrimalType()
            };
            Disadvantages = new List<BasicType>()
            {
                new FireType(),
                new ColdType()
            };
            Immunities = new List<BasicType>()
            {

            };
        }

    }
}