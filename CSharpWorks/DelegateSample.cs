using System;

namespace CSharpWorks
{
    public class Video {
        public string Title {get;set;}
    } 
    public class VideoEventArgs {
        public Video Video { get; set; }

    }

    public class VideoEncoder 
    {
        public event EventHandler<VideoEventArgs> VideoEncoded;

        public void Encode(Video video)
        {
            Console.Write("Encoding...");
            OnVideoEncoded(video);
        }

        private void OnVideoEncoded(Video video)
        {
            if( VideoEncoded != null) VideoEncoded(this,new VideoEventArgs() { Video=video} );
        }
    }
}