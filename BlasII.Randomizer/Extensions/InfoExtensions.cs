using BlasII.ModdingAPI;
using Il2Cpp;
using System.Text;
using UnityEngine;

namespace BlasII.Randomizer.Extensions;

internal static class InfoExtensions
{
    // Recursive method that returns the entire hierarchy of an object
    public static string DisplayHierarchy(this Transform transform, int maxLevel, bool includeComponents)
    {
        return transform.DisplayHierarchy_INTERNAL(new StringBuilder(), 0, maxLevel, includeComponents).ToString();
    }

    private static StringBuilder DisplayHierarchy_INTERNAL(this Transform transform, StringBuilder currentHierarchy, int currentLevel, int maxLevel, bool includeComponents)
    {
        // Indent
        for (int i = 0; i < currentLevel; i++)
            currentHierarchy.Append('\t');

        // Add this object
        currentHierarchy.Append(transform.name);

        // Add components
        if (includeComponents)
        {
            currentHierarchy.Append(" - ");
            foreach (Component c in transform.GetComponents<Component>())
                currentHierarchy.Append(c.GetIl2CppType().FullName + ", ");
        }
        currentHierarchy.AppendLine();

        // Add children
        if (currentLevel < maxLevel)
        {
            for (int i = 0; i < transform.childCount; i++)
                currentHierarchy = transform.GetChild(i).DisplayHierarchy_INTERNAL(currentHierarchy, currentLevel + 1, maxLevel, includeComponents);
        }

        // Return output
        return currentHierarchy;
    }

    // Displays all states and actions of a playmaker fsm
    public static void DisplayActions(this PlayMakerFSM fsm)
    {
        ModLog.Warn("FSM: " + fsm.name);
        foreach (var state in fsm.FsmStates)
        {
            ModLog.Info("State: " + state.Name);
            foreach (var action in state.Actions)
            {
                ModLog.Error("Action: " + action.Name + ", " + action.GetIl2CppType().Name);
            }
        }
    }
}
