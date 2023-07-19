public class Storyline
{
    public int Position { get; private set; }
    public bool Seen { get; private set; }
    public string Text { get; private set; }
    public int Area { get; private set; }

    public Storyline(int position, string text, int area = 3)
    {
        Position = position;
        Text = text;
        Area = area;
    }

    public void SetSeen() => Seen = true;
}