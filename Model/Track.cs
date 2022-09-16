namespace Model
{
    public class Track
    {
        public string Name { get; set; }
        public LinkedList<Section> Sections { get; set; }

        public Track(string name, ESectionType[] sectionTypes)
        {
            Name = name;
            Sections = new LinkedList<Section>();

            foreach (ESectionType type in sectionTypes)
            {
                Sections.AddLast(new Section(type));
            }
        }
    }
}