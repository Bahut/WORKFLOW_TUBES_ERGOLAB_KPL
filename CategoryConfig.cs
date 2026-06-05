namespace WORKFLOW_TUBES_KPL_ERGOLAB
{
    public class CategoryConfig
    {
        public List<CategoryItem> Categories { get; set; }
    }

    public class CategoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
    }
}
