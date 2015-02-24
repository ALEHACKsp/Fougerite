﻿namespace RustPP.Commands
{
    using Facepunch.Utility;
    using Fougerite;
    using System;
    using System.Linq;
    using System.Collections.Generic;

    internal class SpawnItemCommand : ChatCommand
    {
        public override void Execute(ref ConsoleSystem.Arg Arguments, ref string[] ChatArguments)
        {
            if (ChatArguments.Length > 0)
            {
                int qty = 0;
                int qtyIdx = -1;
                for (var i = 0; i < ChatArguments.Length; i++)
                {
                    string arg = ChatArguments[i];
                    int test;
                    if (int.TryParse(arg, out test))
                    {
                        if (test >= 1 || test <= 7)
                        {
                            if (i - 1 >= 0)
                            {
                                string str = ChatArguments[i - 1].ToUpperInvariant();
                                if (str == "PART" || str == "KIT")
                                    continue;
                            }
                        }
                        if (test == 556)
                        {
                            if (i + 1 < ChatArguments.Length)
                            {
                                string nextArg = ChatArguments[i + 1];
                                if (nextArg.Similarity("Ammo") > 0.749
                                    || nextArg.Similarity("Casing") > 0.799)
                                    continue;
                            }
                        }
                        qty = test;
                        qtyIdx = i;
                    }
                }
                if (qty == 0)
                {
                    qty = 1;
                }
                string quantity = qty.ToString();
                string[] remain = qtyIdx > -1 ? ChatArguments.Slice(0, qtyIdx)
                    .Concat(ChatArguments.Slice(Math.Min(qtyIdx + 1, ChatArguments.Length), ChatArguments.Length))
                    .ToArray() : ChatArguments;

                string itemName = string.Join(" ", remain.ToArray()).MatchItemName();
                Arguments.Args = new string[] { itemName, quantity };
                Logger.LogDebug(string.Format("[SpawnItemCommand] terms={0}, itemName={1}, quantity={2}", string.Join(",", remain.ToArray()), itemName, quantity));
                try
                {
                    Util.sayUser(Arguments.argUser.networkPlayer, Core.Name, string.Format("{0}  {1} were placed in your inventory.", quantity, itemName));
                    inv.give(ref Arguments);
                } 
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }
            else
            {
                Util.sayUser(Arguments.argUser.networkPlayer, Core.Name, "Spawn Item usage:  /i  (quantity)  itemName");
            }
        }
    }
}