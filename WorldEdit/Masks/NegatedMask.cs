﻿using System;
using WorldEdit.Extents;

namespace WorldEdit.Masks
{
    /// <summary>
    /// Represents a negated mask.
    /// </summary>
    public class NegatedMask : Mask
    {
        private readonly Mask _mask;

        /// <summary>
        /// Initializes a new instance of the <see cref="NegatedMask" /> class with the specified mask.
        /// </summary>
        /// <param name="mask">The mask to negate.</param>
        /// <exception cref="ArgumentNullException"><paramref name="mask" /> is <c>null</c>.</exception>
        public NegatedMask(Mask mask)
        {
            _mask = mask ?? throw new ArgumentNullException(nameof(mask));
        }

        /// <inheritdoc />
        protected override bool TestImpl(Extent extent, Vector position) => !_mask.Test(extent, position);
    }
}
