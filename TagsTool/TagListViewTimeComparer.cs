
namespace TagsTool
{
    // **自定义排序器（按时间排序）**
    public class TagListViewTimeComparer : System.Collections.IComparer
    {
        public int Compare(object a, object b)
        {
            ListViewItem itemA = a as ListViewItem;
            ListViewItem itemB = b as ListViewItem;
            if (itemA == null || itemB == null) return 0;
            return string.Compare(itemB.SubItems[1].Text, itemA.SubItems[1].Text);
        }
    }

}
