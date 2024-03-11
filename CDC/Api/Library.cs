//using Api.Repair;
public class Library
{
    
    public int bookId { get; set; } 
    

    public string title { get; set; }
    public int author_id { get; set; }
   
    public int genre_id { get; set; }
   
    public int publication_year { get; set; }

   
     public string Damage { get; set; } 


    public override string ToString()
    {
        return $" Title: {title}, Author ID: {author_id}, Genre ID: {genre_id},Publication Year: {publication_year}";
    }
   
    public decimal EstimatedCost { get; set; }
    public string RepairStatus { get; set; }
}
