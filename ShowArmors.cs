using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;


namespace Plugin
{
    [ApiVersion(2, 1)]
    public class ShowArmors : TerrariaPlugin
    {
        public override string Author => "FrankV22, Ak";

        public override string Description => "Mostrar Slots de inventario";

        public override string Name => "Show Armors";

        public override Version Version => Assembly.GetExecutingAssembly().GetName().Version;

        public ShowArmors(Main game) : base(game)
        {
        }

        public override void Initialize()
        {

            Commands.ChatCommands.Add(new Command(
                permissions: "ShowArmors",
                cmd: ShowMySlots,
                "mostrar", "show", "zb")
            {
                HelpText = "Muestra tu equipamiento"
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // 清理托管资源
                Commands.ChatCommands.RemoveAll(cmd => cmd.Names.Contains("mostrar") || cmd.Names.Contains("show") || cmd.Names.Contains("zb"));
                // 还可以添加其他托管资源的清理操作
            }

            // 清理非托管资源

            base.Dispose(disposing);
        }

        private void ShowMySlots(CommandArgs args)
        {
            TSPlayer target = null;
            Item[] armors = null;
            string str = "";
            const int MAX_SLOTS_NUMBER = 10;
            int argsCount = args.Parameters.Count;
            bool nothingEquipped = false;

            if (argsCount != 0 && argsCount != 1)
            {
                args.Player.SendErrorMessage($"¡Error de sintaxis! Sintaxis correcta [c/55D284:/mostrar] [c/55D284:<jugador>]");
            }
            else if (argsCount == 0)
            {
                target = args.Player;
                armors = target.TPlayer.armor;
                str = $"{target.Name}" + " : ";
            }
            else if (argsCount == 1)
            {
                var players = TSPlayer.FindByNameOrID(args.Parameters[0]);
                if (players.Count == 0)
                {
                    args.Player.SendErrorMessage("¡Este jugador no existe!");
                }
                else if (players.Count > 1)
                {
                    args.Player.SendMultipleMatchError(players.Select(p => p.Name));
                }
                else
                {
                    target = players[0];
                    armors = target.TPlayer.armor;
                    str = $"{target.Name}" + " : " + "Sosteniendo" + $"[i/p{target.SelectedItem.prefix}:{target.SelectedItem.netID}]" + $"{(ItemPrefix)target.SelectedItem.prefix}";
                }
            }
            for (int i = 0; i < MAX_SLOTS_NUMBER; i++)
            {
                bool isArmor = i < 3;
                bool isAccessories = i < MAX_SLOTS_NUMBER;
                if (armors[i] == null || armors[i].netID == 0)
                {
                    continue;
                }
                else if (isArmor)
                {
                    str += $"[i:{armors[i].netID}]";
                    continue;
                }
                else if (isAccessories)
                {
                    str += $"[i/p{armors[i].prefix}:{armors[i].netID}]" + $"{(ItemPrefix)armors[i].prefix}" + " ";
                }
                else
                {
                    continue;
                }
            }

            nothingEquipped = str == ($"{target.Name}" + " : ");
            if (argsCount == 0)
            {
                if (nothingEquipped)
                {
                    TShock.Utils.Broadcast($"{target.Name}Este no tiene equipo." + "Espera: " + $"[i/p{target.SelectedItem.prefix}:{target.SelectedItem.netID}]" + $"{(ItemPrefix)target.SelectedItem.prefix}", Microsoft.Xna.Framework.Color.Green);
                }
                else
                {
                    TShock.Utils.Broadcast(str += "Sosteniendo: " + $"[i/p{target.SelectedItem.prefix}:{target.SelectedItem.netID}]" + $"{(ItemPrefix)target.SelectedItem.prefix}", Microsoft.Xna.Framework.Color.Green);
                }
            }
            else if (argsCount == 1)
            {
                if (nothingEquipped)
                {
                    args.Player.SendSuccessMessage($"{target.Name}este no tiene equipo，Espera" + $"[i/p{target.SelectedItem.prefix}:{target.SelectedItem.netID}]" + $"{(ItemPrefix)target.SelectedItem.prefix}");
                }
                else
                {
                    args.Player.SendSuccessMessage(str += " Sosteniendo: " + $"[i/p{target.SelectedItem.prefix}:{target.SelectedItem.netID}]" + $"{(ItemPrefix)target.SelectedItem.prefix}");
                }
            }

        }

        public enum ItemPrefix
        {
        }
    
    }
}