﻿using System;
using System.Linq;
using WorldEdit.Extents;
using WorldEdit.History;
using WorldEdit.Masks;
using WorldEdit.Regions;
using WorldEdit.Templates;

namespace WorldEdit.Sessions
{
    /// <summary>
    /// Represents an edit session whose changes can be considered collectively.
    /// </summary>
    public class EditSession : Extent
    {
        private readonly ChangeSet _changeSet = new ChangeSet();
        private readonly Extent _extent;
        private readonly World _world;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditSession" /> class with the specified world, mask, and limit.
        /// </summary>
        /// <param name="world">The world.</param>
        /// <param name="mask">The mask.</param>
        /// <param name="limit">The limit on the number of tiles that can be set.</param>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="world" /> or <paramref name="mask" /> is <c>null</c>.
        /// </exception>
        public EditSession(World world, Mask mask, int limit)
        {
            _world = world ?? throw new ArgumentNullException(nameof(world));
            var loggedExtent = new LoggedExtent(_world, _changeSet);
            var limitedExtent = new LimitedExtent(loggedExtent, limit);
            _extent = new MaskedExtent(limitedExtent, mask ?? throw new ArgumentNullException(nameof(mask)));
        }

        /// <inheritdoc />
        public override Vector LowerBound => _extent.LowerBound;

        /// <inheritdoc />
        public override Vector UpperBound => _extent.UpperBound;

        /// <summary>
        /// Applies the specified template to the tiles in the region.
        /// </summary>
        /// <param name="template">The template to use.</param>
        /// <param name="region">The region to modify.</param>
        /// <returns>The number of modified tiles.</returns>
        public int ApplyTemplate(ITemplate template, Region region)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }
            if (region == null)
            {
                throw new ArgumentNullException(nameof(region));
            }

            var count = 0;
            foreach (var position in region.Where(IsInBounds))
            {
                if (SetTile(position, template.Apply(GetTile(position))))
                {
                    ++count;
                }
            }
            return count;
        }

        /// <inheritdoc />
        public override Tile GetTile(int x, int y) => _extent.GetTile(x, y);

        /// <summary>
        /// Redoes the edit session.
        /// </summary>
        /// <returns>The number of redone changes.</returns>
        public int Redo() => _changeSet.Redo(_world);

        /// <inheritdoc />
        public override bool SetTile(int x, int y, Tile tile) => _extent.SetTile(x, y, tile);

        /// <summary>
        /// Undoes the edit session.
        /// </summary>
        /// <returns>The number of undone changes.</returns>
        public int Undo() => _changeSet.Undo(_world);
    }
}
