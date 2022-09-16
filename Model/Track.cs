namespace Model
{
    public class Track
    {
        public string Name { get; set; }
        public LinkedList<Section> Sections { get; set; }

        public Track(string name, SectionType[] sectionTypes)
        {
            Name = name;
            Sections = ConvertToSections(sectionTypes);
        }

        public static LinkedList<Section> ConvertToSections(SectionType[] sectionTypes)
        {
            LinkedList<Section> sections = new();

            foreach (SectionType type in sectionTypes)
            {
                sections.AddLast(new Section(type));
            }

            return sections;
        }
    }
}