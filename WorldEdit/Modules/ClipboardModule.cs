﻿using JetBrains.Annotations;
using TShockAPI;

namespace WorldEdit.Modules
{
    /// <summary>
    /// Represents a module that encapsulates the clipboard functionality.
    /// </summary>
    public sealed class ClipboardModule : Module
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClipboardModule" /> class with the specified WorldEdit plugin.
        /// </summary>
        /// <param name="plugin">The WorldEdit plugin, which must not be <c>null</c>.</param>
        public ClipboardModule([NotNull] WorldEditPlugin plugin) : base(plugin)
        {
        }

        /// <inheritdoc />
        public override void Deregister()
        {
        }

        /// <inheritdoc />
        public override void Register()
        {
            var command = Plugin.RegisterCommand("clearclipboard", ClearClipboard, "worldedit.clipboard.clear");
            command.HelpText = "Syntax: /clearclipboard\n" +
                               "Clears your clipboard.";

            command = Plugin.RegisterCommand("/copy", Copy, "worldedit.clipboard.copy");
            command.HelpText = "Syntax: //copy\n" +
                               "Copies your selection to your clipboard.";

            command = Plugin.RegisterCommand("/cut", Cut, "worldedit.clipboard.cut");
            command.HelpText = "Syntax: //cut\n" +
                               "Cuts your selection to your clipboard.";

            command = Plugin.RegisterCommand("/paste", Paste, "worldedit.clipboard.paste");
            command.HelpText = "Syntax: //paste\n" +
                               "Pastes your clipboard to your primary position.";
        }

        private void ClearClipboard(CommandArgs args)
        {
            var player = args.Player;
            var session = Plugin.GetOrCreateSession(player);
            session.Clipboard = null;
            player.SendSuccessMessage("Cleared clipboard.");
        }

        private void Copy(CommandArgs args)
        {
            var player = args.Player;
            var session = Plugin.GetOrCreateSession(player);
            var editSession = session.CreateEditSession();
            session.Clipboard = Clipboard.CopyFrom(editSession, session.Selection);
            player.SendSuccessMessage("Copied clipboard from selection.");
        }

        private void Cut(CommandArgs args)
        {
            var player = args.Player;
            var session = Plugin.GetOrCreateSession(player);
            var editSession = session.CreateEditSession(true);
            session.Clipboard = Clipboard.CopyFrom(editSession, session.Selection);
            editSession.Clear(session.Selection);
            player.SendSuccessMessage("Cut clipboard from selection.");
        }

        private void Paste(CommandArgs args)
        {
            var player = args.Player;
            var session = Plugin.GetOrCreateSession(player);
            var position = session.RegionSelector.PrimaryPosition;
            if (position == null)
            {
                player.SendErrorMessage("Invalid primary position.");
                return;
            }

            var clipboard = session.Clipboard;
            if (clipboard == null)
            {
                player.SendErrorMessage("Invalid clipboard.");
                return;
            }

            var editSession = session.CreateEditSession(true);
            clipboard.PasteTo(editSession, position.Value);
            player.SendSuccessMessage("Pasted clipboard to primary position.");
        }
    }
}
