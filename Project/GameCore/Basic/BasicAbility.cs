using System.Threading.Tasks;

namespace ProjectOrigin
{
    /// <summary>Framework for event-based abilities.</summary>
    /// -WIP- Very few abilities have been implemented yet, and much functionality is still needed here.
    public class BasicAbility
    {
        /// <summary>Name of the ability.</summary>
        public virtual string Name { get; }
        /// <summary>Description of the ability.</summary>
        public virtual string Description { get; }

        public BasicAbility()
        {

        }

        public BasicAbility(bool newability, BasicMon owner)
        {

        }

        /// <summary>An event handler to be triggered when the mon enters combat.</summary>
        /// <param name="owner">The mon who has this ability.</param>
        /// <param name="inst">The combat instance that triggered this event.</param>
        public virtual async Task mon_EnteredCombat(BasicMon owner, CombatInstance inst)
        {
            await Task.Run(null);
        }

    }
}