#if NET35
using IReadOnlyListBaseReloc = System.Collections.Generic.IList<AsmResolver.PE.Relocations.BaseRelocation>;
#else
using IReadOnlyListBaseReloc = System.Collections.Generic.IReadOnlyList<AsmResolver.PE.Relocations.BaseRelocation>;
#endif


namespace AsmResolver.PE.Relocations
{
    /// <summary>
    /// Pairs a segment with relocation information.
    /// </summary>
    public readonly struct RelocatableSegment
    {
        /// <summary>
        /// Creates a new pairing between a segment and relocation information.
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <param name="relocations">The relocation information.</param>
        public RelocatableSegment(ISegment segment, IReadOnlyListBaseReloc relocations)
        {
            Segment = segment;
            Relocations = relocations;
        }

        /// <summary>
        /// Gets the segment that is relocatable.
        /// </summary>
        public ISegment Segment
        {
            get;
        }

        /// <summary>
        /// Gets the relocation information required to relocate the segment.
        /// </summary>
        public IReadOnlyListBaseReloc Relocations
        {
            get;
        }
    }
}
