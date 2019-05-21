using System;
using System.Collections.Generic;
using System.Text;

namespace tb
{
    public class Category
    {
        private string id;
        private string name;
        private string status;
        private string parentId;
        private string position;
        private string remark;
        private string hotSearch;
        private string imageUrl;
        private string parentCategory;
        private string childrenCategories;

        public string Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Status { get => status; set => status = value; }
        public string ParentId { get => parentId; set => parentId = value; }
        public string Position { get => position; set => position = value; }
        public string Remark { get => remark; set => remark = value; }
        public string HotSearch { get => hotSearch; set => hotSearch = value; }
        public string ImageUrl { get => imageUrl; set => imageUrl = value; }
        public string ParentCategory { get => parentCategory; set => parentCategory = value; }
        public string ChildrenCategories { get => childrenCategories; set => childrenCategories = value; }
    }
}
