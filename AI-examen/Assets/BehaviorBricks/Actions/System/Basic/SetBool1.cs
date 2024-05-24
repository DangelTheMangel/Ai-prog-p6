using Pada1.BBCore.Framework;
using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{
    /// <summary>
    /// It is a basic action to associate a Boolean to a variable.
    /// </summary>
    [Action("Basic/SetBool")]
    [Help("Sets a value to a boolean variable")]
    public class SetGameobjectToNull : BasePrimitiveAction
    {
        ///<value>Input Boolean Parameter.</value>
        [InParam("value")]
        [Help("Value")]
        public GameObject value;

        /// <summary>Initialization Method of SetBool.</summary>
        /// <remarks>Initializes the Boolean value.</remarks>
        public override void OnStart()
        {
            value = null;
        }
        /// <summary>Method of Update of SetBool.</summary>
        /// <remarks>Complete the task.</remarks>
        public override TaskStatus OnUpdate()
        {
            return TaskStatus.COMPLETED;
        }
    }
}
