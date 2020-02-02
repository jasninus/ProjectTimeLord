using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * 
 * As the name implies, Behaviour Trees are a way to code behaviour using a tree of actions. They are used in the Game Kit to control the behaviour of AI for enemies and the boss battle sequence.
 * https://connect-prd-cdn.unity.com/20190130/3793230c-12cd-42d0-98ca-44589339a628_copy_of_beginner_2dgk_components_documentation_39.png.2000x0x1.webp
 * 
 * Theory
During each update of the game, the tree is "ticked", meaning it goes through each child node of the root, going further down if said node has children, etc.
Each node has associated actions, and returns one of three states to its parent:
Success: the node successfully finished its task.
Failure: the node failed the task.
Continue: the node didn't finish the task yet.
Returned state is used by each node parent differently.
For example:
*Selector makes the next child active if the current one returns Failure or Success, and keeps the current one active if it returns Continue.*
*Test nodes call their child nodes and return the child state if the test is true, or return Failure without calling their child nodes if the test is false.*
 * 
 * Here is an example of a very simple behaviour tree:
First, we need to add using BTAI; at the top of the file.
Root aiRoot = BT.Root();
aiRoot.Do(
BT.If(TestVisibleTarget).Do(
BT.Call(Aim),
BT.Call(Shoot)
),
BT.Sequence().Do(
BT.Call(Walk),
BT.Wait(5.0f),
BT.Call(Turn),
BT.Wait(1.0f),
BT.Call(Turn)
)
);
aiRoot should be stored in the class as a member, because you need to call aiRoot.Tick() in the Update function so that Tree actions can be executed.
Let's walk through how the Update works in the aiRoot.Tick():



First, we test if the function TestVisibleTarget returns true. If it does, it goes on to execute the children, which are calling the functions Aim and Shoot

- If the test returns false, the If node returns Failure and the root then goes to the next child. This is a Sequence, which starts by executing its first child
- This calls the function Walk. It returns Success so that the Sequence sets the next child as active and executes it
- The Wait node executes. Because it has to wait for 5 seconds and was just called for the 1st time, it hasn't reached the wait time, so it returns Continue
- Because the Sequence receives a Continue state from its active child, it doesn't change the active child, so it starts from that child on the next Update
- Once the Wait node has been updated enough to reach its timer, it returns Success so that the sequence goes to the next child.


Sequence
Execute child nodes one after another. If a child returns:
== Success: the sequence ticks the next child on the next frame.
== Failure: the sequence returns to the first child on the next frame.
== Continue: the sequence calls that node again on the next frame.


RandomSequence
Execute a random child from its list of children every time it is called. You can specify a list of weights to apply to each child as an int array in the constructor, to make some children more likely to be picked.

Selector
Execute all children in order until one returns Success, then exit without executing the remaining children nodes. If none return Success then this node returns Failure.

Call
Call the given function, which always returns Success.

If
Call the given function.
- If it returns true, it calls the current active child and return its state.
- Otherwise, it returns Failure without calling its children

While
Return Continue as long as the given function returns true (so the next frame when the tree is ticked, it starts from that node again without evaluating all the previous nodes).
Children are executed one after another.
Returns Failure when the function returns false and the loop is broken.


Condition
This node returns Success if the given function returns true, and returns Failure if false.
Useful when chained with other nodes that depend on their children’s result (e.g Sequence, Selector.)

Repeat
Executes all child nodes a given number of times consecutively.
Returns Continue until it reaches the count, where it returns Success.

Wait
Returns Continue until the given time has been reached (starting when first called), where it then returns Success.

Trigger
Allows the setting of a trigger (or unsetting of a trigger if the last argument is set to false) in the given animator. Always returns Success.

SetBool
Allows setting the value of a Boolean Parameter in the given animator. Always returns Success

SetActive Set active/inactive a given GameObject. Always returns Success.
 */
namespace BTAI
{
    public enum BTState
    {
        Failure,
        Success,
        Continue,
        Abort
    }

