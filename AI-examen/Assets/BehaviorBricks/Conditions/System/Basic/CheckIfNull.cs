using Pada1.BBCore.Framework;
using Pada1.BBCore;
using UnityEngine;

namespace BBCore.Conditions
{
    /// <summary>
    /// It is a basic condition to check if Booleans have the same value.
    /// </summary>
    [Condition("Basic/CheckIfNull")]
    [Help("Checks whether somthing is null")]
    public class CheckIfNull : ConditionBase
    {
        ///<value>Input First Boolean Parameter.</value>
        [InParam("valueA")]
        [Help("First value to be compared")]
        public GameObject valueA;

        /// <summary>
        /// Checks whether two booleans have the same value.
        /// </summary>
        /// <returns>the value of compare first boolean with the second boolean.</returns>
		public override bool Check()
		{
			return valueA == null;
		}
    }
}