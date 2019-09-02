using FilterLibrary.FilterHelp;

namespace FilterLibrary.SortableBindingList
{
    public interface IDynamicFiltrable
    {
        void ApplyFilter(PropertiesFilter filter);
        void RemoveFilter();
    }
}