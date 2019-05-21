using System;
using System.Collections.Generic;
using System.Text;

namespace tb
{
    public class Brand
    {
        private string id;
        private string name;
        private string status;
        private string position;
        private string description;
        private string logoUrl;
        private string backgroundUrl;

        public string Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Status { get => status; set => status = value; }
        public string Position { get => position; set => position = value; }
        public string Description { get => description; set => description = value; }
        public string LogoUrl { get => logoUrl; set => logoUrl = value; }
        public string BackgroundUrl { get => backgroundUrl; set => backgroundUrl = value; }
    }
}
