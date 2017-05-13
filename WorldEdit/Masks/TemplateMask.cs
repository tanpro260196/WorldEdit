﻿using System;
using WorldEdit.Extents;
using WorldEdit.Templates;

namespace WorldEdit.Masks
{
    /// <summary>
    /// Represents a mask that tests for template matching.
    /// </summary>
    public class TemplateMask : Mask
    {
        private readonly ITemplate _template;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateMask" /> class with the specified template.
        /// </summary>
        /// <param name="template">The template to match with.</param>
        /// <exception cref="ArgumentNullException"><paramref name="template" /> is <c>null</c>.</exception>
        public TemplateMask(ITemplate template)
        {
            _template = template ?? throw new ArgumentNullException(nameof(template));
        }

        /// <inheritdoc />
        protected override bool TestImpl(Extent extent, Vector position) =>
            _template.Matches(extent.GetTile(position));
    }
}