    public static class BT
    {
        public static Root Root() { return new Root(); }
        public static Sequence Sequence() { return new Sequence(); }
        public static Selector Selector(bool shuffle = false) { return new Selector(shuffle); }
        public static Action RunCoroutine(System.Func<IEnumerator<BTState>> coroutine) { return new Action(coroutine); }
        public static Action Call(System.Action fn) { return new Action(fn); }
        public static ConditionalBranch If(System.Func<bool> fn) { return new ConditionalBranch(fn); }
        public static While While(System.Func<bool> fn) { return new While(fn); }
        public static Condition Condition(System.Func<bool> fn) { return new Condition(fn); }
        public static Repeat Repeat(int count) { return new Repeat(count); }
        public static Wait Wait(float seconds) { return new Wait(seconds); }
        public static Trigger Trigger(Animator animator, string name, bool set = true) { return new Trigger(animator, name, set); }
        public static WaitForAnimatorState WaitForAnimatorState(Animator animator, string name, int layer = 0) { return new WaitForAnimatorState(animator, name, layer); }
        public static SetBool SetBool(Animator animator, string name, bool value) { return new SetBool(animator, name, value); }
        public static SetActive SetActive(GameObject gameObject, bool active) { return new SetActive(gameObject, active); }
        public static WaitForAnimatorSignal WaitForAnimatorSignal(Animator animator, string name, string state, int layer = 0) { return new WaitForAnimatorSignal(animator, name, state, layer); }
        public static Terminate Terminate() { return new Terminate(); }
        public static Log Log(string msg) { return new Log(msg); }
        public static RandomSequence RandomSequence(int[] weights = null) { return new BTAI.RandomSequence(weights); }
    }

    public abstract class BTNode
    {
        public abstract BTState Tick();
    }

    public abstract class Branch : BTNode
    {
        protected int activeChild;
        protected List<BTNode> children = new List<BTNode>();

        public virtual Branch OpenBranch(params BTNode[] children)
        {
            for (var i = 0; i < children.Length; i++)
                this.children.Add(children[i]);
            return this;
        }

        public List<BTNode> Children()
        {
            return children;
        }

        public int ActiveChild()
        {
            return activeChild;
        }

        public virtual void ResetChildren()
        {
            activeChild = 0;
            for (var i = 0; i < children.Count; i++)
            {
                Branch b = children[i] as Branch;
                if (b != null)
                    b.ResetChildren();
            }
        }
    }

    public abstract class Decorator : BTNode
    {
        protected BTNode child;
        public Decorator Do(BTNode child)
        {
            this.child = child;
            return this;
        }
    }

    public class Sequence : Branch
    {
        public override BTState Tick()
        {
            var childState = children[activeChild].Tick();
            switch (childState)
            {
                case BTState.Success:
                    activeChild++;
                    if (activeChild == children.Count)
                    {
                        activeChild = 0;
                        return BTState.Success;
                    }
                    else
                        return BTState.Continue;
                case BTState.Failure:
                    activeChild = 0;
                    return BTState.Failure;
                case BTState.Continue:
                    return BTState.Continue;
                case BTState.Abort:
                    activeChild = 0;
                    return BTState.Abort;
            }
            throw new System.Exception("This should never happen, but looks like it did!");
        }
    }

    public class Selector : Branch
    {
        public Selector(bool shuffle)
        {
            if (shuffle)
            {
                var n = children.Count;
                while (n > 1)
                {
                    n--;
                    var k = Mathf.FloorToInt(Random.value * (n + 1));
                    var value = children[k];
                    children[k] = children[n];
                    children[n] = value;
                }
            }
        }

        public override BTState Tick()
        {
            var childState = children[activeChild].Tick();
            switch (childState)
            {
                case BTState.Success:
                    activeChild = 0;
                    return BTState.Success;
                case BTState.Failure:
                    activeChild++;
                    if (activeChild == children.Count)
                    {
                        activeChild = 0;
                        return BTState.Failure;
                    }
                    else
                        return BTState.Continue;
                case BTState.Continue:
                    return BTState.Continue;
                case BTState.Abort:
                    activeChild = 0;
                    return BTState.Abort;
            }
            throw new System.Exception("This should never happen, but looks like it did!");
        }
    }

    public class Action : BTNode
    {
        System.Action fn;
        System.Func<IEnumerator<BTState>> coroutineFactory;
        IEnumerator<BTState> coroutine;
        public Action(System.Action fn)
        {
            this.fn = fn;
        }
        public Action(System.Func<IEnumerator<BTState>> coroutineFactory)
        {
            this.coroutineFactory = coroutineFactory;
        }
        public override BTState Tick()
        {
            if (fn != null)
            {
                fn();
                return BTState.Success;
            }
            else
            {
                if (coroutine == null)
                    coroutine = coroutineFactory();
                if (!coroutine.MoveNext())
                {
                    coroutine = null;
                    return BTState.Success;
                }
                var result = coroutine.Current;
                if (result == BTState.Continue)
                    return BTState.Continue;
                else
                {
                    coroutine = null;
                    return result;
                }
            }
        }

