public class Library
{
    public string title { get; set; }
    public int author_id { get; set; }
   
    public int genre_id { get; set; }
   
    public int publication_year { get; set; }


    public override string ToString()
    {
        return $" Title: {title}, Author ID: {author_id}, Genre ID: {genre_id},Publication Year: {publication_year}";
    }
}
