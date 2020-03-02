using System;
using System.Linq;

namespace CSharpWorks
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string str = "String has an extension method";
            Console.WriteLine(str.Summary(3));
            int y = 5;
            int z = 8;            
            Func<int,string> mySum = x => Convert.ToString( x + y + z);
            string ss =  mySum(3);
            //Console.WriteLine( mySum(3).ToString());
            VideoEncoder encoder = new VideoEncoder();
            encoder.VideoEncoded += OnVideoEncoded;
            encoder.Encode(new Video() { Title = "My Video"});

            BookRepository b = new BookRepository();
            var books = b.GetBooks();

            var cheaperBooks = from bk in books 
            where bk.Price < 10
            select bk;

            var cheaperBooksOrdered = from bk in books 
            where bk.Price < 10
            orderby bk.Title
            select bk;

            var cheaperBooksLinq = books.Where(bk => bk.Price < 10).
            OrderBy(bk => bk.Title).
            Select(bk => bk.Title);

        }

        private static void OnVideoEncoded(object sender, VideoEventArgs e)
        {
            Console.WriteLine( e.Video.Title);
        }
    }
}