        public override string ToString()
        {
            return "Action : " + fn.Method.ToString();
        }
    }

    public class Condition : BTNode
    {
        public System.Func<bool> fn;

        public Condition(System.Func<bool> fn)
        {
            this.fn = fn;
        }
        public override BTState Tick()
        {
            return fn() ? BTState.Success : BTState.Failure;
        }

        public override string ToString()
        {
            return "Condition : " + fn.Method.ToString();
        }
    }

    public class ConditionalBranch : Block
    {
        public System.Func<bool> fn;
        bool tested = false;
        public ConditionalBranch(System.Func<bool> fn)
        {
            this.fn = fn;
        }
        public override BTState Tick()
        {
            if (!tested)
                tested = fn();
            if (tested)
            {
                var result = base.Tick();
                if (result == BTState.Continue)
                    return BTState.Continue;
                else
                {
                    tested = false;
                    return result;
                }
            }
            else
            {
                return BTState.Failure;
            }
        }
        
        public override string ToString()
        {
            return "ConditionalBranch : " + fn.Method.ToString();
        }
    }

    public class While : Block
    {
        public System.Func<bool> fn;

        public While(System.Func<bool> fn)
        {
            this.fn = fn;
        }

        public override BTState Tick()
        {
            if (fn())
                base.Tick();
            else
            {
                ResetChildren();
                return BTState.Failure;
            }

            return BTState.Continue;
        }

        public override string ToString()
        {
            return "While: " + fn.Method.ToString();
        }
    }

    public abstract class Block : Branch
    {
        public override BTState Tick()
        {
            switch (children[activeChild].Tick())
            {
                case BTState.Continue:
                    return BTState.Continue;
                default:
                    activeChild++;
                    if (activeChild == children.Count)
                    {
                        activeChild = 0;
                        return BTState.Success;
                    }
                    return BTState.Continue;
            }
        }
    }

    public class Root : Block
    {
        public bool isTerminated = false;

        public override BTState Tick()
        {
            if (isTerminated)
                return BTState.Abort;
            while (true)
            {
                switch (children[activeChild].Tick())
                {
                    case BTState.Continue:
                        return BTState.Continue;
                    case BTState.Abort:
                        isTerminated = true;
                        return BTState.Abort;
                    default:
                        activeChild++;
                        if (activeChild == children.Count)
                        {
                            activeChild = 0;
                            return BTState.Success;
                        }
                        continue;
                }
            }
        }
    }

    public class Repeat : Block
    {
        public int count = 1;
        int currentCount = 0;
        public Repeat(int count)
        {
            this.count = count;
        }
        public override BTState Tick()
        {
            if (count > 0 && currentCount < count)
            {
                var result = base.Tick();
                switch (result)
                {
                    case BTState.Continue:
                        return BTState.Continue;
                    default:
                        currentCount++;
                        if (currentCount == count)
                        {
                            currentCount = 0;
                            return BTState.Success;
                        }
                        return BTState.Continue;
                }
            }
            return BTState.Success;
        }

        public override string ToString()
        {
            return "Repeat Until : " + currentCount + " / " + count;
        }
    }

    public class RandomSequence : Block
    {
        int[] weightArray = null;
        int[] addedWeightArray = null;

        public RandomSequence(int[] weight = null)
        {
            activeChild = -1;

            weightArray = weight;
        }

        public override Branch OpenBranch(params BTNode[] children)
        {
            addedWeightArray = new int[children.Length];

            for (int i = 0; i < children.Length; ++i)
            {
                int weight = 0;
                int previousWeight = 0;

                if (weightArray == null || weightArray.Length <= i)
                {
                    weight = 1;
                }
                else
                    weight = weightArray[i];

                if (i > 0)
                    previousWeight = addedWeightArray[i - 1];

                addedWeightArray[i] = weight + previousWeight;
            }

            return base.OpenBranch(children);
        }

        public override BTState Tick()
        {
            if (activeChild == -1)
                PickNewChild();

            var result = children[activeChild].Tick();

            switch (result)
            {
                case BTState.Continue:
                    return BTState.Continue;
                default:
                    PickNewChild();
                    return result;
            }
        }

