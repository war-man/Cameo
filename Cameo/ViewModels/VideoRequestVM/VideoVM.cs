using Cameo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo.ViewModels.VideoRequestVM
{
    public class VideoDetailsVM
    {
        public int RequestID { get; set; }
        public string To { get; set; }

        public string From { get; set; }
        public AttachmentDetailsVM Video { get; set; }

        public VideoDetailsVM() { }

        public VideoDetailsVM(VideoRequest request)
        {
            if (request == null)
                return;

            RequestID = request.ID;
            To = request.To;
            From = request.From;
            Video = new AttachmentDetailsVM(request.Video);
        }
    }
}
