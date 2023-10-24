using System.Collections.Generic;

namespace VimTableExplorer
{
    public interface ITreeViewModel
    {
        string Title { get; }
        IReadOnlyList<ITreeViewModel> Items { get; }
    }
}