        void PickNewChild()
        {
            int choice = Random.Range(0, addedWeightArray[addedWeightArray.Length - 1]);

            for (int i = 0; i < addedWeightArray.Length; ++i)
            {
                if (choice - addedWeightArray[i] >= 0)
                {
                    activeChild = i;
                    break;
                }
            }
        }

        public override string ToString()
        {
            return "Random Sequence : " + activeChild + "/" + children.Count;
        }
    }

    public class Wait : BTNode
    {
        public float seconds = 0;
        float future = -1;
        public Wait(float seconds)
        {
            this.seconds = seconds;
        }

        public override BTState Tick()
        {
            if (future < 0)
                future = Time.time + seconds;

            if (Time.time > future)
            {
                future = -1;
                return BTState.Success;
            }
            else
                return BTState.Continue;
        }

        public override string ToString()
        {
            return "Wait : " + (future - Time.time) + " / " + seconds;
        }
    }

    public class Trigger : BTNode
    {
        Animator animator;
        int id;
        string triggerName;
        bool set = true;

        public Trigger(Animator animator, string name, bool set = true)
        {
            this.id = Animator.StringToHash(name);
            this.animator = animator;
            this.triggerName = name;
            this.set = set;
        }

        public override BTState Tick()
        {
            if (set)
                animator.SetTrigger(id);
            else
                animator.ResetTrigger(id);

            return BTState.Success;
        }

        public override string ToString()
        {
            return "Trigger : " + triggerName;
        }
    }

    public class SetBool : BTNode
    {
        Animator animator;
        int id;
        bool value;
        string triggerName;

        public SetBool(Animator animator, string name, bool value)
        {
            this.id = Animator.StringToHash(name);
            this.animator = animator;
            this.value = value;
            this.triggerName = name;
        }

        public override BTState Tick()
        {
            animator.SetBool(id, value);
            return BTState.Success;
        }

        public override string ToString()
        {
            return "SetBool : " + triggerName + " = " + value.ToString();
        }
    }

    public class WaitForAnimatorState : BTNode
    {
        Animator animator;
        int id;
        int layer;
        string stateName;

        public WaitForAnimatorState(Animator animator, string name, int layer = 0)
        {
            this.id = Animator.StringToHash(name);
            if (!animator.HasState(layer, this.id))
            {
                Debug.LogError("The animator does not have a state: " + name);
            }
            this.animator = animator;
            this.layer = layer;
            this.stateName = name;
        }

        public override BTState Tick()
        {
            var state = animator.GetCurrentAnimatorStateInfo(layer);
            if (state.fullPathHash == this.id || state.shortNameHash == this.id)
                return BTState.Success;
            return BTState.Continue;
        }

        public override string ToString()
        {
            return "Wait For State : " + stateName;
        }
    }

    public class SetActive : BTNode
    {
        GameObject gameObject;
        bool active;

        public SetActive(GameObject gameObject, bool active)
        {
            this.gameObject = gameObject;
            this.active = active;
        }

        public override BTState Tick()
        {
            gameObject.SetActive(this.active);
            return BTState.Success;
        }

        public override string ToString()
        {
            return "Set Active : " + gameObject.name + " = " + active;
        }
    }

    public class WaitForAnimatorSignal : BTNode
    {
        internal bool isSet = false;
        string name;
        int id;

        public WaitForAnimatorSignal(Animator animator, string name, string state, int layer = 0)
        {
            this.name = name;
            this.id = Animator.StringToHash(name);
            if (!animator.HasState(layer, this.id))
            {
                Debug.LogError("The animator does not have state: " + name);
            }
            else
            {
                //SendSignal.Register(animator, name, this);
            }
        }

        public override BTState Tick()
        {
            if (!isSet)
                return BTState.Continue;
            else
            {
                isSet = false;
                return BTState.Success;
            }
        }

        public override string ToString()
        {
            return "Wait For Animator Signal : " + name;
        }
    }

    public class Terminate : BTNode
    {
        public override BTState Tick()
        {
            return BTState.Abort;
        }
    }

    public class Log : BTNode
    {
        string msg;

        public Log(string msg)
        {
            this.msg = msg;
        }

        public override BTState Tick()
        {
            Debug.Log(msg);
            return BTState.Success;
        }
    }
}

#if UNITY_EDITOR
namespace BTAI
{
    public interface IBTDebugable
    {
        Root GetAIRoot();
    }
}
#endif